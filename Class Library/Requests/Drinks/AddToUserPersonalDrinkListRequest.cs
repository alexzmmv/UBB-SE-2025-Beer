namespace WinUIApp.ProxyServices.Requests.Drinks
{
    public class AddToUserPersonalDrinkListRequest
    {
        public int userId { get; set; }
        public int drinkId { get; set; }
    }
}
