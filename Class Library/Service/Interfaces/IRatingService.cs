using System.Collections.Generic;
using WinUiApp.Data.Data;

namespace WinUIApp.ProxyServices
{
    public interface IRatingService
    {
        /// <summary>
        /// Retrieves a rating by its unique identifier.
        /// </summary>
        /// <param name="ratingId">The rating identifier.</param>
        /// <returns>The corresponding rating or null if it doesnt exist.<see cref="Rating"/>.</returns>
        public Rating? GetRatingById(int ratingId);

        /// <summary>
        /// Retrieves all ratings associated with a specific product.
        /// </summary>
        /// <param name="
        /// 
        /// ">The product identifier.</param>
        /// <returns>A collection of <see cref="Rating"/> instances for the product.</returns>
        public IEnumerable<Rating> GetRatingsByDrink(int drinkId);

        /// <summary>
        /// Creates a new rating.
        /// </summary>
        /// <param name="rating">The rating to create.</param>
        /// <returns>The created <see cref="Rating"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown when the rating is invalid.</exception>
        public Rating CreateRating(Rating rating);

        /// <summary>
        /// Updates an existing rating.
        /// </summary>
        /// <param name="rating">The rating to update.</param>
        /// <returns>The updated <see cref="Rating"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown when the rating is invalid.</exception>
        public Rating UpdateRating(Rating rating);

        /// <summary>
        /// Deletes a rating by its unique identifier.
        /// </summary>
        /// <param name="ratingId">The rating identifier.</param>
        public void DeleteRatingById(int ratingId);

        /// <summary>
        /// Calculates the average value of all active ratings for a product.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>The average rating value, or 0 if no ratings are present.</returns>
        public double GetAverageRating(int productId);
    }
}