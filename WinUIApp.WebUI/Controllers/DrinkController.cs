using DataAccess.DTOModels;
using DataAccess.Extensions;
using DataAccess.Service;
using DataAccess.Service.Interfaces;
using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using WinUiApp.Data.Data;
using WinUIApp.ProxyServices;
using WinUIApp.ProxyServices.Models;
using WinUIApp.WebAPI.Models;
using WinUIApp.WebUI.Models;

namespace WinUIApp.WebUI.Controllers
{
    public class DrinkController : Controller
    {
        private readonly IDrinkService drinkService;
        private readonly IReviewService reviewService;
        private readonly IUserService userService;
        private readonly ICheckersService checkersService;

        public DrinkController(IDrinkService drinkService, IReviewService reviewService, IUserService userService, ICheckersService checkersService)
        {
            this.drinkService = drinkService;
            this.reviewService = reviewService;
            this.userService = userService;
            this.checkersService = checkersService;
        }

        public async Task<IActionResult> DrinkDetail(int id)
        {

            var drink = drinkService.GetDrinkById(id);
            if (drink == null)
            {
                return NotFound();
            }

            // Get all reviews for this drink
            var reviews = await reviewService.GetReviewsWithUserInfoByDrink(id);
            reviews= reviews.Where(review=>review.IsHidden==false).ToList();
            double averageRating = reviews.Count > 0 ? reviews.Average(r => r.RatingValue ?? 0) : 0;
            Guid CurrentUserId = Guid.Parse(HttpContext.Session.GetString("UserId") ?? Guid.Empty.ToString());
            if (CurrentUserId == Guid.Empty)
                return RedirectToAction("AuthenticationPage", "Auth");
            bool isInFavorites = drinkService.IsDrinkInUserPersonalList(CurrentUserId, id);

            var viewModel = new DrinkDetailViewModel
            {
                UserRole = userService.GetUserById(CurrentUserId).Result.AssignedRole,
                Drink = drink,
                CategoriesDisplay = drink.CategoryList != null
                    ? string.Join(", ", drink.CategoryList.Select(c => c.CategoryName))
                    : string.Empty,
                AverageRatingScore = averageRating,
                Reviews = reviews.ToList(),
                IsInFavorites = isInFavorites,
                NewReview = new RatingReviewViewModel { DrinkId = id, UserId = CurrentUserId }
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddRatingAndReview(RatingReviewViewModel model)
        {
            Guid CurrentUserId = Guid.Parse(HttpContext.Session.GetString("UserId") ?? Guid.Empty.ToString());
            if (CurrentUserId == Guid.Empty)
                return RedirectToAction("AuthenticationPage", "Auth");
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fill in all required fields.";
                return RedirectToAction("DrinkDetail", new { id = model.DrinkId });
            }

            try
            {
                // Check if the user already has a review for this drink
                var existingReview = reviewService.GetReviewsByDrink(model.DrinkId)
                    .Result.FirstOrDefault(r => r.UserId == model.UserId);
                if (existingReview != null)
                {
                    TempData["ErrorMessage"] = "You have already rated this drink. You can only rate a drink once.";
                    return RedirectToAction("DrinkDetail", new { id = model.DrinkId });
                }

                var reviewDto = new ReviewDTO
                {
                    ReviewId = 0, // Let the DB assign the ID
                    DrinkId = model.DrinkId,
                    UserId = model.UserId,
                    Content = model.ReviewContent ?? string.Empty,
                    RatingValue = (float?)model.Score ?? 0,
                    CreatedDate = DateTime.UtcNow,
                    NumberOfFlags = 0,
                    IsHidden = false
                };

                // // For debugging: store the review as JSON
                // var debugReviewJson = new {
                //     reviewId = reviewDto.ReviewId,
                //     drinkId = reviewDto.DrinkId,
                //     userId = reviewDto.UserId,
                //     content = reviewDto.Content,
                //     ratingValue = reviewDto.RatingValue,
                //     createdDate = reviewDto.CreatedDate,
                //     numberOfFlags = reviewDto.NumberOfFlags,
                //     isHidden = reviewDto.IsHidden
                // };
                // TempData["DebugReviewJson"] = System.Text.Json.JsonSerializer.Serialize(debugReviewJson, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });

                reviewService.AddReview(reviewDto).GetAwaiter().GetResult();
                TempData["SuccessMessage"] = "Your review has been submitted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"There was an error submitting your review. Please try again. {ex.Message}";
            }

            return RedirectToAction("DrinkDetail", new { id = model.DrinkId });
        }

        [HttpPost]
        public IActionResult ToggleFavorites(int id)
        {
            try
            {
                Guid CurrentUserId = Guid.Parse(HttpContext.Session.GetString("UserId") ?? Guid.Empty.ToString());
                if (CurrentUserId == Guid.Empty)
                    return RedirectToAction("AuthenticationPage", "Auth");
                bool isInFavorites = drinkService.IsDrinkInUserPersonalList(CurrentUserId, id);

                if (isInFavorites)
                {
                    drinkService.DeleteFromUserPersonalDrinkList(CurrentUserId, id);
                    TempData["SuccessMessage"] = "Drink removed from favorites.";
                }
                else
                {
                    drinkService.AddToUserPersonalDrinkList(CurrentUserId, id);
                    TempData["SuccessMessage"] = "Drink added to favorites!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error updating favorites.";
            }

            return RedirectToAction("DrinkDetail", new { id });
        }

        [HttpPost]
        public IActionResult RemoveDrink(int id)
        {
            try
            {
                Guid currentUserId = Guid.Parse(HttpContext.Session.GetString("UserId") ?? Guid.Empty.ToString());
                if (currentUserId == Guid.Empty)
                    return RedirectToAction("AuthenticationPage", "Auth");
                drinkService.DeleteDrink(id, currentUserId);
                return RedirectToAction("Index", "HomePage");
            }
            catch (Exception ex)
            {
                return RedirectToAction("DrinkDetail", new { id });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AICheckReview(int reviewId)
        {    
            ReviewDTO? review = await this.reviewService.GetReviewById(reviewId);

            try
            {

                User? user = await this.userService.GetUserById(review.UserId);
                DrinkDTO? drink = this.drinkService.GetDrinkById(review.DrinkId);


                Drink regularDrink = DrinkExtensions.ConvertDTOToEntity(drink);
                regularDrink.UserDrinks = new List<UserDrink>();
                regularDrink.Votes = new List<Vote>();
                regularDrink.DrinkCategories = new List<DrinkCategory>();
                var reviewEntity = new WinUiApp.Data.Data.Review
                {
                    ReviewId = review.ReviewId,
                    DrinkId = review.DrinkId,
                    UserId = review.UserId,
                    Content = review.Content,
                    RatingValue = review.RatingValue,
                    CreatedDate = review.CreatedDate,
                    NumberOfFlags = review.NumberOfFlags,
                    IsHidden = review.IsHidden,
                    Drink = regularDrink,
                    User = user
                };

                this.checkersService.RunAICheckForOneReviewAsync(reviewEntity);
            }
            catch (Exception exception)
            {
                Debug.WriteLine("Couldn't run AiChecker. Make sure you have your token set correctly:", exception.Message);
            }         
            return RedirectToAction("DrinkDetail", new { id = review.DrinkId });
        }
        public async Task<IActionResult> HideReview(int reviewId)
        {
            ReviewDTO? review = await this.reviewService.GetReviewById(reviewId);
            int drinkId = review.DrinkId;
            await this.reviewService.HideReview(reviewId);
            return RedirectToAction("DrinkDetail", new { id = review.DrinkId });
        }

        public async Task<IActionResult> ReportReview(int reviewId)
        {
            ReviewDTO? review = await this.reviewService.GetReviewById(reviewId);

            await this.reviewService.UpdateNumberOfFlagsForReview(reviewId, review.NumberOfFlags+1);
            return RedirectToAction("DrinkDetail", new { id = review.DrinkId });
        }
    }
}