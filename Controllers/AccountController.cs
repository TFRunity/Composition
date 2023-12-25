using Composition.Interfaces;
using Composition.Models;
using Composition.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace Composition.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IOrderRepository orderMethods;
        private readonly IUserRepository userMethods;
        private readonly IUserPicturesRepository userPictureMethods;
        public AccountController(UserManager<User> _userManager, SignInManager<User> _signInManager, IOrderRepository _orderMethods, IUserRepository _userMethods, IUserPicturesRepository userPicturesRepository)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            orderMethods = _orderMethods;
            userMethods = _userMethods;
            userPictureMethods = userPicturesRepository;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Name, Year = model.Year};
                // добавляем пользователя
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // установка куки
                    await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Anon"));
                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        public async Task<IActionResult> PersonalArea(string email)
        {
            User? user = await userMethods.GetByEmail(email);
            if(user == null)
                return NotFound();
            WebUser webUser = new WebUser(user);
            return View(webUser);
        }

        [Authorize(Policy = "Manager")]
        public IActionResult AdminArea()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login(string? returnurl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnurl }) ;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await signInManager.PasswordSignInAsync(model.Name, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                // проверяем, принадлежит ли URL приложению
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "Неправильный логин и (или) пароль");
            }
            return View(model);
        }

        [Authorize(Policy = "Member")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Policy = "Member")]
        [HttpGet]
        public async Task<IActionResult> Edit(string email)
        {
            User? user = await userMethods.GetByEmail(email);
            if (user == null)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Такого пользователя не существует." });
            return View(user);
        }

        [Authorize(Policy = "Member")]
        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            bool result = await userMethods.Update(user);
            if (result == false)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Не получилось обновить информацию о пользователе." });
            return RedirectToAction(nameof(PersonalArea), new { email = user.Email });
        }

        [Authorize(Policy = "Member")]
        [HttpGet]
        public async Task<IActionResult> EditPassword(string email)
        {
            User? user = await userMethods.GetByEmail(email);
            if (user == null)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Такого пользователя не существует." });
            if (user.Email != null)
            {
                ChangePasswordViewModel viewmodel = new ChangePasswordViewModel() { Email = user.Email };
                return View(viewmodel);
            }
            return RedirectToAction("ErrorOccupied", "Home", new { message = "Такого пользователя не существует." });
        }

        [Authorize(Policy = "Member")]
        [HttpPost]
        public async Task<IActionResult> EditPassword(ChangePasswordViewModel viewmodel)
        {
            bool result = await userMethods.UpdatePassword(viewmodel.Email, viewmodel.Password);
            if (result == true)
                return RedirectToAction(nameof(PersonalArea), new { email = viewmodel.Email });
            return RedirectToAction("ErrorOccupied", "Home", new { message = "Не получилось обновить пароль, попробуйте заново. Возможно в нём присутствуют запрещённые знаки." });
        }

        [Authorize(Policy = "Member")]
        public async Task<IActionResult> Pictures(string email)
        {
            User? user = await userMethods.GetByEmail(email);
            if (user != null)
            {
                WebUser webUser = new WebUser(user);
                return View(webUser);
            }
            return RedirectToAction("ErrorOccupied", "Home", new { message = "Такого пользователя не существует." });
        }

        [HttpGet]
        public async Task<IActionResult> AddPicture(string? email)
        {
            if (email == null)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Передача пустой почты." });
            User? user = await userMethods.GetByEmail(email);
            if (user == null)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Такого пользователя не существует." });
            AddPictureViewModel model = new AddPictureViewModel { Email = email };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddPicture(AddPictureViewModel pictureViewModel)
        {
            if (pictureViewModel.Email == null)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Пользователя с такой почтой не существует." });
            User? user = await userMethods.GetByEmail(pictureViewModel.Email);
            if (user == null)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Такого пользователя не существует." });
            UserPicture picture = new UserPicture
            {
                URL = pictureViewModel.Url,
                User = user
            };
            bool result = await userPictureMethods.Create(picture);
            if (result == false)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Не получилось создать картинку для этого пользователя." });
            return RedirectToAction(nameof(Pictures), new { email = pictureViewModel.Email });
        }

        [Authorize(Policy = "Member")]
        public async Task<IActionResult> SetMainPicture(Guid pictureId)
        {
            UserPicture? picture = await userPictureMethods.GetById(pictureId);
            if(picture == null)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Такой картинки не существует." });
            bool result = await userPictureMethods.SetMainPicture(picture);
            if(result == false)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Не получилось установить аватарку." });
            return RedirectToAction("Index", "Home");
        }
        
        public async Task<IActionResult> Cart(CreateOrderItemViewModel viewModel)
        {
            bool result = await orderMethods.Create(viewModel);
            if (result == true)
                return RedirectToAction("Index", "Home");
            return RedirectToAction("ErrorOccupied", "Home", new { message = "Не получилось создать заказ." });
        }

        [Authorize(Policy = "Member")]
        [HttpGet]
        public async Task<IActionResult> Order(Guid orderId)
        {
            return View(await orderMethods.GetById(orderId));
        }

        [Authorize(Policy = "Member")]
        [HttpPost]
        public async Task<IActionResult> Order(Order order)
        {
            bool result = await orderMethods.Update(order);
            if(result == true)
                return RedirectToAction("Index", "Home");
            return RedirectToAction("ErrorOccupied", "Home", new { message = "Не получилось обновить заказ." });
        }

        [Authorize(Policy = "Member")]
        public async Task<IActionResult> DeleteItem(Guid OrderItemId, Guid orderId)
        {
            Order? order = await orderMethods.GetById(orderId);
            if (order == null)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Такого заказа не существует." });
            if (order.Paid == true)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Заказ уже оплачен, его нельзя изменить." });
            bool result = await orderMethods.DeleteObjFromOrder(OrderItemId);
            if(result == true)
                return RedirectToAction(nameof(Order), new { orderId = orderId });
            return RedirectToAction("ErrorOccupied", "Home", new { message = "Не получилось удалить товар." });
        }

        [Authorize(Policy = "Member")]
        public async Task<IActionResult> ConfirmOrder(Guid orderId, string email)
        {
            Order? order = await orderMethods.GetById(orderId);
            if (order == null)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Такого заказа не существует." });
            if (order.Paid == true)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Заказ уже оплачен, его не подтвердить снова." });
            bool result = await orderMethods.ConfirmOrderByIdAndEmail(orderId, email);
            if(result == true)
                return RedirectToAction(nameof(PersonalArea), new { email = email });
            return RedirectToAction("ErrorOccupied", "Home", new { message = "Не получилось подтвердить заказ." });
        }

        [Authorize(Policy = "Member")]
        [HttpPost]
        public async Task<IActionResult> ChangeCount(CounterViewModel viewModel)
        {
            bool result = await orderMethods.ChangeCount(viewModel);
            if(result == true)
                return RedirectToAction(nameof(Order), new { orderId = viewModel.OrderId });
            return RedirectToAction("ErrorOccupied", "Home", new { message = "Не получилось изменить кол-во товаров."});
        }

        [Authorize(Policy = "Member")]
        public async Task<IActionResult> ClearOrder(Guid orderId)
        {
            bool result = await orderMethods.Clear(orderId);
            if(result == true)
                return RedirectToAction(nameof(Order), new { orderId = orderId });
            return RedirectToAction("ErrorOccupied", "Home", new { message = "Не получилось очистить данный заказ."});
        }

        [Authorize(Policy = "Member")]
        public async Task<IActionResult> OrderHistory(string email)
        {
            User? user = await userMethods.GetByEmail(email);
            if (user == null)
                return RedirectToAction("ErrorOccupied","Home", new { message = "Данного пользователя не существует."});
            WebUser webUser = new WebUser(user);
            return View(webUser);
        }
    }
}
