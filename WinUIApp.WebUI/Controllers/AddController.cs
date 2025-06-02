using System.Diagnostics;
using DataAccess.Service;
using DataAccess.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WinUiApp.Data.Data;
using WinUIApp.ProxyServices;
using WinUIApp.ProxyServices.Models;
using WinUIApp.WebUI.Models;
using DataAccess.Service;

namespace WinUIApp.WebUI.Controllers
{
    public class AddController : Controller
    {
        private IDrinkService drinkService;
        private IUserService userService;
        public AddController(IDrinkService drinkService, IUserService userService)
        {
            this.drinkService = drinkService;
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Drink()
        {
            Guid currentUserId = Guid.Parse(HttpContext.Session.GetString("UserId") ?? Guid.Empty.ToString());
            if (currentUserId == Guid.Empty)
                return RedirectToAction("AuthenticationPage", "Auth");

            var addViewModel = new AddDrinkViewModel
            {
                UserRole = userService.GetUserById(currentUserId).Result.AssignedRole,
                AvailableCategories = [.. drinkService.GetDrinkCategories()
                    .Select(c => new SelectListItem
                    {
                        Value = c.CategoryId.ToString(),
                        Text = c.CategoryName,
                    })],
            };

            return View(addViewModel);
        }

        [HttpPost]
        public IActionResult Drink(AddDrinkViewModel addViewModel)
        {
            Guid currentUserId = Guid.Parse(HttpContext.Session.GetString("UserId")??Guid.Empty.ToString());
                if(currentUserId==Guid.Empty)
                    return RedirectToAction("AuthenticationPage", "Auth");

            if (ModelState.IsValid)
            {
                var categories = drinkService.GetDrinkCategories();
           
                drinkService.AddDrink(
                    addViewModel.DrinkName,
                    addViewModel.DrinkImagePath,
                    [.. addViewModel.DrinkCategories.Select((index, categoryId) =>
                    {
                        var category = categories.Find((category) =>
                        {
                            return category.CategoryId == categoryId;
                        });
                        return category;
                    }).OfType<Category>()],
                    addViewModel.DrinkBrandName,
                    addViewModel.DrinkAlcoholPercentage,
                    currentUserId,
                    false
                    );
                return RedirectToAction("Index", "HomePage");
            }
            else
            {
                var newViewModel = new AddDrinkViewModel
                {
                    UserRole = userService.GetUserById(currentUserId).Result.AssignedRole,
                    AvailableCategories = [.. drinkService.GetDrinkCategories()
                        .Select(c => new SelectListItem
                        {
                            Value = c.CategoryId.ToString(),
                            Text = c.CategoryName,
                        })],
                };
                return View(newViewModel);
            }
        }
    }
}
