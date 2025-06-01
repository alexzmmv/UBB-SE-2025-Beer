namespace WinUIApp.WebAPI.Requests.Drink
{
    public class AddToUserPersonalDrinkListRequest
    {
        public Guid UserId { get; set; }
        public int DrinkId { get; set; }
    }
}
