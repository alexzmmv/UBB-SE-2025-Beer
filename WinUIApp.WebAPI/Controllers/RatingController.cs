
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WinUIApp.WebAPI.Models;
using WinUIApp.WebAPI.Requests.Rating;
using WinUIApp.WebAPI.Services;

namespace WinUIApp.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService ratingService;

        public RatingController(IRatingService ratingService)
        {
            this.ratingService = ratingService;
        }
        
        [HttpGet("get-one")]
        public IActionResult GetRatingById([FromQuery] int ratingId)
        {
            return Ok(ratingService.GetRatingById(ratingId));
        }
        
        [HttpGet("get-ratings-by-drink")]
        public IActionResult GetRatingsByDrink([FromQuery] int drinkId)
        {
            return Ok(ratingService.GetRatingsByDrink(drinkId));
        }
        
        [HttpGet("get-average-rating-by-drink")]
        public IActionResult GetAverageRating([FromQuery] int drinkId)
        {
            return Ok(ratingService.GetAverageRating(drinkId));
        }
            
        [HttpPost("add")]
        public IActionResult AddRating([FromBody] AddRatingRequest request)
        {
            ratingService.CreateRating(request.RatingDto);
            
            return Ok();
        }
        
        [HttpPut("update")]
        public IActionResult UpdateRating([FromBody] UpdateRatingRequest request)
        {
            ratingService.UpdateRating(request.RatingDto);
            
            return Ok();
        }
        
        [HttpDelete("delete")]
        public IActionResult DeleteRating([FromQuery] int ratingId)
        {
            ratingService.DeleteRatingById(ratingId);
            
            return Ok();
        }
    }
}
