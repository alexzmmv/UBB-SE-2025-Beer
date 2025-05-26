// <copyright file="IReviewService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.WebAPI.Services
{
    using System;
    using System.Collections.Generic;
    using WinUiApp.Data.Data;

    /// <summary>
    /// Interface for managing review-related operations.
    /// </summary>
    public interface IReviewService
    {
        /// <summary>
        /// Retrieves all reviews associated with a specific rating.
        /// </summary>
        /// <param name="ratingId">The rating identifier.</param>
        /// <returns>A collection of <see cref="Review"/> instances for the rating.</returns>
        /// <exception cref="Exception"> Any issues. </exception>
        public IEnumerable<Review> GetReviewsByRating(int ratingId);

        /// <summary>
        /// Adds a new review after validating it.
        /// </summary>
        /// <param name="reviewDto">The review to add.</param>
        /// <returns>The added <see cref="Review"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown when the review is invalid.</exception>
        /// <exception cref="Exception"> Any issues. </exception>
        public Review AddReview(WinUIApp.WebAPI.Models.ReviewDTO reviewDto);

        /// <summary>
        /// Deletes a review by its unique identifier.
        /// </summary>
        /// <param name="reviewId">The review identifier.</param>
        /// <exception cref="Exception"> Any issues. </exception>
        public void DeleteReviewById(int reviewId);
    }
}
