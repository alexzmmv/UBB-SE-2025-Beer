// <copyright file="RatingExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.WebAPI.Extensions
{
    using System;
    using WinUiApp.Data.Data;
    using WinUIApp.WebAPI.Models;
    using WinUIApp.WebAPI.Constants;

    /// <summary>
    /// Extension methods for the <see cref="Rating"/> class.
    /// </summary>
    public static class RatingExtensions
    {
        private const int NoRatingValue = 0;
        /// <summary>
        /// Checks if a rating is valid.
        /// </summary>
        /// <param name="rating">The rating to validate.</param>
        /// <returns>True if the rating is valid; otherwise, false.</returns>
        public static bool IsValid(this Rating rating)
        {
            return rating != null 
                && rating.RatingValue.HasValue 
                && rating.RatingValue >= RatingDomainConstants.MinRatingValue 
                && rating.RatingValue <= RatingDomainConstants.MaxRatingValue;
        }
        
        public static Rating? ToDataModel(this RatingDTO? ratingDto)
        {
            if (ratingDto == null) return null;

            return new Rating
            {
                RatingDate = ratingDto.RatingDate,
                RatingValue = ratingDto.RatingValue,
                UserId = ratingDto.UserId,
                DrinkId = ratingDto.DrinkId,
                IsActive = ratingDto.IsActive,
            };
        }

        public static RatingDTO? ToModel(this Rating? rating)
        {
            if (rating == null) return null;
        
            return new RatingDTO
            {
                RatingId = rating.RatingId,
                RatingDate = rating.RatingDate ?? new DateTime(),
                RatingValue = rating.RatingValue ?? NoRatingValue,
                UserId = rating.UserId,
                DrinkId = rating.DrinkId,
                IsActive = rating.IsActive ?? false,
            };
        }

        public static IEnumerable<RatingDTO?> ToModels(this IEnumerable<Rating?> ratings)
        {
            return ratings.Select(rating => rating.ToModel());
        }
    }
} 
