namespace Composition.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid IdFromProduct { get; set; }
        public Guid IdFromSubCategory { get; set; }
        public string _Case { get; set; } = "First";
        public int PriceFromProduct { get; set; }
        //Count of Products
        public int CurrentCount { get; set; } = 1;
        public int PreviousCount { get; set; } = 1;
        public Guid? OrderId { get; set; }
        public virtual Order? Order { get; set; }
    }
}
