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
    public class UpdateController(IDrinkService drinkService) : Controller
    {
        [HttpGet]
        public IActionResult Drink(int id)
        {
            try
            {
                var drink = drinkService.GetDrinkById(id);
                var allCategories = drinkService.GetDrinkCategories();
                if (drink == null)
                {
                    throw new Exception("Drink with that id does not exist!");
                }
                var updateDrinkViewModel = new UpdateDrinkViewModel
                {
                    DrinkId = id,
                    //OldDrink = drink,
                    DrinkName = drink.DrinkName ?? string.Empty,
                    DrinkImagePath = drink.DrinkURL,
                    DrinkCategoriesIds = [.. drink.DrinkCategories.Select(category => category.CategoryId)],
                    DrinkBrandName = drink.Brand.BrandName,
                    DrinkAlcoholPercentage = drink.AlcoholContent,
                    AvailableCategories = [.. allCategories.Select(category => new SelectListItem
                    {
                        Value = category.CategoryId.ToString(),
                        Text = category.CategoryName,
                        Selected = drink.DrinkCategories.Any(drinkCategory => drinkCategory.CategoryId == category.CategoryId)
                    })]
                };
                return View(updateDrinkViewModel);
            } 
            catch (Exception apiException)
            {
                throw new Exception("API EXCEPTION: " + apiException.Message);
            }
        }

        [HttpPost]
        public IActionResult Drink(int id, UpdateDrinkViewModel updateDrinkViewModel)
        {
            try
            {
                var categories = drinkService.GetDrinkCategories();
                if (ModelState.IsValid)
                {

                    drinkService.UpdateDrink(new Drink
                    {
                        DrinkId = updateDrinkViewModel.DrinkId,
                        DrinkName = updateDrinkViewModel.DrinkName,
                        DrinkURL = updateDrinkViewModel.DrinkImagePath,
                        DrinkCategories = updateDrinkViewModel.DrinkCategoriesIds
                            .Select(id => categories.FirstOrDefault(c => c.CategoryId == id))
                            .Where(c => c != null),
                        AlcoholContent = updateDrinkViewModel.DrinkAlcoholPercentage,
                        DrinkBrand = new Brand(-1, updateDrinkViewModel.DrinkBrandName)
                    });
                    return RedirectToAction("DrinkDetail", "Drink", new {id = updateDrinkViewModel.DrinkId});
                }
                else
                {
                    var newUpdateDrinkViewModel = new UpdateDrinkViewModel
                    {
                        DrinkId = id,
                        DrinkName = updateDrinkViewModel.DrinkName,
                        DrinkBrandName = updateDrinkViewModel.DrinkBrandName,
                        DrinkAlcoholPercentage = updateDrinkViewModel.DrinkAlcoholPercentage,
                        DrinkImagePath = updateDrinkViewModel.DrinkImagePath,
                        DrinkCategoriesIds = updateDrinkViewModel.DrinkCategoriesIds,
                        AvailableCategories = [.. categories.Select(category => new SelectListItem
                        {
                            Value = category.CategoryId.ToString(),
                            Text = category.CategoryName,
                            Selected = updateDrinkViewModel.DrinkCategoriesIds.Contains(category.CategoryId)
                        })]
                    };
                    return View(newUpdateDrinkViewModel);
                }
            }
            catch (Exception apiException)
            {
                throw new Exception("API EXCEPTION: " + apiException.Message);
            }
        }
    }
}
