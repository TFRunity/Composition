using Composition.Models;

namespace Composition.ViewModels
{
    public class WebUser
    {
        public WebUser(User user)
        {
            Name = user.UserName;
            UserPictures = user.UserPictures;
            Year = user.Year;
            MainPicture = user.MainPicture;
            OrderId = user.CurrentOrderId;
            Orders = user.Orders;
            Email = user.Email;
        }
        public string? Name { get; set; }
        public List<UserPicture> UserPictures { get; set; }
        public int Year { get; set; }
        public Guid? MainPicture { get; set; }
        public string? Email { get; set; }
        public Guid OrderId { get; set; }
        public List<Order> Orders { get; set; }
    }
}
