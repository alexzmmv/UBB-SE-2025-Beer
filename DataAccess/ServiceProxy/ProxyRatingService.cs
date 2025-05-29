namespace WinUIApp.ProxyServices
{
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Json;
    using WinUiApp.Data.Data;
    using WinUIApp.ProxyServices.Models;

    /// <summary>
    /// Proxy service for managing rating-related operations.
    /// </summary>
    public class ProxyRatingService : IRatingService
    {
        private readonly HttpClient httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyRatingService"/> class.
        /// </summary>
        public ProxyRatingService(string baseUrl)
        {
            this.httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl),
            };
        }

        /// <summary>
        /// Retrieves a rating by its unique identifier.
        /// </summary>
        /// <param name="ratingId">The rating identifier.</param>
        /// <returns>The corresponding rating or null if it doesnt exist.<see cref="Rating"/>.</returns>
        /// <exception cref="Exception">Exceptions.</exception>
        public Rating? GetRatingById(int ratingId)
        {
            try
            {
                var response = this.httpClient.GetAsync($"Rating/get-one?ratingId={ratingId}").Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<Rating>().Result;
            }
            catch (Exception exception)
            {
                throw new Exception($"Error happened while getting rating with ID {ratingId}:", exception);
            }
        }

        /// <summary>
        /// Retrieves all ratings associated with a specific product.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>A collection of <see cref="Rating"/> instances for the product.</returns>
        /// <exception cref="Exception">Exceptions.</exception>
        public IEnumerable<Rating> GetRatingsByDrink(int productId)
        {
            try
            {
                var response = this.httpClient.GetAsync($"Rating/get-ratings-by-drink?drinkId={productId}").Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<IEnumerable<Rating>>().Result;
            }
            catch (Exception exception)
            {
                throw new Exception($"Error happened while getting ratings for product with ID {productId}:", exception);
            }
        }

        /// <summary>
        /// Creates a new rating.
        /// </summary>
        /// <param name="rating">The rating to create.</param>
        /// <returns>The created <see cref="Rating"/> instance.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the rating is invalid.</exception>
        /// <exception cref="Exception">Other exceptions.</exception>
        public Rating CreateRating(Rating rating)
        {
            try
            {
                var request = new AddRating
                {
                    ratingDto = rating
                };

                var response = this.httpClient.PostAsJsonAsync("Rating/add", request).Result;
                response.EnsureSuccessStatusCode();
                return rating; // Returning the input rating as the API endpoint doesn't return the created rating
            }
            catch (Exception exception)
            {
                throw new Exception("Error happened while creating a rating:", exception);
            }
        }

        /// <summary>
        /// Updates an existing rating.
        /// </summary>
        /// <param name="rating">The rating to update.</param>
        /// <returns>The updated <see cref="Rating"/> instance.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the rating is invalid.</exception>
        /// <exception cref="Exception">Other exceptions.</exception>
        public Rating UpdateRating(Rating rating)
        {
            try
            {
                var request = new UpdateRatingRequest
                {
                    Rating = rating
                };

                var response = this.httpClient.PutAsJsonAsync("Rating/update", request).Result;
                response.EnsureSuccessStatusCode();
                return rating; // Returning the input rating as the API endpoint doesn't return the updated rating
            }
            catch (Exception exception)
            {
                throw new Exception("Error happened while updating a rating:", exception);
            }
        }

        /// <summary>
        /// Deletes a rating by its unique identifier.
        /// </summary>
        /// <param name="ratingId">The rating identifier.</param>
        /// <exception cref="Exception">Exceptions.</exception>
        public void DeleteRatingById(int ratingId)
        {
            try
            {
                var response = this.httpClient.DeleteAsync($"Rating/delete?ratingId={ratingId}").Result;
                response.EnsureSuccessStatusCode();
            }
            catch (Exception exception)
            {
                throw new Exception($"Error happened while deleting rating with ID {ratingId}:", exception);
            }
        }

        /// <summary>
        /// Calculates the average value of all active ratings for a product.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>The average rating value, or 0 if no ratings are present.</returns>
        /// <exception cref="Exception">Exceptions.</exception>
        public double GetAverageRating(int productId)
        {
            try
            {
                var response = this.httpClient.GetAsync($"Rating/get-average-rating-by-drink?drinkId={productId}").Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<double>().Result;
            }
            catch (Exception exception)
            {
                throw new Exception($"Error happened while getting average rating for product with ID {productId}:", exception);
            }
        }
    }
}
