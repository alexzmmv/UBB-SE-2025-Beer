// <copyright file="RepositoryErrorMessages.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.WebAPI.Constants.ErrorMessages
{
    /// <summary>
    /// Error messages used by repositories.
    /// </summary>
    public static class RepositoryErrorMessages
    {
        /// <summary>
        /// Error message for when review content is empty.
        /// </summary>
        public const string EmptyReviewContent = "Review content cannot be null or empty.";

        /// <summary>
        /// Error message for when an entity is not found.
        /// </summary>
        public const string EntityNotFound = "No record found with the provided Id.";
    }
} 
