namespace WinUIApp.WebAPI.Requests.Drink
{
    public class DeleteFromUserPersonalDrinkListRequest
    {
        public Guid UserId { get; set; }
        public int DrinkId { get; set; }
    }
}
