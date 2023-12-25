using System.ComponentModel.DataAnnotations;

namespace Composition.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string Email { get; set; } = string.Empty;
        [DataType(DataType.Password), Required(ErrorMessage = "Пароль должен содержать не менее 7 символов")]
        public string Password { get; set; } = string.Empty;
    }
}
