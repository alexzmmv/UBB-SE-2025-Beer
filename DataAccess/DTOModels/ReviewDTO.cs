namespace DataAccess.DTOModels
{
    using System;

    /// <summary>
    /// Represents a review with associated user, drink, content, and rating.
    /// </summary>
    public class ReviewDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier for the review.
        /// </summary>
        public int ReviewId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the drink.
        /// </summary>
        public int DrinkId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the content of the review.
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the rating value of the review.
        /// </summary>
        public float? RatingValue { get; set; }

        /// <summary>
        /// Gets or sets the date the review was created.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the number of flags for the review.
        /// </summary>
        public int NumberOfFlags { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the review is hidden.
        /// </summary>
        public bool IsHidden { get; set; }
    }
}
