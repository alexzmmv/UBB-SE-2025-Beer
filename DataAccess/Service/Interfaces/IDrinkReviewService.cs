// <copyright file="IDrinkReviewService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.ProxyServices
{
    using System.Collections.Generic;
    using DataAccess.DTOModels;

    /// <summary>
    /// Interface for managing drink reviews.
    /// </summary>
    public interface IDrinkReviewService
    {
        /// <summary>
        /// Adds a review for a drink.
        /// </summary>
        /// <param name="drinkID"> Drink id. </param>
        /// <returns> Average review. </returns>
        float GetReviewAverageByDrinkID(int drinkID);

        /// <summary>
        /// Retrieves all reviews for a specific drink by its ID.
        /// </summary>
        /// <param name="drinkID"> Drink id. </param>
        /// <returns> All reviews. </returns>
        List<ReviewDTO> GetReviewsByDrinkID(int drinkID);
    }
}