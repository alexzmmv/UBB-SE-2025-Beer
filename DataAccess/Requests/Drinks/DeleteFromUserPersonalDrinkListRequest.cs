namespace WinUIApp.ProxyServices.Requests.Drinks
{
    public class DeleteFromUserPersonalDrinkListRequest
    {
        public Guid UserId { get; set; }
        public int DrinkId { get; set; }
    }
}
