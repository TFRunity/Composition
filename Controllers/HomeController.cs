using Composition.Interfaces;
using Composition.Models;
using Composition.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Composition.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ICategoryRepository categoryMethods;
        private readonly UserManager<User> userManager;
        public HomeController(ICategoryRepository _categoryMethods, UserManager<User> _userManager)
        {
            categoryMethods = _categoryMethods;
            userManager = _userManager;
        }
        public async Task<IActionResult> Index()
        {
            List<Category>? categories = await categoryMethods.GetAll();
            if (User?.Identity?.IsAuthenticated == true)
            {
                if(User.Identity.Name != null)
                {
                    User? _user = await userManager.FindByNameAsync(User.Identity.Name);
                    if (_user == null)
                        return View(categories);
                    if (_user.CurrentOrderId != Guid.Empty)
                    {
                        ViewBag.Created = true;
                    }
                    else
                    {
                        ViewBag.Created = false;
                    }
                }
            }
            return View(categories);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult ErrorOccupied(string message)
        {
            ErrorMessage model = new ErrorMessage()
            {
                Message = message
            };
            return View(model);
        }

        //Добавить в List возвращение ошибки и её инициализацию пользователю
    }
}
