namespace WinUIApp.WebAPI.Requests.Drink
{
    public class VoteDrinkOfTheDayRequest
    {
        public Guid userId { get; set; }
        public int drinkId { get; set; }
    }
}
