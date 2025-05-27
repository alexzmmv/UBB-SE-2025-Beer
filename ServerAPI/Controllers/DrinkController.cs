using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WinUIApp.WebAPI.Models;
using WinUIApp.WebAPI.Requests.Drink;
using WinUIApp.WebAPI.Services;

namespace WinUIApp.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DrinkController : ControllerBase
    {
        private readonly IDrinkDTOService drinkService;

        public DrinkController(IDrinkDTOService drinkService)
        {
            this.drinkService = drinkService;
        }

        [HttpPost("get-all")]
        public IActionResult GetAllDrinks([FromBody] GetDrinksRequest request)
        {
            return Ok(
                drinkService.GetDrinks(
                    request.searchKeyword,
                    request.drinkBrandNameFilter,
                    request.drinkCategoryFilter,
                    request.minimumAlcoholPercentage,
                    request.maximumAlcoholPercentage,
                    request.orderingCriteria));
        }
        
        [HttpGet("get-one")]
        public IActionResult GetDrinkById([FromQuery] int drinkId)
        {
            return Ok(drinkService.GetDrinkById(drinkId));
        }

        [HttpGet("get-drink-brands")]
        public IActionResult GetDrinkBrands()
        {
            return Ok(drinkService.GetDrinkBrandNames());
        }

        [HttpGet("get-drink-categories")]
        public IActionResult GetDrinkCategories() {
            return Ok(drinkService.GetDrinkCategories());
        }

        [HttpGet("get-drink-of-the-day")]
        public IActionResult GetDrinkOfTheDay()
        {
            return Ok(drinkService.GetDrinkOfTheDay());
        }

        [HttpPost("get-user-drink-list")]
        public IActionResult GetUserPersonalDrinkList([FromBody] GetUserDrinkListRequest request)
        {
            return Ok(drinkService.GetUserPersonalDrinkList(request.userId));
        }
        
        [HttpPost("add")]
        public IActionResult AddDrink([FromBody] AddDrinkRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            drinkService.AddDrink(
                request.inputtedDrinkName,
                request.inputtedDrinkPath,
                request.inputtedDrinkCategories,
                request.inputtedDrinkBrandName,
                request.inputtedAlcoholPercentage);
            return Ok();
        }

        [HttpPost("add-to-user-drink-list")]
        public IActionResult AddToUserPersonalDrinkList([FromBody] AddToUserPersonalDrinkListRequest request)
        {
            return Ok(drinkService.AddToUserPersonalDrinkList(request.userId, request.drinkId));
        }

        [HttpPost("vote-drink-of-the-day")]
        public IActionResult VoteDrinkOfTheDay(VoteDrinkOfTheDayRequest request)
        {
            return Ok(drinkService.VoteDrinkOfTheDay(request.userId,request.drinkId));
        }

        [HttpPut("update")]
        public IActionResult UpdateDrink([FromBody] UpdateDrinkRequest request)
        {
            drinkService.UpdateDrink(request.Drink);
            return Ok();
        }

        [HttpDelete("delete")]
        public IActionResult DeleteDrink([FromBody] DeleteDrinkRequest request)
        {
            drinkService.DeleteDrink(request.drinkId);
            return Ok();
        }

        [HttpDelete("delete-from-user-drink-list")]
        public IActionResult DeleteFromUserPersonalDrinkList([FromBody] DeleteFromUserPersonalDrinkListRequest request)
        {
            return Ok(drinkService.DeleteFromUserPersonalDrinkList(request.userId, request.drinkId));
        }
    }
}
