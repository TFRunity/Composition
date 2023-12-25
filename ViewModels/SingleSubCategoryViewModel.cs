namespace Composition.ViewModels
{
    public class SingleSubCategoryViewModel
    {
        public Guid SubCategoryId { get; set; }
        public string Feature { get; set; } = string.Empty;
        public int Price { get; set; }
        public string Url { get; set; } = string.Empty;
    }
}
