namespace WinUiApp.Data.Data
{
    public class DrinkOfTheDay
    {
        public int DrinkId { get; set; }
        public DateTime DrinkTime { get; set; }

        public Drink Drink { get; set; }
    }
}
