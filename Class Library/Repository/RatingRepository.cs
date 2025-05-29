// <copyright file="RatingRepository.cs" company="PlaceholderCompany">
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
    /// Repository for managing rating-related operations.
    /// </summary>
    public class RatingRepository : IRatingRepository
    {
        private readonly IAppDbContext dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="RatingRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public RatingRepository(IAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <inheritdoc/>
        public Rating? GetRatingById(int ratingId)
        {
            return this.dbContext.Ratings
                .FirstOrDefault(rating => rating.RatingId == ratingId);
        }

        /// <inheritdoc/>
        public List<Rating> GetAllRatings()
        {
            return this.dbContext.Ratings.ToList();
        }

        /// <inheritdoc/>
        public List<Rating> GetRatingsByDrinkId(int drinkId)
        {
            return this.dbContext.Ratings
                .Where(rating => rating.DrinkId == drinkId)
                .ToList();
        }

        /// <inheritdoc/>
        public Rating AddRating(Rating rating)
        {
            rating.RatingDate ??= DateTime.UtcNow;
            rating.IsActive ??= true;

            this.dbContext.Ratings.Add(rating);
            this.dbContext.SaveChanges();
            
            return rating;
        }

        /// <inheritdoc/>
        public Rating UpdateRating(Rating rating)
        {
            var existingRating = this.dbContext.Ratings
                .FirstOrDefault(existingRating => existingRating.RatingId == rating.RatingId);

            if (existingRating == null)
                throw new Exception(RepositoryErrorMessages.EntityNotFound);

            existingRating.DrinkId = rating.DrinkId;
            existingRating.UserId = rating.UserId;
            existingRating.RatingValue = rating.RatingValue;
            existingRating.RatingDate = rating.RatingDate ?? existingRating.RatingDate;
            existingRating.IsActive = rating.IsActive ?? existingRating.IsActive;

            this.dbContext.SaveChanges();
            
            return existingRating;
        }

        /// <inheritdoc/>
        public void DeleteRating(int ratingId)
        {
            var rating = this.dbContext.Ratings
                .FirstOrDefault(rating => rating.RatingId == ratingId);

            if (rating == null)
                throw new Exception(RepositoryErrorMessages.EntityNotFound);

            this.dbContext.Ratings.Remove(rating);
            this.dbContext.SaveChanges();
        }
    }
}
