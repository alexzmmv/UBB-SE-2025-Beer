namespace IRepository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DataAccess.Model.AdminDashboard;
    using WinUiApp.Data.Data;

    public interface IReviewsRepository
    {
        Task<List<Review>> GetAllReviews();

        Task<List<Review>> GetReviewsSince(DateTime date);

        Task<double> GetAverageRatingForVisibleReviews();

        Task<List<Review>> GetMostRecentReviews(int count);

        Task<int> GetReviewCountAfterDate(DateTime date);

        Task<List<Review>> GetFlaggedReviews(int minFlags);

        Task<List<Review>> GetReviewsByUser(Guid userId);

        Task<Review?> GetReviewById(int reviewID);

        Task UpdateReviewVisibility(int reviewID, bool isHidden);

        Task UpdateNumberOfFlagsForReview(int reviewID, int numberOfFlags);

        // From 923 this returns a Review
        Task<int> AddReview(Review review);

        // Changed this to bool
        Task<bool> UpdateReview(Review review);

        Task RemoveReviewById(int reviewID);

        Task<List<Review>> GetHiddenReviews();

        Task<List<Review>> GetReviewsByRatingId(int ratingId);
    }
}
