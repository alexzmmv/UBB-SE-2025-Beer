using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication.Controllers
{
	public class AddDrinkController : Controller
	{
		private IDrinkService drinkService;
		public AddDrinkController(IDrinkService drinkService)
		{
			this.drinkService = drinkService;
		}

		[HttpGet]
		public IActionResult Drink()
		{
			AddDrinkViewModel addViewModel = new AddDrinkViewModel
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
				AddDrinkViewModel newViewModel = new AddDrinkViewModel
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