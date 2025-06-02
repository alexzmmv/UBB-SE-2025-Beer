namespace DrinkDb_Auth.Service.AdminDashboard.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DataAccess.DTOModels;

    public interface IReviewService
    {
        Task<int> AddReview(ReviewDTO review);

        Task RemoveReviewById(int reviewId);

        Task<ReviewDTO?> GetReviewById(int reviewId);

        Task UpdateNumberOfFlagsForReview(int reviewId, int numberOfFlags);

        Task UpdateReviewVisibility(int reviewId, bool isHidden);

        Task HideReview(int reviewID);

        Task<List<ReviewDTO>> GetFlaggedReviews(int minFlags = 1);

        Task<List<ReviewDTO>> GetHiddenReviews();

        Task<List<ReviewDTO>> GetAllReviews();

        Task<List<ReviewDTO>> GetReviewsSince(DateTime date);

        Task<double> GetAverageRatingForVisibleReviews();

        Task<List<ReviewDTO>> GetMostRecentReviews(int count);

        Task<List<ReviewDTO>> GetReviewsForReport();

        Task<int> GetReviewCountAfterDate(DateTime date);

        Task<List<ReviewDTO>> GetReviewsByUser(Guid userId);

        Task ResetReviewFlags(int reviewId);

        Task<List<ReviewDTO>> FilterReviewsByContent(string content);

        Task<List<ReviewDTO>> GetReviewsByDrink(int drinkId);

        Task<double> GetAverageRating(int drinkId);

        Task<List<ReviewWithUserDTO>> GetReviewsWithUserInfoByDrink(int drinkId);

        Task<List<ReviewDTO>> GetReviewsByDrinkAndUser(int drinkId, Guid userId);
    }
}