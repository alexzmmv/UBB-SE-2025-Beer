namespace WinUIApp.ProxyServices.Requests.Drinks
{
    public class AddToUserPersonalDrinkListRequest
    {
        public Guid UserId { get; set; }
        public int DrinkId { get; set; }
    }
}
