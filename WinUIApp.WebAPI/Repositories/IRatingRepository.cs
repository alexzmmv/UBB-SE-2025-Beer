// <copyright file="IRatingRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.WebAPI.Repositories
{
    using System.Collections.Generic;
    using WinUiApp.Data.Data;

    /// <summary>
    /// Interface for managing rating operations.
    /// </summary>
    public interface IRatingRepository
    {
        /// <summary>
        /// Retrieves a rating by its unique identifier.
        /// </summary>
        /// <param name="ratingId">The unique identifier of the rating.</param>
        /// <returns>The rating with the specified identifier or null if not found.</returns>
        Rating? GetRatingById(int ratingId);

        /// <summary>
        /// Retrieves all ratings from the data store.
        /// </summary>
        /// <returns>A list of all ratings.</returns>
        List<Rating> GetAllRatings();

        /// <summary>
        /// Retrieves all ratings associated with a specific drink.
        /// </summary>
        /// <param name="drinkId">The unique identifier of the drink.</param>
        /// <returns>A list of ratings related to the specified drink.</returns>
        List<Rating> GetRatingsByDrinkId(int drinkId);

        /// <summary>
        /// Adds a new rating to the data store.
        /// </summary>
        /// <param name="rating">The rating to add.</param>
        /// <returns>The added rating with its generated ID.</returns>
        Rating AddRating(Rating rating);

        /// <summary>
        /// Updates an existing rating in the data store.
        /// </summary>
        /// <param name="rating">The rating with updated information.</param>
        /// <returns>The updated rating.</returns>
        Rating UpdateRating(Rating rating);

        /// <summary>
        /// Deletes a rating by its unique identifier.
        /// </summary>
        /// <param name="ratingId">The unique identifier of the rating to delete.</param>
        void DeleteRating(int ratingId);
    }
}
