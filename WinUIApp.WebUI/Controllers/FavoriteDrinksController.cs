using DataAccess.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WinUIApp.ProxyServices;
using WinUIApp.WebUI.Models;

namespace WinUIApp.WebUI.Controllers
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
            Guid CurrentUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));

            var favoriteDrinks = this.drinkService.GetUserPersonalDrinkList(CurrentUserId);

            var viewModel = new FavoriteDrinksViewModel
            {
                FavoriteDrinks = favoriteDrinks.ToList()
            };

            return View(viewModel);
        }
    }
}
