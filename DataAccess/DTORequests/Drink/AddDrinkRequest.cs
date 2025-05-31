using WinUiApp.Data.Data;
using WinUIApp.WebAPI.Models;

namespace DataAccess.DTORequests.Drink
{
    public class AddDrinkRequest
    {
        public Guid RequestingUserId { get; set; }
        public string InputtedDrinkName { get; set; }
        public string InputtedDrinkPath { get; set; }
        public List<Category> InputtedDrinkCategories { get; set; }
        public string InputtedDrinkBrandName { get; set; }
        public float InputtedAlcoholPercentage { get; set; }
    }
}
