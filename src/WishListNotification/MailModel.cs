namespace WishListNotification
{
    internal class MailModel
    {
        public Addressee From =
            new() { Email = "noreply@servarr-companion-app.com", Name = "Prowlarr" };

        public IEnumerable<Addressee>? To { get; set; }

        public string Subject = "Good news, a book from your wishlist is available!";

        public string TemplateId = "neqvygmo298l0p7w";

        public IEnumerable<PersonalizationItem>? Personalization { get; set; }
    }

    public class PersonalizationItem
    {
        public string? Email { get; set; }

        public Dictionary<string, object>? Data { get; set; }
    }

    public class BooksPersonilizationItem
    {
        public string? Url { get; set; }
        public string? Title { get; set; }
        public string? Image { get; set; }
    }

    public class Addressee
    {
        public string? Email { get; set; }
        public string? Name { get; set; }
    }
}
