using DataAccess.Model.AdminDashboard;
using DataAccess.Model.Authentication;
using WinUiApp.Data.Data;

namespace WebServer.Models
{
    public class AdminDashboardViewModel
    {
        public required IEnumerable<DataAccess.DTOModels.ReviewDTO> Reviews { get; set; }

        public required IEnumerable<UpgradeRequest> UpgradeRequests { get; set; }

        public required IEnumerable<string> OffensiveWords { get; set; }

        public required IEnumerable<User> AppealsList { get; set; }

        public IEnumerable<AppealDetailsViewModel> AppealsWithDetails { get; set; }

        public string SearchBarContent { get; set; } = string.Empty;

        public AdminDashboardViewModel()
        {
            this.AppealsWithDetails = new List<AppealDetailsViewModel>();
        }
    }
}
