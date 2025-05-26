// <copyright file="ReviewService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
using WinUIApp.WebAPI.Models;

namespace WinUIApp.WebAPI.Services
{
    using System;
    using System.Collections.Generic;
    using WinUiApp.Data.Data;
    using WinUIApp.WebAPI.Constants.ErrorMessages;
    using WinUIApp.WebAPI.Extensions;
    using WinUIApp.WebAPI.Repositories;

    /// <summary>
    /// Implementation of the review service.
    /// </summary>
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository reviewRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReviewService"/> class.
        /// </summary>
        /// <param name="reviewRepository">The review repository dependency.</param>
        public ReviewService(IReviewRepository reviewRepository)
        {
            this.reviewRepository = reviewRepository;
        }

        /// <summary>
        /// Retrieves all reviews associated with a specific rating.
        /// </summary>
        /// <param name="ratingId">The rating identifier.</param>
        /// <returns>A collection of <see cref="Review"/> instances for the rating.</returns>
        public IEnumerable<Review> GetReviewsByRating(int ratingId)
            => this.reviewRepository.GetReviewsByRatingId(ratingId);

        /// <summary>
        /// Adds a new review after validating it.
        /// </summary>
        /// <param name="reviewDto">The review to add.</param>
        /// <returns>The added <see cref="Review"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown when the review is invalid.</exception>
        public Review AddReview(ReviewDTO reviewDto)
        {
            var reviewToAdd = reviewDto.ToDataModel();

            if (!reviewToAdd.IsValid())
            {
                throw new ArgumentException(ServiceErrorMessages.InvalidReview);
            }

            reviewToAdd.Activate();
            return this.reviewRepository.AddReview(reviewToAdd);
        }

        /// <summary>
        /// Deletes a review by its unique identifier.
        /// </summary>
        /// <param name="reviewId">The review identifier.</param>
        public void DeleteReviewById(int reviewId)
            => this.reviewRepository.DeleteReview(reviewId);
    }
}
