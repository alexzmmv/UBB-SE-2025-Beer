namespace WinUIApp.ProxyServices.Requests.Drinks
{
    public class DeleteFromUserPersonalDrinkListRequest
    {
        public int userId { get; set; }
        public int drinkId { get; set; }
    }
}
