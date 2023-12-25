using Composition.Models;
using Composition.ViewModels;
namespace Composition.Interfaces
{
    public interface IUserRepository
    {
        /// <summary>
        /// Simple user creating
        /// </summary>
        /// <param name="viewModel">nullable viewmodel</param>
        /// <returns>false/true</returns>
        public Task<bool> Create(RegisterViewModel? viewModel);
        /// <summary>
        /// Updating of user if all params are ok
        /// </summary>
        /// <param name="updatedmodel">nullable updated user</param>
        /// <returns>false/true</returns>
        public Task<bool> Update(User? updatedmodel);
        /// <summary>
        /// Deleting user by it's email
        /// </summary>
        /// <param name="email">nullable email string</param>
        /// <returns>false/true</returns>
        public Task<bool> DeleteByEmail(string? email);
        /// <summary>
        /// Returns user by it's email
        /// </summary>
        /// <param name="email">nullable email string</param>
        /// <returns>User/null</returns>
        public Task<User?> GetByEmail(string? email);
        /// <summary>
        /// Caution: Not to use at prod
        /// </summary>
        /// <returns>List of all users</returns>
        public Task<List<User>> GetAll();
        /// <summary>
        /// Password's update
        /// </summary>
        /// <param name="email">nullable email string</param>
        /// <param name="password">nullable password string</param>
        /// <returns>false/true</returns>
        public Task<bool> UpdatePassword(string? email, string? password);
        /// <summary>
        /// Set currentorderid to Guid.Empty
        /// </summary>
        /// <param name="updatedUser">nullable updated user</param>
        /// <returns>false/true</returns>
        public Task<bool> ResetOrderState(User? updatedUser);
        /// <summary>
        /// Add a new order to user's history
        /// </summary>
        /// <param name="order">nullable order</param>
        /// <param name="email">nullable user's email string</param>
        /// <returns>false/true</returns>
        public Task<bool> AddNewOrder(Order? order, string? email);
    }
}
