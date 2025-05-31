using DataAccess.Model.AdminDashboard;
using WinUiApp.Data.Data;

namespace WebServer.Models
{
    public class UserPageModel
    {
        public User CurrentUser { get; set; }
        public IEnumerable<Review> CurrentUserReviews { get; set; }
        public IEnumerable<string> CurrentUserDrinks { get; set; }
        public bool HasPendingUpgradeRequest { get; set; }
    }
}
