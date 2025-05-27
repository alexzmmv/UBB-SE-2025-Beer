namespace WinUIApp.ProxyServices.Requests.Drinks
{
    public class AddToUserPersonalDrinkListRequest
    {
        public Guid userId { get; set; }
        public int drinkId { get; set; }
    }
}
