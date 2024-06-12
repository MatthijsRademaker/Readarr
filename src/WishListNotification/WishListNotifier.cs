using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Hosting;
using NzbDrone.Core.Download;
using NzbDrone.Core.Messaging.Events;

namespace WishListNotification
{
    public class WishListNotifier : IHostedService, IHandleAsync<DownloadCompletedEvent>
    {
        private readonly HttpClient httpClient;
        private readonly Repository repository;

        public WishListNotifier(HttpClient httpClient, Repository repository)
        {
            this.httpClient = httpClient;
            this.repository = repository;
        }

        public void HandleAsync(DownloadCompletedEvent message)
        {
            var edition = message.TrackedDownload.RemoteBook.Books
                .FirstOrDefault()
                ?.Editions?.Value.FirstOrDefault();

            if (edition != null)
                return;

            var imgUrl = edition?.Images.FirstOrDefault()?.Url;
            var wishList = repository
                .GetWishListItemsBasedOnBookId(
                    message.TrackedDownload.RemoteBook.Books.Select(book => book.Id)
                )
                .Select(item => item.UserId);
            var users = repository.GetAllUsers()?.Where(user => wishList.Contains(user.Id));
            if (users == null)
                return;

            var sendTo = new List<Addressee>();

            var personilization = new List<PersonalizationItem>();

            foreach (var user in users)
            {
                user.UserMetadata.TryGetValue("display_name", out var displayName);

                if (user.Email != null && displayName != null)
                {
                    var addressee = new Addressee()
                    {
                        Email = user.Email!,
                        Name = displayName.ToString()!
                    };

                    sendTo.Add(addressee);

                    var personilizationData = new Dictionary<string, object>();
                    var bookData = new BooksPersonilizationItem()
                    {
                        Image = imgUrl ?? "Not found",
                        Title = edition?.Title ?? "Not found",
                        Url =
                            $"https://servarr-companion-app.vercel.app/readarr/books/indexed/{edition?.Id}"
                    };

                    personilizationData.Add("books", bookData);
                }
            }

            var mailModel = new MailModel() { To = sendTo, Personalization = personilization };
            httpClient.PostAsJsonAsync(
                "https://api.mailersend.com/v1/email",
                mailModel,
                CancellationToken.None
            );
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                "mlsn.a39e0341d9b3b350b861d64b294e76b670cb3ca16036e38a3bd32c865d9aa9d6"
            );
            await Task.Delay(-1);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
