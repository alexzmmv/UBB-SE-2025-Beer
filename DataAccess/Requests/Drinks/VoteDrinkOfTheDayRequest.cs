namespace WinUIApp.ProxyServices.Requests.Drinks
{
    public class VoteDrinkOfTheDayRequest
    {
        public Guid UserId { get; set; }
        public int DrinkId { get; set; }
    }
}
