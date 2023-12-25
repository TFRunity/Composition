using Composition.Interfaces;
using Composition.Models;
using Composition.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Composition.Controllers
{
    [Authorize(Policy = "Manager")]
    public class ModeratorController : Controller
    {
        private readonly IUserPicturesRepository picturesMethods;
        private readonly IUserRepository userMethods;
        public ModeratorController(IUserPicturesRepository _picturesMethods, IUserRepository _userMethods)
        {
            picturesMethods = _picturesMethods;
            userMethods = _userMethods;
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            List<User> users = await userMethods.GetAll();
            if (users == null)
                return View();
            List<WebUser> webusers = new List<WebUser>();
            foreach(User user in users)
            {
                WebUser web = new WebUser(user);
                webusers.Add(web);
            }
            return View(webusers);
        }
        [HttpGet]
        public async Task<IActionResult> EditUser(string? email)
        {
            User? user = await userMethods.GetByEmail(email);
            if (user == null)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Такого пользователя не существует." });
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(User? editedUser)
        {
            bool result = await userMethods.Update(editedUser);
            if (result == false)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Не получилось обновить информацию о пользователе." });
            return RedirectToAction(nameof(List));
        }
        public async Task<IActionResult> Delete(string? email)
        {
            bool result = await userMethods.DeleteByEmail(email);
            if(result == false) 
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Не получилось удалить пользователя." });
            return RedirectToAction(nameof(List));
        }
        public async Task<IActionResult> UsersPictures(string? email)
        {
            User? user = await userMethods.GetByEmail(email);
            if (user == null)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Такого пользователя не существует." });
            if (user.UserPictures == null)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "У пользователя нет картинок." });
            return View(user.UserPictures);
        }

        public async Task<IActionResult> DeletePicture(Guid? id)
        {
            bool result = await picturesMethods.DeleteById(id);
            if(result == false)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Не получилось удалить фотографию пользователя." });
            return RedirectToAction(nameof(List));
        }
        
        [HttpGet]
        public async Task<IActionResult> AddPicture(string? email)
        {
            if (email == null)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Передача пустой почты." });
            User? user = await userMethods.GetByEmail(email);
            if(user == null)
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
            bool result = await picturesMethods.Create(picture);
            if (result == false)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Не получилось создать картинку для этого пользователя." });
            return RedirectToAction(nameof(List));
        }
    }
}
