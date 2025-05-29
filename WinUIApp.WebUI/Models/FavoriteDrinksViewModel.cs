
using WinUIApp.WebAPI.Models;

namespace WinUIApp.WebUI.Models
{
    public class FavoriteDrinksViewModel
    {
        public List<DrinkDTO> FavoriteDrinks { get; set; } = new();
    }
}