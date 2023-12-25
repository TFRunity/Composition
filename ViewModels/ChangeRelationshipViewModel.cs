using Composition.Models;

namespace Composition.ViewModels
{
    public class ChangeRelationshipViewModel
    {
        public Guid ProductId { get; set; }
        public List<Guid>? CategoryIds { get; set; }
        public List<Guid> AllCategories { get; set; } = new List<Guid>();
    }
}
