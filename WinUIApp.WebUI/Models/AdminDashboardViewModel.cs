using DataAccess.Data;
using DataAccess.DTOModels;
using DataAccess.Model.AdminDashboard;
using DataAccess.Model.Authentication;
using WinUiApp.Data.Data;
using WinUIApp.WebAPI.Models;

namespace WebServer.Models
{
    public class AdminDashboardViewModel
    {
        public required IEnumerable<DataAccess.DTOModels.ReviewDTO> Reviews { get; set; }

        public required IEnumerable<UpgradeRequest> UpgradeRequests { get; set; }

        public required IEnumerable<string> OffensiveWords { get; set; }

        public required IEnumerable<User> AppealsList { get; set; }

        public required IEnumerable<DrinkModificationRequestDTO> DrinkModificationRequests { get; set; }
        public IEnumerable<AppealDetailsViewModel> AppealsWithDetails { get; set; }

        public required IEnumerable<DrinkDTO> Drinks { get; set; } = new List<DrinkDTO>();
        public string SearchBarContent { get; set; } = string.Empty;

        public required List<User> UsersWithHiddenReviews { get; set; }
        public AdminDashboardViewModel()
        {
            this.AppealsWithDetails = new List<AppealDetailsViewModel>();
        }
    }
}
