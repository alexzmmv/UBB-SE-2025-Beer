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
    using DataAccess.DTOModels;
    using DataAccess.Extensions;

    public class ReviewsRepository : IReviewsRepository
    {
        private readonly IAppDbContext dataContext;

        public ReviewsRepository(IAppDbContext context)
        {
            dataContext = context;
        }

        public async Task<List<ReviewDTO>> GetAllReviews()
        {
            List<Review> reviews = await dataContext.Reviews.ToListAsync();
            return reviews.Select(ReviewMapper.ToDTO).ToList();
        }

        public async Task<List<ReviewDTO>> GetReviewsSince(DateTime date)
        {
            List<Review> reviews = await dataContext.Reviews
                .Where(review => review.CreatedDate >= date && !review.IsHidden)
                .OrderByDescending(review => review.CreatedDate)
                .ToListAsync();
            return reviews.Select(ReviewMapper.ToDTO).ToList();
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

            double average = visibleReviews.Average(r => r.RatingValue ?? 0);
            return Math.Round(average, 1);
        }

        public async Task<List<ReviewDTO>> GetMostRecentReviews(int count)
        {
            List<Review> reviews = await dataContext.Reviews
                .Where(review => !review.IsHidden)
                .OrderByDescending(review => review.CreatedDate)
                .Take(count)
                .ToListAsync();
            return reviews.Select(ReviewMapper.ToDTO).ToList();
        }

        public async Task<int> GetReviewCountAfterDate(DateTime date)
        {
            return await dataContext.Reviews
                .CountAsync(review => review.CreatedDate >= date && !review.IsHidden);
        }

        public async Task<List<ReviewDTO>> GetFlaggedReviews(int minFlags)
        {
            List<Review> reviews = await dataContext.Reviews
                .Where(review => review.NumberOfFlags >= minFlags && !review.IsHidden)
                .ToListAsync();
            return reviews.Select(ReviewMapper.ToDTO).ToList();
        }
        
        public async Task<List<ReviewDTO>> GetReviewsByUser(Guid userId)
        {
            var reviews = await dataContext.Review
                .Where(review => review.UserId == userId && !review.IsHidden)
                .OrderByDescending(review => review.CreatedDate)
                .ToListAsync();
            return reviews.Select(ReviewMapper.ToDTO).ToList();
        }

        public async Task<ReviewDTO?> GetReviewById(int reviewId)
        {
            Review? review = await dataContext.Reviews.FirstOrDefaultAsync(r => r.ReviewId == reviewId);
            return review != null ? ReviewMapper.ToDTO(review) : null;
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

        public async Task<int> AddReview(ReviewDTO reviewDto)
        {
            Review? review = ReviewMapper.ToEntity(reviewDto);
            await dataContext.Reviews.AddAsync(review);
            await dataContext.SaveChangesAsync();
            return review.ReviewId;
        }

        public async Task RemoveReviewById(int reviewId)
        {
            Review? review = await dataContext.Reviews.FirstOrDefaultAsync(r => r.ReviewId == reviewId);

            if (review == null)
            {
                return;
            }

            dataContext.Reviews.Remove(review);
            await dataContext.SaveChangesAsync();
        }

        public async Task<List<ReviewDTO>> GetHiddenReviews()
        {
            List<Review> reviews = await dataContext.Reviews
                .Where(review => review.IsHidden)
                .ToListAsync();
            return reviews.Select(ReviewMapper.ToDTO).ToList();
        }

        public async Task<bool> UpdateReview(ReviewDTO reviewDto)
        {
            Review? existingReview = this.dataContext.Reviews.FirstOrDefault(existingReview => existingReview.ReviewId == reviewDto.ReviewId);

            if (existingReview == null)
            {
                return false;
            }

            existingReview.DrinkId = reviewDto.DrinkId;
            existingReview.UserId = reviewDto.UserId;
            existingReview.RatingValue = reviewDto.RatingValue;
            existingReview.Content = reviewDto.Content;
            existingReview.CreatedDate = reviewDto.CreatedDate;
            existingReview.IsActive = reviewDto.IsHidden;

            await this.dataContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<ReviewDTO>> GetReviewsByDrinkId(int drinkId)
        {
            List<Review> reviews = await dataContext.Reviews
                .Where(review => review.DrinkId == drinkId)
                .ToListAsync();
            return reviews.Select(ReviewMapper.ToDTO).ToList();
        }

        public async Task<List<ReviewDTO>> GetReviewsByUserId(Guid userId)
        {
            List<Review> reviews = await this.dataContext.Reviews
                .Where(review => review.UserId == userId)
                .ToListAsync();
            return reviews.Select(ReviewMapper.ToDTO).ToList();
        }

        public async Task<List<ReviewDTO>> GetReviewsByDrinkIdAndUserId(int drinkId, Guid userId)
        {
            List<Review> reviews = await this.dataContext.Reviews
                .Where(review => review.DrinkId == drinkId && review.UserId == userId)
                .ToListAsync();
            return reviews.Select(ReviewMapper.ToDTO).ToList();
        }
    }
}