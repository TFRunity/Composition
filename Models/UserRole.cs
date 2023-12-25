using Microsoft.AspNetCore.Identity;

namespace Composition.Models
{
    public class UserRole : IdentityRole<Guid>
    {
        public UserRole(string name) { base.Name = name; }   
    }
}
