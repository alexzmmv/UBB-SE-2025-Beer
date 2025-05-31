namespace WinUIApp.WebAPI.Requests.Drink
{
    public class GetDrinksRequest
    {
        public string SearchKeyword { get; set; }
        public List<string> DrinkBrandNameFilter { get; set; }
        public List<string> DrinkCategoryFilter { get; set; }
        public float MinimumAlcoholPercentage { get; set; }
        public float MaximumAlcoholPercentage { get; set; }
        public Dictionary<string, bool>? OrderingCriteria { get; set; }
    }
}
