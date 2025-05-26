using WinUIApp.ProxyServices.Models;
using Drink = WinUIApp.ProxyServices.Models.Drink;

namespace WinUIApp.WebUI.Models;

public class HomeViewModel
{
    public Drink DrinkOfTheDay { get; set; }
    public List<Category> drinkCategories { get; set; }
    public List<Brand> drinkBrands { get; set; }
    public List<Drink> drinks { get; set; }
}