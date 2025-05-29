namespace WinUIApp.ProxyServices.Requests.Drinks
{
    public class VoteDrinkOfTheDayRequest
    {
        public Guid userId { get; set; }
        public int drinkId { get; set; }
    }
}
