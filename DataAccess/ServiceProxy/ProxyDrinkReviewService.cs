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
    using DataAccess.DTOModels;

    /// <summary>
    /// Proxy service for managing drink-related operations.
    /// </summary>
    public class ProxyDrinkReviewService : IDrinkReviewService
    {
        private const int DEFAULT_PERSONAL_DRINK_COUNT = 1;
        private const float MINIMUM_ALCOHOOL_PERCENTAGE = 0f;
        private const float MAXIMUM_ALCOHOOL_PERCENTAGE = 100f;
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
                HttpResponseMessage response = this.httpClient.GetAsync($"api/reviews/get-average-rating-by-drink?drinkId={drinkID}").Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<float>().Result;
            }
            catch (Exception exception)
            {
                throw new Exception($"Error happened while getting average for drink with ID {drinkID}:", exception);
            }
        }

        public List<ReviewDTO> GetReviewsByDrinkID(int drinkID)
        {
            try
            {
                HttpResponseMessage response = this.httpClient.GetAsync($"api/reviews/get-reviews-by-drink?drinkId={drinkID}").Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<List<ReviewDTO>>().Result ?? new List<ReviewDTO>();
            }
            catch (Exception exception)
            {
                throw new Exception($"Error happened while getting reviews for drink with ID {drinkID}:", exception);
            }
        }
    }
}