using WinUiApp.Data.Data;
using WinUIApp.WebAPI.Models;

namespace WinUIApp.WebUI.Models;

public class HomeViewModel
{
    public required DrinkDTO Drink { get; set; }
    public List<Category> drinkCategories { get; set; }
    public List<Brand> drinkBrands { get; set; }
    public List<DrinkDTO> drinks { get; set; }
    public string SearchKeyword { get; set; }
    public float? MinValue { get; set; }
    public float? MaxValue { get; set; }
    public int? MinStars { get; set; }
    public string[] SelectedCategories { get; set; }
    public string[] SelectedBrandNames { get; set; }
}
