using System.ComponentModel.DataAnnotations;

namespace Composition.ViewModels
{
    public class SubCategoryViewModel
    {
        public Guid? ProductId { get; set; }
        public Guid? SubCategoryId { get; set; }
        [Required]
        public string Feature { get; set; } = string.Empty;
        public int Price { get; set; }
        public string Url { get; set; } = string.Empty;
        public string _Case { get; set; } = "First";
    }
}
