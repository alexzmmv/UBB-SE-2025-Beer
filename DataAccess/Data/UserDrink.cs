namespace WinUiApp.Data.Data
{
    public class UserDrink
    {
        public Guid UserId { get; set; }
        public int DrinkId { get; set; }

        public User User { get; set; }
        public Drink Drink { get; set; }
    }
}
