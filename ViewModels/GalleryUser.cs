using Composition.Models;

namespace Composition.ViewModels
{
    public class GalleryUser
    {
        public GalleryUser(User user)
        {
            Email = user.Email;
            UsersPictures = user.UserPictures;
            Name = user.UserName;
        }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public List<UserPicture> UsersPictures { get; set; }
    }
}
