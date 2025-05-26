// <copyright file="IReviewRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.WebAPI.Repositories
{
    using System.Collections.Generic;
    using WinUiApp.Data.Data;

    /// <summary>
    /// Interface for managing review operations.
    /// </summary>
    public interface IReviewRepository
    {
        /// <summary>
        /// Retrieves a review by its unique identifier.
        /// </summary>
        /// <param name="reviewId">The unique identifier of the review.</param>
        /// <returns>The review with the specified identifier or null if not found.</returns>
        Review? GetReviewById(int reviewId);

        /// <summary>
        /// Retrieves all reviews from the data store.
        /// </summary>
        /// <returns>A list of all reviews.</returns>
        List<Review> GetAllReviews();

        /// <summary>
        /// Retrieves all reviews associated with a specific rating.
        /// </summary>
        /// <param name="ratingId">The unique identifier of the rating.</param>
        /// <returns>A list of reviews related to the specified rating.</returns>
        List<Review> GetReviewsByRatingId(int ratingId);

        /// <summary>
        /// Adds a new review to the data store.
        /// </summary>
        /// <param name="review">The review to add.</param>
        /// <returns>The added review with its generated ID.</returns>
        Review AddReview(Review review);

        /// <summary>
        /// Updates an existing review in the data store.
        /// </summary>
        /// <param name="review">The review with updated information.</param>
        /// <returns>The updated review.</returns>
        Review UpdateReview(Review review);

        /// <summary>
        /// Deletes a review by its unique identifier.
        /// </summary>
        /// <param name="reviewId">The unique identifier of the review to delete.</param>
        void DeleteReview(int reviewId);

        /// <summary>
        /// Checks whether a review with the specified identifier exists in the data store.
        /// </summary>
        /// <param name="reviewId">The unique identifier of the review to check.</param>
        /// <returns>True if the review exists; otherwise, false.</returns>
        bool CheckIfReviewExists(int reviewId);
    }
}
