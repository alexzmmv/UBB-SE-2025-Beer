// <copyright file="RatingExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.WebAPI.Extensions
{
    using System;
    using WinUiApp.Data.Data;
    using WinUIApp.ProxyServices.Constants;
    using WinUIApp.WebAPI.Models;

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
    }
} 
