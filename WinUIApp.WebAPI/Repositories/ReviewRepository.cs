// <copyright file="ReviewRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("WinUIApp.Tests")]

namespace WinUIApp.WebAPI.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using WinUiApp.Data;
    using WinUiApp.Data.Data;
    using WinUiApp.Data.Interfaces;
    using WinUIApp.WebAPI.Constants.ErrorMessages;

    /// <summary>
    /// Repository for managing review-related operations.
    /// </summary>
    internal class ReviewRepository : IReviewRepository
    {
        private readonly IAppDbContext dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReviewRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public ReviewRepository(IAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <inheritdoc/>
        public Review? GetReviewById(int reviewId)
        {
            return this.dbContext.Reviews
                .FirstOrDefault(review => review.ReviewId == reviewId);
        }

        /// <inheritdoc/>
        public List<Review> GetAllReviews()
        {
            return this.dbContext.Reviews.ToList();
        }

        /// <inheritdoc/>
        public List<Review> GetReviewsByRatingId(int ratingId)
        {
            return this.dbContext.Reviews
                .Where(rating => rating.Rating.RatingId == ratingId)
                .ToList();
        }

        /// <inheritdoc/>
        public Review AddReview(Review review)
        {
            if (string.IsNullOrWhiteSpace(review.Content))
                throw new ArgumentException(RepositoryErrorMessages.EmptyReviewContent, nameof(review.Content));

            review.CreationDate ??= DateTime.UtcNow;
            review.IsActive ??= true;

            this.dbContext.Reviews.Add(review);
            this.dbContext.SaveChanges();

            return review;
        }

        /// <inheritdoc/>
        public Review UpdateReview(Review review)
        {
            if (string.IsNullOrWhiteSpace(review.Content))
                throw new ArgumentException(RepositoryErrorMessages.EmptyReviewContent, nameof(review.Content));

            var existingReview = this.dbContext.Reviews
                .FirstOrDefault(existingReview => existingReview.ReviewId == review.ReviewId);

            if (existingReview == null)
                throw new Exception(RepositoryErrorMessages.EntityNotFound);

            existingReview.RatingId = review.RatingId;
            existingReview.UserId = review.UserId;
            existingReview.Content = review.Content;
            existingReview.CreationDate = review.CreationDate ?? existingReview.CreationDate;
            existingReview.IsActive = review.IsActive ?? existingReview.IsActive;

            this.dbContext.SaveChanges();

            return existingReview;
        }

        /// <inheritdoc/>
        public void DeleteReview(int reviewId)
        {
            var review = this.dbContext.Reviews
                .FirstOrDefault(review => review.ReviewId == reviewId);

            if (review == null)
                throw new Exception(RepositoryErrorMessages.EntityNotFound);

            this.dbContext.Reviews.Remove(review);
            this.dbContext.SaveChanges();
        }

        /// <inheritdoc/>
        public bool CheckIfReviewExists(int reviewId)
        {
            return this.dbContext.Reviews.Any(review => review.ReviewId == reviewId);
        }
    }
}
