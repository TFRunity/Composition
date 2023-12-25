namespace Composition.Models
{
    public class SubCategory
    {
        public Guid Id { get; set; }
        public string Case { get; set; } = "First";
        public int Price { get; set; } = 0;
        public string Feature { get; set; } = "None";
        public string Url { get; set; } = "None";

        //Relationship
        public Guid? ProductId { get; set; }
        public virtual Product? Product { get; set; }
    }
}
