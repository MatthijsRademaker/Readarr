using System;
using Supabase;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using Client = Supabase.Client;

#nullable enable
namespace WishListNotification
{
    public class CompanionAppDbContext
    {
        public Client Client { get; set; }

        public CompanionAppDbContext()
        {
            var url = "https://vbwlwsukbtejxcrzrmel.supabase.co";
            var key =
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InZid2x3c3VrYnRlanhjcnpybWVsIiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImlhdCI6MTcxNzk2MTU3NSwiZXhwIjoyMDMzNTM3NTc1fQ.n4Bgtd5gTDgRC57KDweIApZAMYNDGtqRv5bH0x4IVUI";

            var options = new SupabaseOptions { AutoConnectRealtime = true };

            Client = new Client(url, key, options);
            Client.InitializeAsync().Wait();
        }
    }

    [Table("WishList")]
    public class WishListItem : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("user_id")]
        public string UserId { get; set; } = "";

        [Column("book_id")]
        public string BookId { get; set; } = "";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
#nullable disable
