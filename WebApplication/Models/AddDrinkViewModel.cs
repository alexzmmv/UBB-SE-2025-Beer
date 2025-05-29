using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
	public class AddDrinkViewModel
	{
		[Required]
		public string DrinkName { get; set; } = string.Empty;

		[Display(Name = "Image URL")]
		public string DrinkImagePath { get; set; } = string.Empty;

		[Display(Name = "Categories")]
		public List<int> DrinkCategories { get; set; } = [];

		[Display(Name = "Brand Name")]
		public string DrinkBrandName { get; set; } = string.Empty;

		[Display(Name = "Alcohol Content (%)")]
		[Range(0, 100)]
		public float DrinkAlcoholPercentage { get; set; } = 0.0f;

		public List<SelectListItem> AvailableCategories { get; set; } = new();
	}
}