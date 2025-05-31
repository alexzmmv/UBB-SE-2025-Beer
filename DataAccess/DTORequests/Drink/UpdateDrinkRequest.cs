using WinUIApp.WebAPI.Models;

namespace WinUIApp.WebAPI.Requests.Drink
{
    public class UpdateDrinkRequest
    {
        public Guid requestingUserId { get; set; }

        public DrinkDTO Drink { get; set; }
    }
}
