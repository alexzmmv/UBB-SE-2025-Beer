using DataAccess.Constants;
using DataAccess.DTORequests.Drink;
using DataAccess.Service;
using DataAccess.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WinUiApp.Data.Data;
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
                    request.SearchKeyword,
                    request.DrinkBrandNameFilter,
                    request.DrinkCategoryFilter,
                    request.MinimumAlcoholPercentage,
                    request.MaximumAlcoholPercentage,
                    request.OrderingCriteria));
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
        public IActionResult GetDrinkCategories()
        {
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
            return Ok(drinkService.GetUserPersonalDrinkList(request.UserId));
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddDrink([FromBody] AddDrinkRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            User? user = await userService.GetUserById(request.RequestingUserId) ?? throw new Exception("No user with given ID");
            if (user.AssignedRole == RoleType.Admin)
            {
                drinkService.AddDrink(
                    request.InputtedDrinkName,
                    request.InputtedDrinkPath,
                    request.InputtedDrinkCategories,
                    request.InputtedDrinkBrandName,
                    request.InputtedAlcoholPercentage,
                    new Guid(),
                    false);
            }
            else
            {
                DrinkDTO drinkRequestingAddition = drinkService.AddDrink(
                    request.InputtedDrinkName,
                    request.InputtedDrinkPath,
                    request.InputtedDrinkCategories,
                    request.InputtedDrinkBrandName,
                    request.InputtedAlcoholPercentage,
                    new Guid(),
                    true);

                drinkModificationRequestService.AddRequest(DrinkModificationRequestType.Add, null, drinkRequestingAddition.DrinkId, user.UserId);
            }
            return Ok();
        }

        [HttpPost("add-to-user-drink-list")]
        public IActionResult AddToUserPersonalDrinkList([FromBody] AddToUserPersonalDrinkListRequest request)
        {
            return Ok(drinkService.AddToUserPersonalDrinkList(request.UserId, request.DrinkId));
        }

        [HttpPost("vote-drink-of-the-day")]
        public IActionResult VoteDrinkOfTheDay(VoteDrinkOfTheDayRequest request)
        {
            return Ok(drinkService.VoteDrinkOfTheDay(request.UserId, request.DrinkId));
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateDrink([FromBody] UpdateDrinkRequest request)
        {
            User user = await userService.GetUserById(request.RequestingUserId) ?? throw new Exception("No user with given ID");
            if (user.AssignedRole == RoleType.Admin)
            {
                drinkService.UpdateDrink(request.Drink, new Guid());
            }
            else
            {
                DrinkDTO? oldDrink = drinkService.GetDrinkById(request.Drink.DrinkId);
                if (oldDrink == null)
                {
                    DrinkDTO newDrink = drinkService.AddDrink(
                        request.Drink.DrinkName,
                        request.Drink.DrinkImageUrl,
                        request.Drink.CategoryList,
                        request.Drink.DrinkBrand.BrandName,
                        request.Drink.AlcoholContent,
                        new Guid(),
                        true);
                    drinkModificationRequestService.AddRequest(DrinkModificationRequestType.Add, null, newDrink.DrinkId, user.UserId);
                }
                else
                {
                    if (oldDrink.IsRequestingApproval)
                    {
                        throw new Exception("Cant update unapproved drink");
                    }
                    DrinkDTO newDrink = drinkService.AddDrink(
                        request.Drink.DrinkName,
                        request.Drink.DrinkImageUrl,
                        request.Drink.CategoryList,
                        request.Drink.DrinkBrand.BrandName,
                        request.Drink.AlcoholContent,
                        new Guid(),
                        true);
                    drinkModificationRequestService.AddRequest(DrinkModificationRequestType.Edit, request.Drink.DrinkId, newDrink.DrinkId, user.UserId);
                }
            }
            return Ok();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteDrink([FromBody] DeleteDrinkRequest request)
        {
            User? requestingUser = await this.userService.GetUserById(request.RequestingUserId);
            if (requestingUser.AssignedRole == RoleType.Admin)
            {
                drinkService.DeleteDrink(request.DrinkId, new Guid());
            }
            else
            {
                DrinkDTO drink = this.drinkService.GetDrinkById(request.DrinkId) ?? throw new Exception("Drink requested for removal does not exist.");
                if (drink.IsRequestingApproval)
                {
                    throw new Exception("Can't delete unapproved drink");
                }
                this.drinkModificationRequestService.AddRequest(DrinkModificationRequestType.Remove, request.DrinkId, null, request.RequestingUserId);
            }
            return Ok();
        }

        [HttpDelete("delete-from-user-drink-list")]
        public IActionResult DeleteFromUserPersonalDrinkList([FromBody] DeleteFromUserPersonalDrinkListRequest request)
        {
            return Ok(drinkService.DeleteFromUserPersonalDrinkList(request.UserId, request.DrinkId));
        }
    }
}
