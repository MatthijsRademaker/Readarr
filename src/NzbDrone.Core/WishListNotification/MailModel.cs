#nullable enable
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WishListNotification
{
    internal class MailModel
    {
        [JsonPropertyName("sender")]
        public string Sender => "prowlarr@servarr-companion-app.com";

        [JsonPropertyName("to")]
        public IEnumerable<string>? To { get; set; }

        [JsonPropertyName("template_id")]
        public string TemplateId => "7898349";

        [JsonPropertyName("template_data")]
        public PersonalizationItem? TemplateData { get; set; }
    }

    public class PersonalizationItem
    {
        [JsonPropertyName("displayName")]
        public string? DisplayName { get; set; }

        [JsonPropertyName("book")]
        public BooksPersonilizationItem? Book { get; set; }
    }

    public class BooksPersonilizationItem
    {
        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("imgUrl")]
        public string? ImgUrl { get; set; }
    }
}
#nullable disable
