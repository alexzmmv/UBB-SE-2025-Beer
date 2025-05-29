namespace WinUiApp.Data.Data
{
    public class DrinkCategory
    {
        public int DrinkId { get; set; }
        public int CategoryId { get; set; }

        public Drink Drink { get; set; }
        public Category Category { get; set; }
    }
}
