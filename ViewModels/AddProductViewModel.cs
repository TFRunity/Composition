using System.ComponentModel.DataAnnotations;

namespace Composition.ViewModels
{
    public class AddProductViewModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Range(0, int.MaxValue)]
        public int Price { get; set; }
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Url { get; set; } = string.Empty;
    }
}
