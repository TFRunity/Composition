using System.ComponentModel.DataAnnotations;

namespace Composition.ViewModels
{
    public class RegisterViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        [Range(1, 2023)]
        public int Year { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty.ToString();
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; } = string.Empty.ToString() ;
    }
}
