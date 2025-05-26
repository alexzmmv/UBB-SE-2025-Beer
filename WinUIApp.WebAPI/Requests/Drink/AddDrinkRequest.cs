using WinUIApp.WebAPI.Models;

namespace WinUIApp.WebAPI.Requests.Drink
{
    public class AddDrinkRequest
    {
        public string inputtedDrinkName { get; set; }
        public string inputtedDrinkPath { get; set; }
        public List<CategoryDTO> inputtedDrinkCategories { get; set; }
        public string inputtedDrinkBrandName { get; set; }
        public float inputtedAlcoholPercentage { get; set; }
    }
}
