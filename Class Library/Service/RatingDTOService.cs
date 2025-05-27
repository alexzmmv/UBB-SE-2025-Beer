// <copyright file="RatingService.cs" company="PlaceholderCompany">
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
    /// Implementation of the rating service.
    /// </summary>
    public class RatingDTOService : IRatingDTOService
    {
        private readonly IRatingRepository ratingRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RatingService"/> class.
        /// </summary>
        /// <param name="ratingRepository">The rating repository dependency.</param>
        public RatingDTOService(IRatingRepository ratingRepository)
        {
            this.ratingRepository = ratingRepository;
        }

        /// <summary>
        /// Retrieves a rating by its unique identifier.
        /// </summary>
        /// <param name="ratingId">The rating identifier.</param>
        /// <returns>The corresponding rating or null if it doesnt exist.<see cref="RatingDTO"/>.</returns>
        public RatingDTO? GetRatingById(int ratingId) =>
            this.ratingRepository.GetRatingById(ratingId).ToModel();

        /// <summary>
        /// Retrieves all ratings associated with a specific drink.
        /// </summary>
        /// <param name="drinkId">The drink identifier.</param>
        /// <returns>A collection of <see cref="RatingDTO"/> instances for the drink.</returns>
        public IEnumerable<RatingDTO> GetRatingsByDrink(int drinkId) => 
            this.ratingRepository.GetRatingsByDrinkId(drinkId).ToModels();

        /// <summary>
        /// Creates a new rating.
        /// </summary>
        /// <param name="ratingDto">The rating to create.</param>
        /// <returns>The created <see cref="RatingDTO"/> instance.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the rating is invalid.</exception>
        public RatingDTO CreateRating(RatingDTO ratingDto)
        {
            var rating = ratingDto.ToDataModel();

            if (!rating.IsValid())
                throw new ArgumentException(ServiceErrorMessages.InvalidRatingValue);

            var createdRating = this.ratingRepository.AddRating(rating);
            return createdRating.ToModel();
        }

        /// <summary>
        /// Updates an existing rating.
        /// </summary>
        /// <param name="ratingDto">The rating to update.</param>
        /// <returns>The updated <see cref="RatingDTO"/> instance.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the rating is invalid.</exception>
        public RatingDTO UpdateRating(RatingDTO ratingDto)
        {
            var rating = ratingDto.ToDataModel();

            if (!rating.IsValid())
                throw new ArgumentException(ServiceErrorMessages.InvalidRatingValue);

            var updatedRating = this.ratingRepository.UpdateRating(rating);
            return updatedRating.ToModel();
        }

        /// <summary>
        /// Deletes a rating by its unique identifier.
        /// </summary>
        /// <param name="ratingId">The rating identifier.</param>
        public void DeleteRatingById(int ratingId) => 
            this.ratingRepository.DeleteRating(ratingId);


        private const double NoRatingsValue = 0.0;
        /// <summary>
        /// Calculates the average value of all ratings for a drink.
        /// </summary>
        /// <param name="drinkId">The drink identifier.</param>
        /// <returns>The average rating value, or 0 if no ratings are present.</returns>
        public double GetAverageRating(int drinkId)
        {
            var allRatings = this.ratingRepository.GetRatingsByDrinkId(drinkId).ToList();

            var validRatings = allRatings
                .Where(rating => rating.RatingValue.HasValue)
                .Select(rating => rating.RatingValue.Value)
                .ToList();

            if (!validRatings.Any())
                return NoRatingsValue;

            return (double)validRatings.Average();
        }
    }
}
