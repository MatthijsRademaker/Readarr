namespace WishListNotification
{
    using Microsoft.EntityFrameworkCore;
    using Supabase;
    using Supabase.Gotrue;
    using Client = Supabase.Client;

    public class CompanionAppDbContext : DbContext
    {
        private Client _client { get; set; }
        public IEnumerable<User>? Users => _client.AdminAuth("TODO").ListUsers().Result?.Users;

        public DbSet<WishListItem>? WishList { get; set; }

        public CompanionAppDbContext()
        {
            var url = "https://vbwlwsukbtejxcrzrmel.supabase.co";
            var key =
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InZid2x3c3VrYnRlanhjcnpybWVsIiwicm9sZSI6ImFub24iLCJpYXQiOjE3MTc5NjE1NzUsImV4cCI6MjAzMzUzNzU3NX0.jJd744ZSlyrrPqcMusBCg3VBUXEOMagYVI-N8pmhlh4";

            var options = new SupabaseOptions { AutoConnectRealtime = true };

            _client = new Client(url, key, options);
            _client.InitializeAsync();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "postgres://postgres.vbwlwsukbtejxcrzrmel:<PassWord>@aws-0-eu-central-1.pooler.supabase.com:6543/postgres"
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WishListItem>(builder =>
            {
                builder.ToTable("WishListItems");
                builder.Property(product => product.UserId).HasColumnName("user_id").IsRequired();
                builder.Property(product => product.BookId).HasColumnName("book_id").IsRequired();
                builder.Property(product => product.CreatedAt).HasColumnName("created_at");
            });
        }
    }

    public class WishListItem
    {
        public WishListItem() { }

        public int Id { get; set; }

        public string? UserId { get; set; }

        public int BookId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
