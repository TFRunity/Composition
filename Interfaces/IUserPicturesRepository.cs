using Composition.Models;
namespace Composition.Interfaces
{
    public interface IUserPicturesRepository
    {
        /// <summary>
        /// Adds picture(to list)/Create picture(create list {picture})
        /// </summary>
        /// <param name="userPicture">nullable param</param>
        /// <returns>false/true</returns>
        public Task<bool> Create(UserPicture? userPicture);
        /// <summary>
        /// Delete picture
        /// </summary>
        /// <param name="pictureId">nullable param</param>
        /// <returns>false/true</returns>
        public Task<bool> DeleteById(Guid? pictureId);
        /// <summary>
        /// Returns picture
        /// </summary>
        /// <param name="pictureId">nullable param</param>
        /// <returns>picture/null</returns>
        public Task<UserPicture?> GetById(Guid? pictureId);
        /// <summary>
        /// Returns pictures of current user
        /// </summary>
        /// <param name="userId">nullable param</param>
        /// <returns>All pictures</returns>
        public Task<List<UserPicture>?> GetAll(Guid? userId);
        /// <summary>
        /// Sets mainpicture to user
        /// </summary>
        /// <param name="userPicture">mainpicture userpicture</param>
        /// <returns>false/true</returns>
        public Task<bool> SetMainPicture(UserPicture? userPicture);
    }
}
