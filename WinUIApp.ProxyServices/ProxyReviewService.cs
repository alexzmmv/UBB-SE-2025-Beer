// <copyright file="ProxyReviewService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.ProxyServices
{
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Json;
    using WinUIApp.ProxyServices.Models;

    /// <summary>
    /// Proxy service for managing review-related operations.
    /// </summary>
    public class ProxyReviewService : IReviewService
    {
        private readonly HttpClient httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyReviewService"/> class.
        /// </summary>
        public ProxyReviewService()
        {
            this.httpClient = new HttpClient
            {
                BaseAddress = new Uri(this.GetApiUrl()),
            };
        }

        /// <summary>
        /// Retrieves all reviews associated with a specific rating.
        /// </summary>
        /// <param name="ratingId">The rating identifier.</param>
        /// <returns>A collection of <see cref="Review"/> instances for the rating.</returns>
        /// <exception cref="Exception">Exceptions.</exception>
        public IEnumerable<Review> GetReviewsByRating(int ratingId)
        {
            try
            {
                var response = this.httpClient.GetAsync($"Review/get-by-rating?ratingId={ratingId}").Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<IEnumerable<Review>>().Result;
            }
            catch (Exception exception)
            {
                throw new Exception($"Error happened while getting reviews for rating with ID {ratingId}:", exception);
            }
        }

        /// <summary>
        /// Adds a new review after validating it.
        /// </summary>
        /// <param name="review">The review to add.</param>
        /// <returns>The added <see cref="Review"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown when the review is invalid.</exception>
        /// <exception cref="Exception">Other exceptions.</exception>
        public Review AddReview(Review review)
        {
            try
            {
                var response = this.httpClient.PostAsJsonAsync("Review/add", review).Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<Review>().Result;
            }
            catch (HttpRequestException exception) when (exception.Message.Contains("400"))
            {
                // The API returns BadRequest (400) when there's a validation error
                throw new ArgumentException("Review validation failed.", exception);
            }
            catch (Exception exception)
            {
                throw new Exception("Error happened while adding a review:", exception);
            }
        }

        /// <summary>
        /// Deletes a review by its unique identifier.
        /// </summary>
        /// <param name="reviewId">The review identifier.</param>
        /// <exception cref="Exception">Exceptions.</exception>
        public void DeleteReviewById(int reviewId)
        {
            try
            {
                var response = this.httpClient.DeleteAsync($"Review/delete?reviewId={reviewId}").Result;
                response.EnsureSuccessStatusCode();
            }
            catch (Exception exception)
            {
                throw new Exception($"Error happened while deleting review with ID {reviewId}:", exception);
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