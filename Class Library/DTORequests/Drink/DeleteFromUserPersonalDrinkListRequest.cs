namespace WinUIApp.WebAPI.Requests.Drink
{
    public class DeleteFromUserPersonalDrinkListRequest
    {
        public Guid userId { get; set; }
        public int drinkId { get; set; }
    }
}
