namespace WinUIApp.WebAPI.Requests.Drink
{
    public class AddToUserPersonalDrinkListRequest
    {
        public int userId { get; set; }
        public int drinkId { get; set; }
    }
}
