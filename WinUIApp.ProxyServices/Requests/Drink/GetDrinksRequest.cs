namespace WinUIApp.ProxyServices.Requests.Drink
{
    public class GetDrinksRequest
    {
        public string searchKeyword { get; set; }
        public List<string> drinkBrandNameFilter { get; set; }
        public List<string> drinkCategoryFilter { get; set; }
        public float minimumAlcoholPercentage { get; set; }
        public float maximumAlcoholPercentage { get; set; }
        public Dictionary<string, bool>? orderingCriteria { get; set; }
    }
}
