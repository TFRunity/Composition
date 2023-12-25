namespace Composition.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string RussianName { get; set; } = "";

        //Relationship
        public virtual List<ProductCategory>? ProductCategories { get; set; }

    }
}
