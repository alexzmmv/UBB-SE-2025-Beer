// <copyright file="Review.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.WebAPI.Models
{
    using System;

    /// <summary>
    /// Represents a user review for a drink.
    /// </summary>
    public class ReviewDTO
    {
        private const int MaxContentLength = 500;

        /// <summary>
        /// Gets or sets the unique identifier for the review.
        /// </summary>
        public int ReviewId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated rating.
        /// </summary>
        public int RatingId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who submitted the review.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the content of the review.
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date and time when the review was created.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the review is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Validates that the review content is not empty and does not exceed the maximum length.
        /// </summary>
        /// <returns>True if the review is valid; otherwise, false.</returns>
    }
}
