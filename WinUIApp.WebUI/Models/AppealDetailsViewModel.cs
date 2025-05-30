using DataAccess.Model.Authentication;
using DataAccess.Model.AdminDashboard;
using WinUiApp.Data.Data;
namespace WebServer.Models
{
    public class AppealDetailsViewModel
    {
        public required User User { get; set; }
        public required IEnumerable<DataAccess.DTOModels.ReviewDTO> Reviews { get; set; }
    }
}