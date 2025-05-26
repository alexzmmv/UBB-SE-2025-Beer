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

    public class ReviewsRepository : IReviewsRepository
    {
        private readonly AppDbContext dataContext;

        public ReviewsRepository(AppDbContext context)
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

            // Fix this warning, I am too tired to think of this :(
            return Math.Round((double)visibleReviews.Average(r => r.Rating.RatingValue), 1);
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
    }
}