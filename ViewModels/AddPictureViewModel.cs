using System.ComponentModel.DataAnnotations;

namespace Composition.ViewModels
{
    public class AddPictureViewModel
    {
        [Required]
        public string Url { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
    }
}
