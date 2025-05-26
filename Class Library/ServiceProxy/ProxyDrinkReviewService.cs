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
        public ProxyDrinkReviewService()
        {
            this.httpClient = new HttpClient
            {
                BaseAddress = new Uri(this.GetApiUrl()),
            };
        }

        public float GetReviewAverageByDrinkID(int drinkID)
        {
            try
            {
                var response = this.httpClient.GetAsync($"Rating/get-average-rating-by-drink?drinkId={drinkID}").Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<float>().Result;
            }
            catch (Exception exception)
            {
                throw new Exception($"Error happened while getting average for drink with ID {drinkID}:", exception);
            }
        }
        public List<Review> GetAllReviewsForRatings(List<Rating> ratings)
        {
            var allReviews = new List<Review>();

            foreach (var rating in ratings)
            {
                try
                {
                    var response = this.httpClient.GetAsync($"Review/get-by-rating?ratingId={rating.RatingId}").Result;
                    response.EnsureSuccessStatusCode();

                    var reviews = response.Content.ReadFromJsonAsync<List<Review>>().Result;
                    if (reviews != null)
                        allReviews.AddRange(reviews);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error while getting reviews for rating ID {rating.RatingId}:", ex);
                }
            }

            return allReviews;
        }

        public List<Review> GetReviewsByDrinkID(int drinkID)
        {
            try
            {
                var response = this.httpClient.GetAsync($"Rating/get-ratings-by-drink?drinkId={drinkID}").Result;
                response.EnsureSuccessStatusCode();
                var ratings = response.Content.ReadFromJsonAsync<List<Rating>>().Result;

                return GetAllReviewsForRatings(ratings);
            }
            catch (Exception exception)
            {
                throw new Exception($"Error happened while getting average for drink with ID {drinkID}:", exception);
            }
        }

        private string GetApiUrl()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            return configuration.GetValue<string>("ApiUrl");
        }
    }
}
