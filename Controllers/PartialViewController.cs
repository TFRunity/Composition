using Composition.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Composition.Controllers
{
    public class PartialViewController : Controller
    {
        public IActionResult Navbar(OrderNavbarViewModel model)
        {
            return PartialView("_Navbar", model);
        }
        public IActionResult Carousel()
        {
            return PartialView("_Carousel");
        }
        public IActionResult Cart(OrderNavbarViewModel model)
        {
            return PartialView("_Cart", model);
        }
        public IActionResult Counter(CounterViewModel model)
        {
            return PartialView("_Counter", model);
        }
    }
}
