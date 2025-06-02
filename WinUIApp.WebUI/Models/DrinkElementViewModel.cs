using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
using WinUIApp.ProxyServices.Models;
using WinUIApp.WebAPI.Models;

namespace WinUIApp.WebUI.Models
{
    public class DrinkElementViewModel
    {
        public DrinkDTO Drink { get; set; }
        public double AverageRating { get; set; }
    }
}
