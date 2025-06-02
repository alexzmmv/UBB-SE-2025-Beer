using DataAccess.Service;
using DataAccess.Service.Interfaces;
using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WinUIApp.ProxyServices;
using WinUIApp.WebUI.Models;

namespace WinUIApp.WebUI.Controllers
{
    public class FavoriteDrinksController : Controller
    {
        private readonly IDrinkService drinkService;
        private readonly IReviewService reviewService;

        public FavoriteDrinksController(IDrinkService drinkService, IReviewService reviewService)
        {
            this.drinkService = drinkService;
            this.reviewService = reviewService;
        }

        public async Task<IActionResult> FavoriteDrinks()
        {
            Guid CurrentUserId = AuthenticationService.GetCurrentUserId();

            var favoriteDrinks = this.drinkService.GetUserPersonalDrinkList(CurrentUserId);
            var drinkViewModels = new List<DrinkElementViewModel>();

            foreach (var drink in favoriteDrinks)
            {
                var averageRating = await reviewService.GetAverageRating(drink.DrinkId);

                drinkViewModels.Add(new DrinkElementViewModel
                {
                    Drink = drink,
                    AverageRating = averageRating
                });
            }

            var viewModel = new FavoriteDrinksViewModel
            {
                FavoriteDrinks = drinkViewModels
            };

            return View(viewModel);
        }
    }
}
