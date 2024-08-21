using System.Collections.Generic;
using System.Linq;
using Supabase.Gotrue;

#nullable enable
namespace WishListNotification
{
    public class Repository : IRepository
    {
        private readonly CompanionAppDbContext _companionAppDbContext;

        public Repository(CompanionAppDbContext companionAppDbContext)
        {
            _companionAppDbContext = companionAppDbContext;
        }

        public IEnumerable<User>? GetAllUsers()
        {
            var userList = _companionAppDbContext.Client.AdminAuth("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InZid2x3c3VrYnRlanhjcnpybWVsIiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImlhdCI6MTcxNzk2MTU3NSwiZXhwIjoyMDMzNTM3NTc1fQ.n4Bgtd5gTDgRC57KDweIApZAMYNDGtqRv5bH0x4IVUI").ListUsers().Result;
            return userList?.Users;
        }

        public IEnumerable<WishListItem> GetWishListForUser(string userId)
        {
            var wishList = GetWishList() ?? throw new System.Exception("No wishlist found");
            return wishList!.Where(item => item.UserId == userId).ToList();
        }

        public IEnumerable<WishListItem> GetWishListItemsBasedOnBookId(IEnumerable<string> bookIds)
        {
            var wishList = GetWishList() ?? throw new System.Exception("No wishlist found");
            return wishList!.Where(item => bookIds.Contains(item.BookId)).ToList();
        }

        private IEnumerable<WishListItem> GetWishList()
        {
            var result = _companionAppDbContext.Client.From<WishListItem>().Get().GetAwaiter().GetResult();

            return result.Models;
        }
    }
}
#nullable disable
