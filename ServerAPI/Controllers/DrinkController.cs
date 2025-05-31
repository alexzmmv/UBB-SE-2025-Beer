using DataAccess.Constants;
using DataAccess.DTORequests.Drink;
using DataAccess.Service;
using DataAccess.Service.Interfaces;
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
        private readonly IDrinkService drinkService;
        private readonly IUserService userService;
        private readonly IDrinkModificationRequestService drinkModificationRequestService;

        public DrinkController(IDrinkService drinkService, IUserService userService, IDrinkModificationRequestService drinkModificationRequestService)
        {
            this.drinkService = drinkService;
            this.userService = userService;
            this.drinkModificationRequestService = drinkModificationRequestService;
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
        public async Task<IActionResult> AddDrink([FromBody] AddDrinkRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var user = await userService.GetUserById(request.requestingUserId) ?? throw new Exception("No user with given ID");
            if (user.AssignedRole == RoleType.Admin)
            {
                drinkService.AddDrink(
                    request.inputtedDrinkName,
                    request.inputtedDrinkPath,
                    request.inputtedDrinkCategories,
                    request.inputtedDrinkBrandName,
                    request.inputtedAlcoholPercentage,
                    new Guid(),
                    false);
            }
            else
            {
                var drinkRequestingAddition = drinkService.AddDrink(
                    request.inputtedDrinkName,
                    request.inputtedDrinkPath,
                    request.inputtedDrinkCategories,
                    request.inputtedDrinkBrandName,
                    request.inputtedAlcoholPercentage,
                    new Guid(),
                    true);

                drinkModificationRequestService.AddRequest(DrinkModificationRequestType.Add, null, drinkRequestingAddition.DrinkId, user.UserId);
            }
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
        public async Task<IActionResult> UpdateDrink([FromBody] UpdateDrinkRequest request)
        {
            var user = await userService.GetUserById(request.requestingUserId) ?? throw new Exception("No user with given ID");
            if (user.AssignedRole == RoleType.Admin)
            {
                drinkService.UpdateDrink(request.Drink, new Guid());
            }
            else
            {
                var newDrink = drinkService.AddDrink(
                    request.Drink.DrinkName,
                    request.Drink.DrinkImageUrl,
                    request.Drink.CategoryList,
                    request.Drink.DrinkBrand.BrandName,
                    request.Drink.AlcoholContent,
                    new Guid(),
                    true);
                var oldDrink = drinkService.GetDrinkById(request.Drink.DrinkId);
                if (oldDrink == null)
                {
                    drinkModificationRequestService.AddRequest(DrinkModificationRequestType.Add, null, newDrink.DrinkId, user.UserId);
                }
                else
                {
                    if (oldDrink.IsRequestingApproval)
                    {
                        throw new Exception("Cant update unapproved drink");
                    }
                    drinkModificationRequestService.AddRequest(DrinkModificationRequestType.Edit, request.Drink.DrinkId, newDrink.DrinkId, user.UserId);
                }
            }
            return Ok();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteDrink([FromBody] DeleteDrinkRequest request)
        {
            var requestingUser = await this.userService.GetUserById(request.RequestingUserId);
            if (requestingUser.AssignedRole == RoleType.Admin)
            {
                drinkService.DeleteDrink(request.drinkId, new Guid());
            }
            else
            {
                var drink = this.drinkService.GetDrinkById(request.drinkId) ?? throw new Exception("Drink requested for removal does not exist.");
                if (drink.IsRequestingApproval)
                {
                    throw new Exception("Can't delete unapproved drink");
                }
                this.drinkModificationRequestService.AddRequest(DrinkModificationRequestType.Remove, request.drinkId, null, request.RequestingUserId);
            }
            return Ok();
        }

        [HttpDelete("delete-from-user-drink-list")]
        public IActionResult DeleteFromUserPersonalDrinkList([FromBody] DeleteFromUserPersonalDrinkListRequest request)
        {
            return Ok(drinkService.DeleteFromUserPersonalDrinkList(request.userId, request.drinkId));
        }
    }
}
