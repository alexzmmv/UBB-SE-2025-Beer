namespace DataAccess.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess.Model.AdminDashboard;
    using DataAccess.Service.Interfaces;
    using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
    using IRepository;
    using WinUiApp.Data.Data;

    public class ReviewsService : IReviewService
    {
        private IReviewsRepository reviewsRepository;
        private const int REVIEW_ID_FAILURE = -1;

        public ReviewsService(IReviewsRepository reviewsRepository)
        {
            this.reviewsRepository = reviewsRepository;
        }

        public async Task<int> AddReview(Review review)
        {
            try
            {
                return await reviewsRepository.AddReview(review);
            }
            catch
            {
                return REVIEW_ID_FAILURE;
            }
        }

        public async Task RemoveReviewById(int reviewId)
        {
            try
            {
                await reviewsRepository.RemoveReviewById(reviewId);
            }
            catch
            {
            }
        }

        public async Task<Review?> GetReviewById(int reviewId)
        {
            try
            {
                return await reviewsRepository.GetReviewById(reviewId);
            }
            catch
            {
                return null;
            }
        }

        public async Task UpdateNumberOfFlagsForReview(int reviewId, int numberOfFlags)
        {
            try
            {
                await reviewsRepository.UpdateNumberOfFlagsForReview(reviewId, numberOfFlags);
            }
            catch
            {
            }
        }

        public async Task UpdateReviewVisibility(int reviewId, bool isHidden)
        {
            try
            {
                await reviewsRepository.UpdateReviewVisibility(reviewId, isHidden);
            }
            catch
            {
            }
        }

        public async Task ResetReviewFlags(int reviewId)
        {
            try
            {
                await reviewsRepository.UpdateNumberOfFlagsForReview(reviewId, 0);
                Console.WriteLine("Review has 0 flags: " + reviewId);
            }
            catch
            {
            }
        }

        public async Task HideReview(int reviewId)
        {
            try
            {
                await reviewsRepository.UpdateReviewVisibility(reviewId, true);
                Console.WriteLine("Review is hidden");
            }
            catch
            {
            }
        }

        public async Task<List<Review>> GetFlaggedReviews(int minFlags = 1)
        {
            try
            {
                return await reviewsRepository.GetFlaggedReviews(minFlags);
            }
            catch
            {
                return new List<Review>();
            }
        }

        public async Task<List<Review>> GetHiddenReviews()
        {
            try
            {
                List<Review> reviews = await reviewsRepository.GetAllReviews();
                return reviews.Where(review => review.IsHidden == true).ToList();
            }
            catch
            {
                return new List<Review>();
            }
        }

        public async Task<List<Review>> GetAllReviews()
        {
            try
            {
                return await reviewsRepository.GetAllReviews();
            }
            catch
            {
                return new List<Review>();
            }
        }

        public async Task<List<Review>> GetReviewsSince(DateTime date)
        {
            try
            {
                return await reviewsRepository.GetReviewsSince(date);
            }
            catch
            {
                return new List<Review>();
            }
        }

        public async Task<double> GetAverageRatingForVisibleReviews()
        {
            try
            {
                return await reviewsRepository.GetAverageRatingForVisibleReviews();
            }
            catch
            {
                return 0.0;
            }
        }

        public async Task<List<Review>> GetMostRecentReviews(int count)
        {
            try
            {
                return await reviewsRepository.GetMostRecentReviews(count);
            }
            catch
            {
                return new List<Review>();
            }
        }

        public async Task<int> GetReviewCountAfterDate(DateTime date)
        {
            try
            {
                return await reviewsRepository.GetReviewCountAfterDate(date);
            }
            catch
            {
                return 0;
            }
        }

        public async Task<List<Review>> GetReviewsByUser(Guid userId)
        {
            try
            {
                return await reviewsRepository.GetReviewsByUserId(userId);
            }
            catch
            {
                return new List<Review>();
            }
        }

        public async Task<List<Review>> GetReviewsForReport()
        {
            try
            {
                DateTime date = DateTime.Now.AddDays(-1);
                int count = await reviewsRepository.GetReviewCountAfterDate(date);

                List<Review> reviews = await reviewsRepository.GetMostRecentReviews(count);
                return reviews ?? [];
            }
            catch
            {
                return new List<Review>();
            }
        }

        public async Task<List<Review>> FilterReviewsByContent(string content)
        {
            try
            {
                if (string.IsNullOrEmpty(content))
                {
                    return await GetFlaggedReviews();
                }

                content = content.ToLower();
                List<Review> reviews = await GetFlaggedReviews();
                return reviews.Where(review => review.Content.ToLower().Contains(content)).ToList();
            }
            catch
            {
                return new List<Review>();
            }
        }

        public async Task<List<Review>> GetReviewsByDrink(int drinkId)
        {
            try
            {
                return await reviewsRepository.GetReviewsByDrinkId(drinkId);
            }
            catch
            {
                return new List<Review>();
            }
        }

        public async Task<double> GetAverageRating(int drinkId)
        {
            try
            {
                var allReviews = await reviewsRepository.GetReviewsByDrinkId(drinkId);

                var validRatings = allReviews
                    .Where(review => review.RatingValue.HasValue)
                    .Select(review => review.RatingValue.Value)
                    .ToList();

                if (!validRatings.Any())
                    return 0.0;

                return (double)validRatings.Average();
            }
            catch
            {
                return 0.0;
            }
        }
    }
}