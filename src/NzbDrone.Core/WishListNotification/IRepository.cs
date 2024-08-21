using System.Collections.Generic;
using Supabase.Gotrue;

namespace WishListNotification
{
    public interface IRepository
    {
        IEnumerable<User> GetAllUsers();
        IEnumerable<WishListItem> GetWishListForUser(string userId);
        IEnumerable<WishListItem> GetWishListItemsBasedOnBookId(IEnumerable<string> bookIds);
    }
}
