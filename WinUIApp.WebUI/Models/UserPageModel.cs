using DataAccess.Model.AdminDashboard;
using DataAccess.Model.Authentication;
using WinUiApp.Data.Data;
using WinUIApp.WebAPI.Models;

namespace WebServer.Models
{
    public class UserPageModel
    {
        public required User CurrentUser { get; set; }
        public required IEnumerable<DataAccess.DTOModels.ReviewDTO> CurrentUserReviews { get; set; }
        public List<DrinkDTO> FavoriteDrinks { get; set; } = new();
        public bool HasPendingUpgradeRequest { get; set; }
    }
}
