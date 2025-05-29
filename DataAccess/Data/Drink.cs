namespace WinUiApp.Data.Data
{
    public class Drink
    {
        public int DrinkId { get; set; }
        public string DrinkName { get; set; }
        public string DrinkURL { get; set; }
        public int? BrandId { get; set; }
        public decimal AlcoholContent { get; set; }

        public Brand Brand { get; set; }
        public ICollection<DrinkCategory> DrinkCategories { get; set; }
        public ICollection<Vote> Votes { get; set; }
        public ICollection<UserDrink> UserDrinks { get; set; }
    }
}
