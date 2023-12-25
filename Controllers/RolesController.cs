using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Composition.Models;
using Composition.ViewModels;

namespace Composition.Controllers
{
    [Authorize(Policy = "Admin")]
    public class RolesController : Controller
    {
        RoleManager<UserRole> roleManager;
        UserManager<User> userManager;
        public RolesController(RoleManager<UserRole> _roleManager, UserManager<User> _userManager)
        {
            roleManager = _roleManager;
            userManager = _userManager;
        }
        public IActionResult Index() => View(roleManager.Roles.ToList());

        public IActionResult Create() => View();
        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await roleManager.CreateAsync(new UserRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                }
            }
            return View(name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            UserRole? role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                await roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }

        public IActionResult UserList() => View(userManager.Users.ToList());

        public async Task<IActionResult> Edit(string name)
        {
            // получаем пользователя
            User? user = await userManager.FindByNameAsync(name);
            if (user != null)
            {
                if(user.Email ==null)
                    return RedirectToAction("ErrorOccupied", "Home", new { message = "Такого пользователя не существует." });
                // получем список ролей пользователя
                List<string> userRoles = (List<string>)await userManager.GetRolesAsync(user);
                var allRoles = roleManager.Roles.ToList();
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    Id = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }

            return RedirectToAction("ErrorOccupied", "Home", new { message = "Такого пользователя не существует." });
        }
        [HttpPost]
        //roles = allroles
        public async Task<IActionResult> Edit(string name, List<string> roles)
        {
            // получаем пользователя
            User? user = await userManager.FindByEmailAsync(name);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = await userManager.GetRolesAsync(user);
                // получаем список ролей, которые были добавлены
                var addedRoles = roles.Except(userRoles);
                // получаем роли, которые были удалены
                var removedRoles = userRoles.Except(roles);
                if (addedRoles != null)
                {
                    await userManager.AddToRolesAsync(user, addedRoles);
                }
                else
                {
                    await userManager.AddToRoleAsync(user, "Anon");
                }
                await userManager.RemoveFromRolesAsync(user, removedRoles);

                return RedirectToAction("UserList");
            }

            return RedirectToAction("ErrorOccupied", "Home", new { message = "Такого пользователя не существует." });
        }
    }
}
