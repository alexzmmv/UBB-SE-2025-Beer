using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WinUIApp.ProxyServices.Models;
using DataAccess.Constants;

namespace WinUIApp.WebUI.Models
{
    public class UpdateDrinkViewModel
    {
        //[BindNever]
        //public Drink OldDrink { get; set; } = new();

        [Required]
        public int DrinkId { get; set; }

        [Required]
        public string DrinkName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Image URL")]
        public string DrinkImagePath { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Categories")]
        public List<int> DrinkCategoriesIds { get; set; } = [];

        [Required]
        [Display(Name = "Brand Name")]
        public string DrinkBrandName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Alcohol Content (%)")]
        [Range(0, 100)]
        public float DrinkAlcoholPercentage { get; set; } = 0.0f;

        [BindNever]
        public List<SelectListItem> AvailableCategories { get; set; } = new();

        public RoleType UserRole { get; set; }
    }
}
