namespace DataAccess.IRepository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DataAccess.DTOModels;
    using WinUiApp.Data.Data;

    public interface IReviewsRepository
    {
        Task<List<ReviewDTO>> GetAllReviews();

        Task<List<ReviewDTO>> GetReviewsSince(DateTime date);

        Task<double> GetAverageRatingForVisibleReviews();

        Task<List<ReviewDTO>> GetMostRecentReviews(int count);

        Task<int> GetReviewCountAfterDate(DateTime date);

        Task<List<ReviewDTO>> GetFlaggedReviews(int minFlags);

        Task<ReviewDTO?> GetReviewById(int reviewID);

        Task UpdateReviewVisibility(int reviewID, bool isHidden);

        Task UpdateNumberOfFlagsForReview(int reviewID, int numberOfFlags);

        // From 923 this returns a Review
        Task<int> AddReview(ReviewDTO review);

        // Changed this to bool
        Task<bool> UpdateReview(ReviewDTO review);

        Task RemoveReviewById(int reviewID);

        Task<List<ReviewDTO>> GetHiddenReviews();

        Task<List<ReviewDTO>> GetReviewsByDrinkId(int drinkId);

        Task<List<ReviewDTO>> GetReviewsByUserId(Guid userId);

        Task<List<ReviewDTO>> GetReviewsByDrinkIdAndUserId(int drinkId, Guid userId);

        Task<List<ReviewWithUserDTO>> GetReviewsWithUserInfoByDrinkId(int drinkId);
    }
}
