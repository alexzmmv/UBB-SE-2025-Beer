using Drink = WinUIApp.ProxyServices.Models.Drink;

namespace WinUIApp.WebUI.Models
{
    public class FavoriteDrinksViewModel
    {
        public List<Drink> FavoriteDrinks { get; set; } = new();
    }
}