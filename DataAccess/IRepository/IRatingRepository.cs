namespace DataAccess.IRepository
{
    using System.Collections.Generic;
    using WinUiApp.Data.Data;

    public interface IRatingRepository
    {
        Rating? GetRatingById(int ratingId);

        List<Rating> GetAllRatings();

        List<Rating> GetRatingsByDrinkId(int drinkId);

        Rating AddRating(Rating rating);

        Rating UpdateRating(Rating rating);

        void DeleteRating(int ratingId);

        List<Rating> GetRatingsByUserId(Guid userId);
    }
}
