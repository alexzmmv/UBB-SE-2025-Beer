namespace WinUIApp.WebAPI.Requests.Drink
{
    public class VoteDrinkOfTheDayRequest
    {
        public Guid UserId { get; set; }
        public int DrinkId { get; set; }
    }
}
