using System.Collections.Generic;
using System.Linq;

namespace WebApplication.Models
{
    public class DrinkDetailViewModel
    {
        public DrinkDetailViewModel()
        {
            Reviews = new List<Review>();
            Ratings = new List<Rating>();
            ReviewsByRating = new Dictionary<int, List<Review>>();
            NewReview = new RatingReviewViewModel();
        }

        public required Drink Drink { get; set; }
        public required string CategoriesDisplay { get; set; }
        public double AverageRatingScore { get; set; }
        public List<Review> Reviews { get; set; }
        public List<Rating> Ratings { get; set; }
        public Dictionary<int, List<Review>> ReviewsByRating { get; set; }
        public bool IsInFavorites { get; set; }

        // For the review form that appears when a rating is tapped
        public RatingReviewViewModel NewReview { get; set; }
    }
}