using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using NLog;
using NzbDrone.Core.Download;

namespace WishListNotification
{
    public class WishListNotifier : IWishListNotifier
    {
        private readonly Logger _logger;
        private readonly HttpClient _httpClient;
        private readonly IRepository _repository;

        public WishListNotifier(Logger logger, HttpClient httpClient, IRepository repository)
        {
            _logger = logger;
            _httpClient = httpClient;
            _repository = repository;

            _logger.Info("Starting WishListNotifier");
            _httpClient.DefaultRequestHeaders.Add(
                "X-Smtp2go-Api-Key",
                "api-EE2A1F1EDED041CDA5EBF7358C7E8E60");
        }

        public void NotifyUsers(DownloadCompletedEvent message)
        {
            _logger.Info("Handling download completed event for {0}", message.TrackedDownload.RemoteBook.Books.First().Title);

            var book = message.TrackedDownload.RemoteBook.Books.FirstOrDefault();

            var edition = book?.Editions?.Value.FirstOrDefault();

            if (edition == null)
            {
                _logger.Info("No edition found for the book");
                return;
            }

            var imgUrl = edition?.Images.FirstOrDefault()?.Url;

            var wishList = _repository
                .GetWishListItemsBasedOnBookId(message.TrackedDownload.RemoteBook.Books.Select(book => book.Id.ToString()))
                .Select(item => item.UserId);

            var users = _repository.GetAllUsers()?.Where(user => wishList.Contains(user.Id));

            if (users == null || users.Count() == 0)
            {
                _logger.Info("No users found for the wish list item");
                return;
            }

            foreach (var user in users)
            {
                user.UserMetadata.TryGetValue("display_name", out var displayName);

                if (user.Email != null)
                {
                    var bookData = new BooksPersonilizationItem()
                    {
                        ImgUrl = imgUrl ?? "Not found",
                        Title = edition?.Title ?? "Not found",
                        Url =
                            $"https://servarr-companion-app.vercel.app/readarr/books/indexed/{book.Id.ToString()}"
                    };

                    var mailModel = new MailModel()
                    {
                        To = new List<string>() { user!.Email },
                        TemplateData = new PersonalizationItem()
                        {
                            Book = bookData,
                            DisplayName = displayName?.ToString() ?? "Unknown"
                        }
                    };

                    var requestUri = "https://api.smtp2go.com/v3/email/send";
                    var jsonString = JsonSerializer.Serialize(mailModel);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
                    {
                        Content = content
                    };

                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = _httpClient.SendAsync(request, CancellationToken.None).GetAwaiter().GetResult();

                    if (!response.IsSuccessStatusCode)
                    {
                        var resultContent = response.Content.ReadAsStringAsync().Result;
                        _logger.Error("Failed to send email notification {0}", resultContent);
                    }
                }
            }
        }
    }

    public interface IWishListNotifier
    {
        void NotifyUsers(DownloadCompletedEvent message);
    }
}
