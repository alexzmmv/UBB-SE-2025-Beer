// <copyright file="ReviewDomainConstants.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.WebAPI.Constants
{
    /// <summary>
    /// Constants related to review domain rules.
    /// </summary>
    public static class ReviewDomainConstants
    {
        /// <summary>
        /// The maximum allowed length for review content.
        /// </summary>
        public const int MAX_CONTENT_LENGTH = 500;

        /// <summary>
        /// The minimum allowed rating value.
        /// </summary>
        public const int MIN_RATING_VALUE = 1;

        /// <summary>
        /// The maximum allowed rating value.
        /// </summary>
        public const int MAX_RATING_VALUE = 5;
    }
} 
