using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class FavoriteDrinksController : Controller
    {
        private readonly IDrinkService drinkService;

        public FavoriteDrinksController(IDrinkService drinkService)
        {
            this.drinkService = drinkService;
        }

        public IActionResult FavoriteDrinks()
        {
            const int CurrentUserId = 1;

            var favoriteDrinks = this.drinkService.GetUserPersonalDrinkList(CurrentUserId);

            var viewModel = new FavoriteDrinksViewModel
            {
                FavoriteDrinks = favoriteDrinks.ToList()
            };

            return View(viewModel);
        }
    }
}