using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Composition.Models
{
    public class User : IdentityUser<Guid>
    {
        [Range(1, 2023)]
        public int Year { get; set; }

        //Relationships
        public Guid? MainPicture { get; set; }
        public virtual List<UserPicture> UserPictures { get; set; } = new();
        public virtual List<Order> Orders { get; set; } = new();
        public Guid CurrentOrderId { get; set; }
    }
}
