using WinUIApp.WebAPI.Models;

namespace DataAccess.DTORequests.Drink
{
    public class UpdateDrinkRequest
    {
        public Guid requestingUserId { get; set; }

        public DrinkDTO Drink { get; set; }
    }
}
