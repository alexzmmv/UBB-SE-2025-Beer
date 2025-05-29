using System.Collections.Generic;
using WinUiApp.Data.Data;

namespace WinUIApp.ProxyServices
{
    public interface IRatingService
    {
        public Rating? GetRatingById(int ratingId);
        public IEnumerable<Rating> GetRatingsByDrink(int drinkId);
        public Rating CreateRating(Rating rating);
        public Rating UpdateRating(Rating rating);
        public void DeleteRatingById(int ratingId);
        public double GetAverageRating(int productId);
    }
}