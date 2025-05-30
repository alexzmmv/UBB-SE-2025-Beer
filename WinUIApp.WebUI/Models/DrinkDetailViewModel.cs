using System.Collections.Generic;
using System.Linq;
using WinUiApp.Data.Data;
using WinUIApp.ProxyServices.Models;
using WinUIApp.WebAPI.Models;
using WinUIApp.WebUI.Models;

namespace WinUIApp.WebUI.Models
{    public class DrinkDetailViewModel
    {
        public DrinkDetailViewModel()
        {
            Reviews = new List<Review>();
            NewReview = new RatingReviewViewModel();
        }
        
        public required DrinkDTO Drink { get; set; }
        public required string CategoriesDisplay { get; set; }
        public double AverageRatingScore { get; set; }
        public List<Review> Reviews { get; set; }
        public bool IsInFavorites { get; set; }
        
        // For the review form that appears when a rating is tapped
        public RatingReviewViewModel NewReview { get; set; }
    }
}