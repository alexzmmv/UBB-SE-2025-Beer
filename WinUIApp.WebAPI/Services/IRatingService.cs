// <copyright file="IRatingService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using WinUIApp.WebAPI.Models;

namespace WinUIApp.WebAPI.Services
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Interface for managing rating-related operations.
    /// </summary>
    public interface IRatingService
    {
        /// <summary>
        /// Retrieves a rating by its unique identifier.
        /// </summary>
        /// <param name="ratingId">The rating identifier.</param>
        /// <returns>The corresponding rating or null if it doesnt exist.<see cref="RatingDTO"/>.</returns>
        /// <exception cref="Exception"> Any issues. </exception>
        public RatingDTO? GetRatingById(int ratingId);

        /// <summary>
        /// Retrieves all ratings associated with a specific drink.
        /// </summary>
        /// <param name="drinkId">The drink identifier.</param>
        /// <returns>A collection of <see cref="RatingDTO"/> instances for the drink.</returns>
        /// <exception cref="Exception"> Any issues. </exception>
        public IEnumerable<RatingDTO> GetRatingsByDrink(int drinkId);

        /// <summary>
        /// Creates a new rating.
        /// </summary>
        /// <param name="ratingDto">The rating to create.</param>
        /// <returns>The created <see cref="RatingDTO"/> instance.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the rating is invalid.</exception>
        /// <exception cref="Exception"> Any issues. </exception>
        public RatingDTO CreateRating(RatingDTO ratingDto);

        /// <summary>
        /// Updates an existing rating.
        /// </summary>
        /// <param name="ratingDto">The rating to update.</param>
        /// <returns>The updated <see cref="RatingDTO"/> instance.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the rating is invalid.</exception>
        /// <exception cref="Exception"> Any issues. </exception>
        public RatingDTO UpdateRating(RatingDTO ratingDto);

        /// <summary>
        /// Deletes a rating by its unique identifier.
        /// </summary>
        /// <param name="ratingId">The rating identifier.</param>
        /// <exception cref="Exception"> Any issues. </exception>
        public void DeleteRatingById(int ratingId);

        /// <summary>
        /// Calculates the average value of all active ratings for a drink.
        /// </summary>
        /// <param name="drinkId">The drink identifier.</param>
        /// <returns>The average rating value, or 0 if no ratings are present.</returns>
        /// <exception cref="Exception"> Any issues. </exception>
        public double GetAverageRating(int drinkId);
    }
}
