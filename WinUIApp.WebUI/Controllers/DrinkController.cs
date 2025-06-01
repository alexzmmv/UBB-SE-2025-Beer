using System.Diagnostics;
using DataAccess.Service;
using DataAccess.Service.Interfaces;
using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WinUiApp.Data.Data;
using WinUIApp.ProxyServices;
using WinUIApp.ProxyServices.Models;
using WinUIApp.WebUI.Models;
using WinUIApp.WebUI.Models;
using System.Text.Json;
using System.Collections.Generic;
using DataAccess.DTOModels;

namespace WinUIApp.WebUI.Controllers
{
    public class DrinkController : Controller
    {
        private readonly IDrinkService drinkService;
        private readonly IReviewService reviewService;
        private readonly IUserService userService;

        public DrinkController(IDrinkService drinkService, IReviewService reviewService, IUserService userService)
        {
            this.drinkService = drinkService;
            this.reviewService = reviewService;
            this.userService = userService;
        }

        public IActionResult DrinkDetail(int id)
        {
            var drink = drinkService.GetDrinkById(id);
            if (drink == null)
            {
                return NotFound();
            }
            
            // Get all reviews for this drink
            var reviews = reviewService.GetReviewsByDrink(id).Result;
            double averageRating = reviews.Count > 0 ? reviews.Average(r => r.RatingValue ?? 0) : 0;
            Guid CurrentUserId = AuthenticationService.GetCurrentUserId();
            bool isInFavorites = drinkService.IsDrinkInUserPersonalList(CurrentUserId, id);

            var viewModel = new DrinkDetailViewModel
            {
                Drink = drink,
                CategoriesDisplay = drink.CategoryList != null 
                    ? string.Join(", ", drink.CategoryList.Select(c => c.CategoryName)) 
                    : string.Empty,
                AverageRatingScore = averageRating,
                Reviews = reviews,
                IsInFavorites = isInFavorites,
                NewReview = new RatingReviewViewModel { DrinkId = id, UserId = CurrentUserId }
            };
            
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddRatingAndReview(RatingReviewViewModel model)
        {
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
                Guid CurrentUserId = AuthenticationService.GetCurrentUserId();
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
                
                Guid userId = AuthenticationService.GetCurrentUserId();
                drinkService.DeleteDrink(id, userId);
                return RedirectToAction("Index", "HomePage");
            }
            catch (Exception ex)
            {
                return RedirectToAction("DrinkDetail", new { id });
            }
        }
    }
}