namespace Composition.ViewModels
{
    public class OrderNavbarViewModel
    {
        public Guid OrderId { get; set; }
        public bool Authentificated { get; set; }
        public bool Created { get; set; }
        public string? Email { get; set; }
    }
}
