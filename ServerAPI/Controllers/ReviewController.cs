using Microsoft.AspNetCore.Mvc;
using DataAccess.DTOModels;
using DrinkDb_Auth.Service.AdminDashboard.Interfaces;

namespace WinUIApp.WebAPI.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService reviewService;

        public ReviewController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        [HttpGet("")]
        public async Task<IEnumerable<ReviewDTO>> GetAll()
        {
            return await this.reviewService.GetAllReviews();
        }

        [HttpGet("since")]
        public async Task<IEnumerable<ReviewDTO>> GetReviewsSince([FromQuery] DateTime date)
        {
            return await this.reviewService.GetReviewsSince(date);
        }

        [HttpGet("averageRatingVisibleReviews")]
        public async Task<double> GetAverageRatingForVisibleReviews()
        {
            return await this.reviewService.GetAverageRatingForVisibleReviews();
        }

        [HttpGet("mostRecent")]
        public async Task<IEnumerable<ReviewDTO>> GetMostRecentReviews([FromQuery] int count)
        {
            return await this.reviewService.GetMostRecentReviews(count);
        }

        [HttpGet("countAfterDate")]
        public async Task<int> GetReviewCountAfterDate([FromQuery] DateTime date)
        {
            return await this.reviewService.GetReviewCountAfterDate(date);
        }

        [HttpGet("flagged")]
        public async Task<IEnumerable<ReviewDTO>> GetFlaggedReviews([FromQuery] int minFlags)
        {
            return await this.reviewService.GetFlaggedReviews(minFlags);
        }

        [HttpGet("byUser")]
        public async Task<IEnumerable<ReviewDTO>> GetReviewsByUser([FromQuery] Guid userId)
        {
            return await this.reviewService.GetReviewsByUser(userId);
        }

        [HttpGet("{id}")]
        public async Task<ReviewDTO?> GetReviewById(int id)
        {
            return await this.reviewService.GetReviewById(id);
        }

        [HttpPatch("{id}/updateFlags")]
        public async Task UpdateNumberOfFlagsForReview(int id, [FromBody] int numberOfFlags)
        {
            await this.reviewService.UpdateNumberOfFlagsForReview(id, numberOfFlags);
        }

        [HttpPatch("{id}/updateVisibility")]
        public async Task UpdateReviewVisibility(int id, [FromBody] bool isHidden)
        {
            await this.reviewService.UpdateReviewVisibility(id, isHidden);
        }

        [HttpPost("add")]
        public async Task<int> AddReview([FromBody] ReviewDTO reviewDto)
        {
            return await this.reviewService.AddReview(reviewDto);
        }

        [HttpDelete("{id}/delete")]
        public async Task RemoveReviewById(int id)
        {
            await this.reviewService.RemoveReviewById(id);
        }

        [HttpGet("hidden")]
        public async Task<IEnumerable<ReviewDTO>> GetHiddenReviews()
        {
            return await this.reviewService.GetHiddenReviews();
        }

        [HttpGet("report")]
        public async Task<IEnumerable<ReviewDTO>> GetReviewsForReport()
        {
            return await this.reviewService.GetReviewsForReport();
        }

        [HttpGet("filter")]
        public async Task<IEnumerable<ReviewDTO>> FilterReviewsByContent([FromQuery] string content)
        {
            return await this.reviewService.FilterReviewsByContent(content);
        }

        [HttpGet("get-reviews-by-drink")]
        public async Task<IActionResult> GetReviewsByDrink([FromQuery] int drinkId)
        {
            List<ReviewDTO> reviews = await reviewService.GetReviewsByDrink(drinkId);
            return Ok(reviews);
        }

        [HttpGet("get-average-rating-by-drink")]
        public async Task<IActionResult> GetAverageRating([FromQuery] int drinkId)
        {
            double averageRating = await reviewService.GetAverageRating(drinkId);
            return Ok(averageRating);
        }

        [HttpGet("get-reviews-with-user-info-by-drink")]
        public async Task<IActionResult> GetReviewsWithUserInfoByDrink([FromQuery] int drinkId)
        {
            List<ReviewWithUserDTO> reviews = await reviewService.GetReviewsWithUserInfoByDrink(drinkId);
            return Ok(reviews);
        }

        [HttpGet("get-reviews-by-drink-and-user")]
        public async Task<IActionResult> GetReviewsByDrinkAndUser([FromQuery] int drinkId, [FromQuery] Guid userId)
        {
            List<ReviewDTO> reviews = await reviewService.GetReviewsByDrinkAndUser(drinkId, userId);
            return Ok(reviews);
        }
    }
}
