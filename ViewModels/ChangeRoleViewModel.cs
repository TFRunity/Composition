using Composition.Models;

namespace Composition.ViewModels
{
    public class ChangeRoleViewModel
    {
        public Guid Id { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public List<UserRole> AllRoles { get; set; } = new();
        public List<string> UserRoles { get; set; } = new();
        public ChangeRoleViewModel()
        {
            AllRoles = new List<UserRole>();
            UserRoles = new List<string>();
        }
    }
}
