﻿using DataAccess.Service;
using DataAccess.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WinUiApp.Data.Data;
using WinUIApp.WebAPI.Models;
using WinUIApp.WebUI.Models;
using DataAccess.Service;

namespace WinUIApp.WebUI.Controllers
{
    public class UpdateController(IDrinkService drinkService, IUserService userService) : Controller
    {
        [HttpGet]
        public IActionResult Drink(int id)
        {
            Guid CurrentUserId = Guid.Parse(HttpContext.Session.GetString("UserId") ?? Guid.Empty.ToString());
            if (CurrentUserId == Guid.Empty)
                return RedirectToAction("AuthenticationPage", "Auth");
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
                    UserRole = userService.GetUserById(CurrentUserId).Result.AssignedRole,

                    DrinkId = id,
                    //OldDrink = drink,
                    DrinkName = drink.DrinkName ?? string.Empty,
                    DrinkImagePath = drink.DrinkImageUrl,
                    DrinkCategoriesIds = [.. drink.CategoryList.Select(category => category.CategoryId)],
                    DrinkBrandName = drink.DrinkBrand.BrandName,
                    DrinkAlcoholPercentage = drink.AlcoholContent,
                    AvailableCategories = [.. allCategories.Select(category => new SelectListItem
                    {
                        Value = category.CategoryId.ToString(),
                        Text = category.CategoryName,
                        Selected = drink.CategoryList.Any(drinkCategory => drinkCategory.CategoryId == category.CategoryId)
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

            Guid currentUserId = Guid.Parse(HttpContext.Session.GetString("UserId") ?? Guid.Empty.ToString());
            if (currentUserId == Guid.Empty)
                return RedirectToAction("AuthenticationPage", "Auth");
            try
            {
                var categories = drinkService.GetDrinkCategories();
                if (ModelState.IsValid)
                {
                    drinkService.UpdateDrink(new DrinkDTO
                    {
                        DrinkId = updateDrinkViewModel.DrinkId,
                        DrinkName = updateDrinkViewModel.DrinkName,
                        DrinkImageUrl = updateDrinkViewModel.DrinkImagePath,
                        CategoryList = [.. updateDrinkViewModel.DrinkCategoriesIds.Select((categoryId) =>
                            {
                                return categories.FirstOrDefault(category => category.CategoryId == categoryId);
                            }).OfType<Category>()],
                        AlcoholContent = updateDrinkViewModel.DrinkAlcoholPercentage,
                        DrinkBrand = drinkService.GetDrinkBrandNames().FirstOrDefault(brand => brand.BrandName == updateDrinkViewModel.DrinkBrandName)
                    }, currentUserId);
                    return RedirectToAction("DrinkDetail", "Drink", new { id = updateDrinkViewModel.DrinkId });
                }
                else
                {
                    var newUpdateDrinkViewModel = new UpdateDrinkViewModel
                    {
                        UserRole = userService.GetUserById(currentUserId).Result.AssignedRole,

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
                // Preserve the inner exception details for better diagnostics
                throw new Exception("API EXCEPTION: Error processing update drink request.", apiException);
            }
        }
    }
}