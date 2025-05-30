using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Net.Http.Json;
using DataAccess.DTOModels;
using DataAccess.Service.Interfaces;
using WinUiApp.Data.Data;
using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
using System.Diagnostics;

namespace DataAccess.ServiceProxy
{
    public class ReviewsServiceProxy : IReviewService
    {
        private readonly HttpClient httpClient;
        private const string ApiRoute = "api/reviews";

        public ReviewsServiceProxy(string baseUrl)
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseUrl);
        }

        public async Task<int> AddReview(ReviewDTO reviewDto)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync($"{ApiRoute}/add", reviewDto);
            response.EnsureSuccessStatusCode();
            return reviewDto.ReviewId + 1;
        }

        public IEnumerable<ReviewDTO> GetReviewsByRating(int ratingId)
        {
            try
            {
                var response = this.httpClient.GetAsync($"Review/get-by-rating?ratingId={ratingId}").Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadFromJsonAsync<IEnumerable<ReviewDTO>>().Result;
            }
            catch (Exception exception)
            {
                throw new Exception($"Error happened while getting reviews for rating with ID {ratingId}:", exception);
            }
        }

        public async Task<List<ReviewDTO>> GetAllReviews()
        {
            HttpResponseMessage response = await httpClient.GetAsync(ApiRoute);
            response.EnsureSuccessStatusCode();
            List<ReviewDTO>? reviews = await response.Content.ReadFromJsonAsync<List<ReviewDTO>>();
            return reviews ?? new List<ReviewDTO>();
        }

        public async Task<double> GetAverageRatingForVisibleReviews()
        {
            List<ReviewDTO> reviews = await GetAllReviews();
            double average = 0;
            int numberOfVisibleReviews = 0;
            foreach (ReviewDTO review in reviews)
            {
                if (review.IsHidden == false)
                {
                    average += review.RatingValue ?? 0;
                    numberOfVisibleReviews++;
                }
            }
            return numberOfVisibleReviews > 0 ? average / numberOfVisibleReviews : 0;
        }

        public async Task<List<ReviewDTO>> GetFlaggedReviews(int minFlags)
        {
            HttpResponseMessage response = await httpClient.GetAsync(ApiRoute);
            response.EnsureSuccessStatusCode();
            List<ReviewDTO> reviews = await response.Content.ReadFromJsonAsync<List<ReviewDTO>>() ?? new List<ReviewDTO>();

            return reviews.Where(review => review.NumberOfFlags >= minFlags && !review.IsHidden).ToList();
        }

        public async Task<List<ReviewDTO>> GetHiddenReviews()
        {
            HttpResponseMessage response = await httpClient.GetAsync(ApiRoute);
            response.EnsureSuccessStatusCode();
            List<ReviewDTO> reviews = await response.Content.ReadFromJsonAsync<List<ReviewDTO>>() ?? new List<ReviewDTO>();
            return reviews.Where(review => review.IsHidden).ToList();
        }

        public async Task<List<ReviewDTO>> GetMostRecentReviews(int count)
        {
            HttpResponseMessage response = await httpClient.GetAsync(ApiRoute);
            response.EnsureSuccessStatusCode();
            List<ReviewDTO> reviews = await response.Content.ReadFromJsonAsync<List<ReviewDTO>>() ?? new List<ReviewDTO>();

            return reviews.Where(review => !review.IsHidden).OrderByDescending(review => review.CreatedDate).Take(count).ToList();
        }

        public async Task<ReviewDTO?> GetReviewById(int reviewID)
        {
            HttpResponseMessage response = await httpClient.GetAsync(ApiRoute);
            response.EnsureSuccessStatusCode();
            List<ReviewDTO> reviews = await response.Content.ReadFromJsonAsync<List<ReviewDTO>>() ?? new List<ReviewDTO>();

            return reviews.FirstOrDefault(review => review.ReviewId == reviewID);
        }

        public async Task<int> GetReviewCountAfterDate(DateTime date)
        {
            HttpResponseMessage response = await httpClient.GetAsync(ApiRoute);
            response.EnsureSuccessStatusCode();
            List<ReviewDTO> reviews = await response.Content.ReadFromJsonAsync<List<ReviewDTO>>() ?? new List<ReviewDTO>();

            return reviews.Count(review => review.CreatedDate == date);
        }

        public async Task<List<ReviewDTO>> GetReviewsByUser(Guid userId)
        {
            HttpResponseMessage response = await httpClient.GetAsync(ApiRoute);
            response.EnsureSuccessStatusCode();
            List<ReviewDTO> reviews = await response.Content.ReadFromJsonAsync<List<ReviewDTO>>() ?? new List<ReviewDTO>();

            return reviews.Where(review => review.UserId == userId).ToList();
        }

        public async Task<List<ReviewDTO>> GetReviewsSince(DateTime date)
        {
            HttpResponseMessage response = await httpClient.GetAsync(ApiRoute);
            response.EnsureSuccessStatusCode();
            List<ReviewDTO> reviews = await response.Content.ReadFromJsonAsync<List<ReviewDTO>>() ?? new List<ReviewDTO>();

            return reviews.Where(review => review.CreatedDate >= date).ToList();
        }

        public async Task RemoveReviewById(int reviewID)
        {
            string reviewUrl = $"{ApiRoute}/{reviewID}";
            HttpResponseMessage response = await httpClient.DeleteAsync(reviewUrl);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateNumberOfFlagsForReview(int reviewID, int numberOfFlags)
        {
            string reviewUrl = $"{ApiRoute}/{reviewID}/updateFlags";
            HttpResponseMessage response = await httpClient.PatchAsJsonAsync(reviewUrl, numberOfFlags);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateReviewVisibility(int reviewID, bool isHidden)
        {
            string reviewUrl = $"{ApiRoute}/{reviewID}/updateVisibility";
            HttpResponseMessage response = await httpClient.PatchAsJsonAsync(reviewUrl, isHidden);
            response.EnsureSuccessStatusCode();
        }

        public async Task HideReview(int reviewID)
        {
            await UpdateReviewVisibility(reviewID, true);
        }

        public async Task ResetReviewFlags(int reviewId)
        {
            await UpdateNumberOfFlagsForReview(reviewId, 0);
        }

        public async Task<List<ReviewDTO>> GetReviewsForReport()
        {
            return await GetAllReviews();
        }

        public async Task<List<ReviewDTO>> FilterReviewsByContent(string content)
        {
            List<ReviewDTO> reviews = await GetAllReviews();
            return reviews.Where(review => review.Content.Contains(content, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public async Task<List<ReviewDTO>> GetReviewsByDrink(int drinkId)
        {
            try
            {
                var response = await this.httpClient.GetAsync($"{ApiRoute}/get-reviews-by-drink?drinkId={drinkId}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<ReviewDTO>>() ?? new List<ReviewDTO>();
            }
            catch (Exception exception)
            {
                throw new Exception($"Error happened while getting reviews for drink with ID {drinkId}:", exception);
            }
        }

        public async Task<double> GetAverageRating(int drinkId)
        {
            try
            {
                var response = await this.httpClient.GetAsync($"{ApiRoute}/get-average-rating-by-drink?drinkId={drinkId}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<double>();
            }
            catch (Exception exception)
            {
                throw new Exception($"Error happened while getting average rating for drink with ID {drinkId}:", exception);
            }
        }
    }
}