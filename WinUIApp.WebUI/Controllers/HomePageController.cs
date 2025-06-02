using System.Diagnostics;
using DataAccess.Service.Interfaces;
using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WinUiApp.Data.Data;
using WinUIApp.ProxyServices;
using WinUIApp.ProxyServices.Models;
using WinUIApp.WebAPI.Models;
using WinUIApp.WebUI.Models;


namespace WinUIApp.WebUI.Controllers;

public class HomePageController : Controller
{
    private readonly ILogger<HomePageController> _logger;

    private readonly IDrinkService drinkService;
    private readonly IReviewService drinkReviewService;

    public HomePageController(ILogger<HomePageController> logger, IDrinkService drinkService, IReviewService reviewService)
    {
        _logger = logger;
        this.drinkService = drinkService;
        this.drinkReviewService = reviewService;
    }

    public IActionResult Index(string? searchKeyword, float? minValue, float? maxValue, int? minStars, string[] SelectedCategories, string[] SelectedBrandNames)
    {
        Guid currentUserId = Guid.Parse(HttpContext.Session.GetString("UserId") ?? Guid.Empty.ToString());
        if (currentUserId == Guid.Empty)
            return RedirectToAction("AuthenticationPage", "Auth");

        var drinkOfTheDay = drinkService.GetDrinkOfTheDay();
        var drinkCategories = drinkService.GetDrinkCategories();
        var drinkBrands = drinkService.GetDrinkBrandNames();

        List<string> drinkCategoriesList = SelectedCategories?.Select(s => (string?)s ?? "").ToList() ?? new List<string>();
        List<string> drinkBrandsList = SelectedBrandNames?.Select(s => (string?)s ?? "").ToList() ?? new List<string>();

        // Ensure that an empty searchKeyword is treated as null or empty for the service call
        if (string.IsNullOrWhiteSpace(searchKeyword))
        {
            searchKeyword = null;
        }
        if (minStars == null) minStars = 0;

        Dictionary<string, bool> drinkOrderingCriteria = new Dictionary<string, bool>();
        
        List<DrinkElementViewModel> drinksViewModels = new List<DrinkElementViewModel>();

        var drinks = drinkService.GetDrinks(searchKeyword, drinkBrandsList, drinkCategoriesList, minValue, maxValue, drinkOrderingCriteria).Where(drink => !drink.IsRequestingApproval).ToList();
        foreach (var drink in drinks)
        {
            if (drinkReviewService.GetAverageRating(drink.DrinkId).Result >= minStars) {
                drinksViewModels.Add(new DrinkElementViewModel { Drink = drink, AverageRating = drinkReviewService.GetAverageRating(drink.DrinkId).Result });
            }
        }

        var homeViewModel = new HomeViewModel
        {
            Drink = new DrinkElementViewModel { Drink = drinkOfTheDay, AverageRating = drinkReviewService.GetAverageRating(drinkOfTheDay.DrinkId).Result },
            drinkCategories = drinkCategories,
            drinkBrands = drinkBrands,
            drinks = drinksViewModels ?? new List<DrinkElementViewModel>(), // Ensure drinks is not null
             
            SearchKeyword = searchKeyword,
            MinValue = minValue,
            MaxValue = maxValue,
            MinStars = minStars,
            SelectedCategories = SelectedCategories ?? new string[0],
            SelectedBrandNames = SelectedBrandNames ?? new string[0],
        };

        return View(homeViewModel);
    }

    // Filtering products - This method is now combined with the one above.
    // [HttpPost] // Removed HttpPost attribute
    // public IActionResult Index(string? searchKeyword, float? minValue, float? maxValue, int? minStars,string[] SelectedCategories,string[] SelectedBrandNames)
    // {
    //     var drinkOfTheDay = drinkService.GetDrinkOfTheDay();
    //     var drinkCategories = drinkService.GetDrinkCategories();
    //     var drinkBrands = drinkService.GetDrinkBrandNames();
    //     
    //     List<string> drinkCategoriesList = SelectedCategories.Select(s => (string?)s ?? "").ToList();
    //     List<string> drinkBrandsList = SelectedBrandNames.Select(s => (string?)s ?? "").ToList();
    //     
    //     drinkCategoriesList.ForEach(drink=>_logger.LogInformation(drink.ToString()));
    //     Dictionary<string,bool> drinkOrderingCriteria = new Dictionary<string,bool>();
    // 
    //     var drinks = drinkService.GetDrinks(searchKeyword,drinkBrandsList ,drinkCategoriesList,minValue, maxValue,drinkOrderingCriteria);
    //     
    //     var homeViewModel = new HomeViewModel
    //     {
    //         DrinkOfTheDay = drinkOfTheDay,
    //         drinkCategories = drinkCategories,
    //         drinkBrands = drinkBrands,
    //         drinks = drinks
    //     };
    //     return View(homeViewModel);
    // }

    public IActionResult ClearFilters() {
        return RedirectToAction("Index");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}