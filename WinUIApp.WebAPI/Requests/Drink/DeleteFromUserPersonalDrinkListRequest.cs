namespace WinUIApp.WebAPI.Requests.Drink
{
    public class DeleteFromUserPersonalDrinkListRequest
    {
        public int userId { get; set; }
        public int drinkId { get; set; }
    }
}
