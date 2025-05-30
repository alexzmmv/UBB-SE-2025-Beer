namespace WinUIApp.WebAPI.Requests.Drink
{
    public class DeleteDrinkRequest
    {
        public Guid RequestingUserId { get; set; }
        public int drinkId { get; set; }
    }
}
