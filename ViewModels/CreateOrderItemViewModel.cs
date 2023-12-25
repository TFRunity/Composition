namespace Composition.ViewModels
{
    public class CreateOrderItemViewModel
    {
        public Guid ProductId { get; set; }
        public string Email { get; set; } = string.Empty;
        public Guid SubCategoryId { get; set; }
        public string _Case { get; set; } = "First";
    }
}
