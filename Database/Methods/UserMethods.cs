using Composition.Interfaces;
using Composition.Models;
using Microsoft.AspNetCore.Identity;
using Composition.ViewModels;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

namespace Composition.Database.Methods
{
    public class UserMethods : IUserRepository
    {
        private readonly UserManager<User> userManager;
        private readonly AppDBContext appDBContext;
        private readonly IUserPicturesRepository picturesMethods;
        public UserMethods(UserManager<User> _userManager, AppDBContext _appDBContext, IUserPicturesRepository userPicturesRepository)
        {
            userManager = _userManager;
            appDBContext = _appDBContext;
            picturesMethods = userPicturesRepository;
        }
        public async Task<bool> Create(RegisterViewModel? viewModel)
        {
            if (viewModel != null)
            {
                User user = new User { Email = viewModel.Email, UserName = viewModel.Name, Year = viewModel.Year };
                await userManager.CreateAsync(user, viewModel.Password);
                return true;
            }
            return false;
        }
        public async Task<bool> DeleteByEmail(string? email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            } // email != null
            User? user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                await userManager.DeleteAsync(user);
                return true;
            } // user != null
            return false;
        }
        public async Task<List<User>> GetAll()
        {
            List<User> users = await appDBContext.Users
                .Include(a => a.Orders)
                .Include(c => c.UserPictures)
                .ToListAsync();
            return users;
        }
        public async Task<User?> GetByEmail(string? email)
        {
            if (string.IsNullOrEmpty(email))
                return null;  // email != null
            User? user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return null; // user != null
            return user; 
        }
        public async Task<bool> Update(User? updatedModel)
        {
            if(updatedModel == null)
                return false;
            if(updatedModel.Email.IsNullOrEmpty())
                return false;
            else
            {
                User? user = await GetByEmail(updatedModel.Email);
                if(user == null)
                    return false;
                else
                {
                    user.UserName = updatedModel.UserName;
                    user.Year = updatedModel.Year;
                    UserPicture? pic = await picturesMethods.GetById(updatedModel.MainPicture);
                    if (pic != null)
                        await picturesMethods.SetMainPicture(pic);
                    await userManager.UpdateAsync(user);
                    return true;
                }
            }
        }
        public async Task<bool> ResetOrderState(User? updatedUser)
        {
            if(updatedUser == null)
                return false;
            updatedUser.CurrentOrderId = Guid.Empty;
            await appDBContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> AddNewOrder(Order? order, string? email)
        {
            if (email == null)
                return false;
            if(order == null)
                return false;
            User? user = await GetByEmail(email);
            if(user == null)
                return false;
            user.CurrentOrderId = order.Id;
            user.Orders.Add(order);
            await userManager.UpdateAsync(user);
            await appDBContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdatePassword(string? email, string? password)
        {
            if (string.IsNullOrEmpty(email))
                return false;
            if (string.IsNullOrEmpty(password))
                return false;
            User? user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return false;
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            await userManager.ResetPasswordAsync(user, token, password);
            await userManager.UpdateAsync(user);
            return true;
        }
    }
}
