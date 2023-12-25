using Composition.Interfaces;
using Composition.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Composition.Database.Methods
{
    public class UserPicturesMethods : IUserPicturesRepository
    {
        private readonly AppDBContext appDBContext;
        private readonly UserManager<User> userManager;
        public UserPicturesMethods(UserManager<User> _userManager, AppDBContext _appDBContext)
        {
            appDBContext = _appDBContext;
            userManager = _userManager;
        }
        public async Task<bool> Create(UserPicture? userPicture)
        {
            if (userPicture == null)
                return false;
            if (userPicture.User.Email == null)
                return false;
            User? user = await userManager.FindByEmailAsync(userPicture.User.Email);
            if(user == null)
                return false;
            if (user.UserPictures == null)
            {
                await appDBContext.UsersPictures.AddAsync(userPicture);
                List<UserPicture> userPictures = new List<UserPicture> { userPicture };
                user.UserPictures = userPictures;
            }
            else
            {
                await appDBContext.UsersPictures.AddAsync(userPicture);
                List<UserPicture> userPictures = user.UserPictures;
                userPictures.Add(userPicture);
            }
            await userManager.UpdateAsync(user);
            return true;
        }
        public async Task<bool> DeleteById(Guid? pictureId)
        {
            if (pictureId == null)
                return false;
            UserPicture? userPicture = await appDBContext.UsersPictures.FindAsync(pictureId);
            if (userPicture == null)
                return false;
            bool isMain = await IsMainPicture(pictureId);
            if (isMain == true)
            {
                User? user = userPicture.User;
                if (user == null)
                    return false;
                user.MainPicture = Guid.Empty;
            }
            appDBContext.UsersPictures.Remove(userPicture);
            await appDBContext.SaveChangesAsync();
            return true;
        }
        public async Task<List<UserPicture>?> GetAll(Guid? userId)
        {
            if (userId == null)
                return null;
            List<UserPicture>? userPictures = await appDBContext.UsersPictures.Where(x => x.UserId == userId).ToListAsync();
            return userPictures;
        }
        public async Task<UserPicture?> GetById(Guid? pictureId)
        {
            if(pictureId == null)
                return null;
            UserPicture? userPicture = await appDBContext.UsersPictures.FindAsync(pictureId);
            return userPicture;
        }
        public async Task<bool> SetMainPicture(UserPicture? userPicture)
        {
            if(userPicture == null)
                return false;
            User user = userPicture.User;
            user.MainPicture = userPicture.Id;
            await userManager.UpdateAsync(user);
            return true;
        }
        private async Task<bool> IsMainPicture(Guid? id)
        {
            UserPicture? userPicture = await GetById(id);
            if (userPicture == null)
                return false;
            if(userPicture.User.MainPicture == id)
                return true;
            return false;
        }
    }
}
