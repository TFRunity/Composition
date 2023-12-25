namespace Composition.ViewModels
{
    public class CounterViewModel
    {
        public Guid OrderId { get; set; }
        public Guid OrderItemId { get; set; }
        public int PreviousCount { get; set; }
        public int CurrentCount { get; set; }
    }
}
