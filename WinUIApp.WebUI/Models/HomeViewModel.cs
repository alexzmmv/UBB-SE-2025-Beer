using WinUiApp.Data.Data;
using WinUIApp.WebAPI.Models;

namespace WinUIApp.WebUI.Models;

public class HomeViewModel
{
    public required DrinkDTO Drink { get; set; }
    public List<Category> drinkCategories { get; set; }
    public List<Brand> drinkBrands { get; set; }
    public List<DrinkDTO> drinks { get; set; }
}