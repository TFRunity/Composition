namespace Composition.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public int TotalPrice { get; set; } = 0;
        public DateTime Done { get; set; } = DateTime.Now;
        public bool Paid { get; set; } = false;
        public Guid? UserId { get; set; }
        public virtual User? User { get; set; }
        public virtual List<OrderItem> OrderItems { get; set; } = new();
    }
}
