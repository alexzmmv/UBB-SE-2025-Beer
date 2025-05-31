using WinUIApp.WebAPI.Models;

namespace DataAccess.DTORequests.Drink
{
    public class UpdateDrinkRequest
    {
        public Guid RequestingUserId { get; set; }

        public DrinkDTO Drink { get; set; }
    }
}
