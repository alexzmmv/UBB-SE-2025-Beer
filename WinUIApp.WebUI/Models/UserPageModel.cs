﻿using DataAccess.Model.AdminDashboard;
using DataAccess.Model.Authentication;
using WinUiApp.Data.Data;

namespace WebServer.Models
{
    public class UserPageModel
    {
        public required User CurrentUser { get; set; }
        public required IEnumerable<Review> CurrentUserReviews { get; set; }
        public required IEnumerable<string> CurrentUserDrinks { get; set; }
    }
}