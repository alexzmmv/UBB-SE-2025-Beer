namespace WinUIApp.WebAPI.Requests.Drink
{
    public class AddToUserPersonalDrinkListRequest
    {
        public Guid userId { get; set; }
        public int drinkId { get; set; }
    }
}
