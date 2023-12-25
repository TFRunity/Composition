namespace Composition.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int Price { get; set; }
        public string URL { get; set; } = "";

        //Relationship
        // n : n
        public virtual List<ProductCategory>? ProductCategories { get; set; }
        public virtual List<SubCategory>? SubCategories { get; set; } 

    }
}
