using System.Diagnostics;
using DataAccess.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WinUiApp.Data.Data;
using WinUIApp.ProxyServices;
using WinUIApp.ProxyServices.Models;
using WinUIApp.WebUI.Models;

namespace WinUIApp.WebUI.Controllers
{
    public class AddController : Controller
    {
        private IDrinkService drinkService;
        public AddController(IDrinkService drinkService)
        {
            this.drinkService = drinkService;
        }

        [HttpGet]
        public IActionResult Drink()
        {
            var addViewModel = new AddDrinkViewModel
            {
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
                    addViewModel.DrinkAlcoholPercentage
                    );
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var newViewModel = new AddDrinkViewModel
                {
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
