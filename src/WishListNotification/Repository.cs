using Supabase.Gotrue;

namespace WishListNotification
{
    public class Repository
    {
        private readonly CompanionAppDbContext companionAppDbContext;

        public Repository(CompanionAppDbContext companionAppDbContext)
        {
            this.companionAppDbContext = companionAppDbContext;
        }

        public IEnumerable<User>? GetAllUsers()
        {
            return companionAppDbContext.Users;
        }

        public IEnumerable<WishListItem> GetWishListForUser(string userId) =>
            companionAppDbContext.WishList!.Where(item => item.UserId == userId).ToList();

        public IEnumerable<WishListItem> GetWishListItemsBasedOnBookId(IEnumerable<int> bookIds) =>
            companionAppDbContext.WishList!.Where(item => bookIds.Contains(item.BookId)).ToList();
    }
}
