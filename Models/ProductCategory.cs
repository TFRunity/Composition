using System.ComponentModel.DataAnnotations;

namespace Composition.Models
{
    public class ProductCategory
    {
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
        public virtual Product? Product { get; set; }
        public virtual Category? Category { get; set; }
    }
}
