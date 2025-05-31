namespace DataAccess.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess.IRepository;
    using DataAccess.DTOModels;
    using DataAccess.Service.Interfaces;
    using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
    using WinUiApp.Data.Data;
    using DataAccess.Extensions;

    public class ReviewsService : IReviewService
    {
        private IReviewsRepository reviewsRepository;
        private const int REVIEW_ID_FAILURE = -1;

        public ReviewsService(IReviewsRepository reviewsRepository)
        {
            this.reviewsRepository = reviewsRepository;
        }

        public async Task<int> AddReview(ReviewDTO reviewDto)
        {
            try
            {
                if (!ReviewValidator.IsValid(reviewDto))
                {
                    throw new ArgumentException("Invalid review data.");
                }
                return await reviewsRepository.AddReview(reviewDto);
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

        public async Task<ReviewDTO?> GetReviewById(int reviewId)
        {
            try
            {
                var reviewDto = await reviewsRepository.GetReviewById(reviewId);
                return reviewDto;
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
                var reviewDto = await reviewsRepository.GetReviewById(reviewId);
                if (reviewDto == null || !ReviewValidator.IsValid(reviewDto))
                {
                    throw new ArgumentException("Invalid review data.");
                }
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

        public virtual async Task<List<ReviewDTO>> GetFlaggedReviews(int minFlags = 1)
        {
            try
            {
                return await reviewsRepository.GetFlaggedReviews(minFlags);
            }
            catch
            {
                return new List<ReviewDTO>();
            }
        }

        public async Task<List<ReviewDTO>> GetHiddenReviews()
        {
            try
            {
                var reviews = await reviewsRepository.GetAllReviews();
                return reviews.Where(review => review.IsHidden == true).ToList();
            }
            catch
            {
                return new List<ReviewDTO>();
            }
        }

        public async Task<List<ReviewDTO>> GetAllReviews()
        {
            try
            {
                return await reviewsRepository.GetAllReviews();
            }
            catch
            {
                return new List<ReviewDTO>();
            }
        }

        public async Task<List<ReviewDTO>> GetReviewsSince(DateTime date)
        {
            try
            {
                return await reviewsRepository.GetReviewsSince(date);
            }
            catch
            {
                return new List<ReviewDTO>();
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

        public async Task<List<ReviewDTO>> GetMostRecentReviews(int count)
        {
            try
            {
                return await reviewsRepository.GetMostRecentReviews(count);
            }
            catch
            {
                return new List<ReviewDTO>();
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

        public async Task<List<ReviewDTO>> GetReviewsByUser(Guid userId)
        {
            try
            {
                return await reviewsRepository.GetReviewsByUserId(userId);
            }
            catch
            {
                return new List<ReviewDTO>();
            }
        }

        public async Task<List<ReviewDTO>> GetReviewsForReport()
        {
            try
            {
                DateTime date = DateTime.Now.AddDays(-1);
                int count = await reviewsRepository.GetReviewCountAfterDate(date);

                var reviews = await reviewsRepository.GetMostRecentReviews(count);
                return reviews ?? new List<ReviewDTO>();
            }
            catch
            {
                return new List<ReviewDTO>();
            }
        }

        public async Task<List<ReviewDTO>> FilterReviewsByContent(string content)
        {
            try
            {
                if (string.IsNullOrEmpty(content))
                {
                    return await GetFlaggedReviews();
                }

                content = content.ToLower();
                var reviews = await GetFlaggedReviews();
                return reviews.Where(review => review.Content.ToLower().Contains(content)).ToList();
            }
            catch
            {
                return new List<ReviewDTO>();
            }
        }

        public async Task<List<ReviewDTO>> GetReviewsByDrink(int drinkId)
        {
            try
            {
                return await reviewsRepository.GetReviewsByDrinkId(drinkId);
            }
            catch
            {
                return new List<ReviewDTO>();
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