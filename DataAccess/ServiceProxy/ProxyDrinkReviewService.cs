// <copyright file="ProxyDrinkService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.ProxyServices
{
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Json;
    using WinUIApp.ProxyServices;
    using WinUiApp.Data.Data;


    /// <summary>
    /// Proxy service for managing drink-related operations.
    /// </summary>
    public class ProxyDrinkReviewService : IDrinkReviewService
    {
        private const int DefaultPersonalDrinkCount = 1;
        private const float MinimumAlcoholPercentageConstant = 0f;
        private const float MaximumAlcoholPercentageConstant = 100f;
        private readonly HttpClient httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyDrinkService"/> class.
        /// </summary>
        public ProxyDrinkReviewService(string baseUrl)
        {
            this.httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl),
            };
        }

        public float GetReviewAverageByDrinkID(int drinkID)
        {
            try
            {
                var response = this.httpClient.GetAsync($"api/reviews/get-average-rating-by-drink?drinkId={drinkID}").Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<float>().Result;
            }
            catch (Exception exception)
            {
                throw new Exception($"Error happened while getting average for drink with ID {drinkID}:", exception);
            }
        }

        public List<Review> GetReviewsByDrinkID(int drinkID)
        {
            try
            {
                var response = this.httpClient.GetAsync($"api/reviews/get-reviews-by-drink?drinkId={drinkID}").Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<List<Review>>().Result ?? new List<Review>();
            }
            catch (Exception exception)
            {
                throw new Exception($"Error happened while getting reviews for drink with ID {drinkID}:", exception);
            }
        }
    }
}