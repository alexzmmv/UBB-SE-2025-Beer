namespace WinUIApp.ProxyServices.Requests.Drinks
{
    public class DeleteFromUserPersonalDrinkListRequest
    {
        public Guid userId { get; set; }
        public int drinkId { get; set; }
    }
}
