
using WinUIApp.WebAPI.Models;

namespace WinUIApp.WebUI.Models
{
    public class FavoriteDrinksViewModel
    {
        public List<DrinkElementViewModel> FavoriteDrinks { get; set; } = new();
    }
}