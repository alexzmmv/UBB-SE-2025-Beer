using Microsoft.AspNetCore.Mvc;
using WinUIApp.WebAPI.Services;
using System;
using WinUIApp.WebAPI.Models;

namespace WinUIApp.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService reviewService;

        public ReviewController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        [HttpGet("get-by-rating")]
        public IActionResult GetReviewsByRating([FromQuery] int ratingId)
        {
            var reviews = reviewService.GetReviewsByRating(ratingId);
            return Ok(reviews);
        }

        [HttpPost("add")]
        public IActionResult AddReview([FromBody] ReviewDTO reviewDto)
        {
            ArgumentNullException.ThrowIfNull(reviewDto);

            try
            {
                var addedReview = reviewService.AddReview(reviewDto);
                return Ok(addedReview);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete")]
        public IActionResult DeleteReview([FromQuery] int reviewId)
        {
            reviewService.DeleteReviewById(reviewId);
            return Ok();
        }
    }
}
