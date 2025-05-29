using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("WinUIApp.Tests")]

namespace DataAccess.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess.Model.AdminDashboard;
    using IRepository;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Data.SqlClient;
    using WinUiApp.Data;
    using WinUiApp.Data.Data;
    using WinUiApp.Data.Interfaces;
    using Microsoft.VisualBasic;

    public class ReviewsRepository : IReviewsRepository
    {
        private readonly IAppDbContext dataContext;

        public ReviewsRepository(IAppDbContext context)
        {
            dataContext = context;
        }

        public async Task LoadReviews(IEnumerable<Review> reviewsToLoad)
        {
            await dataContext.Reviews.AddRangeAsync(reviewsToLoad);
            await dataContext.SaveChangesAsync();
        }

        public async Task<List<Review>> GetAllReviews()
        {
            return await dataContext.Reviews.ToListAsync();
        }

        public async Task<List<Review>> GetReviewsSince(DateTime date)
        {
            return await dataContext.Reviews
                .Where(review => review.CreatedDate >= date && !review.IsHidden)
                .OrderByDescending(review => review.CreatedDate)
                .ToListAsync();
        }

        public async Task<double> GetAverageRatingForVisibleReviews()
        {
            List<Review> visibleReviews = await dataContext.Reviews
                .Where(review => !review.IsHidden)
                .ToListAsync();

            if (!visibleReviews.Any())
            {
                return 0.0;
            }

            float? average = visibleReviews.Average(r => r.Rating?.RatingValue);

            if (average == null)
            {
                return 0.0;
            }

            return Math.Round((double)average, 1);
        }

        public async Task<List<Review>> GetMostRecentReviews(int count)
        {
            return await dataContext.Reviews
                .Where(review => !review.IsHidden)
                .OrderByDescending(review => review.CreatedDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<int> GetReviewCountAfterDate(DateTime date)
        {
            return await dataContext.Reviews
                .CountAsync(review => review.CreatedDate >= date && !review.IsHidden);
        }

        public async Task<List<Review>> GetFlaggedReviews(int minFlags)
        {
            return await dataContext.Reviews
                .Where(review => review.NumberOfFlags >= minFlags && !review.IsHidden)
                .ToListAsync();
        }

        public async Task<List<Review>> GetReviewsByUser(Guid userId)
        {
            return await dataContext.Reviews
                .Where(review => review.UserId == userId && !review.IsHidden)
                .OrderByDescending(review => review.CreatedDate)
                .ToListAsync();
        }

        public async Task<Review?> GetReviewById(int reviewId)
        {
            return await dataContext.Reviews.FirstOrDefaultAsync(r => r.ReviewId == reviewId);
        }

        public async Task UpdateReviewVisibility(int reviewId, bool isHidden)
        {
            Review? review = dataContext.Reviews.Find(reviewId);

            if (review == null)
            {
                return;
            }

            review.IsHidden = isHidden;
            dataContext.Reviews.Update(review);
            await dataContext.SaveChangesAsync();
        }

        public async Task UpdateNumberOfFlagsForReview(int reviewId, int numberOfFlags)
        {
            Review? review = dataContext.Reviews.Find(reviewId);

            if (review == null)
            {
                return;
            }

            review.NumberOfFlags = numberOfFlags;
            dataContext.Reviews.Update(review);
            await dataContext.SaveChangesAsync();
        }

        public async Task<int> AddReview(Review review)
        {
            await dataContext.Reviews.AddAsync(review);
            await dataContext.SaveChangesAsync();
            return review.ReviewId;
        }

        public async Task RemoveReviewById(int reviewId)
        {
            Review? review = await GetReviewById(reviewId);

            if (review == null)
            {
                return;
            }

            dataContext.Reviews.Remove(review);
            await dataContext.SaveChangesAsync();
        }

        public async Task<List<Review>> GetHiddenReviews()
        {
            return await dataContext.Reviews
                .Where(review => review.IsHidden)
                .ToListAsync();
        }

        public async Task<bool> UpdateReview(Review review)
        {
            Review? existingReview = this.dataContext.Reviews.FirstOrDefault(existingReview => existingReview.ReviewId == review.ReviewId);

            if (existingReview == null)
            {
                return false;
            }

            existingReview.DrinkId = review.DrinkId;
            existingReview.UserId = review.UserId;
            existingReview.RatingValue = review.RatingValue;
            existingReview.Content = review.Content;
            existingReview.CreationDate = review.CreationDate ?? existingReview.CreationDate;
            existingReview.IsActive = review.IsActive ?? existingReview.IsActive;

            await this.dataContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<Review>> GetReviewsByDrinkId(int drinkId)
        {
            return await dataContext.Reviews
                .Where(review => review.DrinkId == drinkId)
                .ToListAsync();
        }

        public async Task<List<Review>> GetReviewsByUserId(Guid userId)
        {
            return await this.dataContext.Reviews
                .Where(review => review.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Review>> GetReviewsByDrinkIdAndUserId(int drinkId, Guid userId)
        {
            return await this.dataContext.Reviews
                .Where(review => review.DrinkId == drinkId && review.UserId == userId)
                .ToListAsync();
        }
    }
}