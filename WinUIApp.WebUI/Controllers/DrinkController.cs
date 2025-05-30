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
                var allReviews = reviewService.GetAllReviews().Result;
                int newReviewId = allReviews.Any() ? allReviews.Max(r => r.ReviewId) + 1 : 1;
                var user = userService.GetUserById(model.UserId).Result;
                var drinkDto = drinkService.GetDrinkById(model.DrinkId);
                var drink = new Drink
                {
                    DrinkId = drinkDto.DrinkId,
                    DrinkName = drinkDto.DrinkName ?? string.Empty,
                    DrinkURL = drinkDto.DrinkImageUrl ?? string.Empty,
                    BrandId = drinkDto.DrinkBrand.BrandId,
                    AlcoholContent = (decimal)drinkDto.AlcoholContent,
                    Brand = new Brand
                    {
                        BrandId = drinkDto.DrinkBrand.BrandId,
                        BrandName = drinkDto.DrinkBrand.BrandName
                    },
                    DrinkCategories = new List<DrinkCategory>(),
                    Votes = new List<Vote>(),
                    UserDrinks = new List<UserDrink>()
                };
                var review = new Review
                {
                    ReviewId = newReviewId,
                    DrinkId = model.DrinkId,
                    UserId = model.UserId,
                    Content = model.ReviewContent ?? string.Empty,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    IsHidden = false,
                    NumberOfFlags = 0,
                    RatingValue = (float?)model.Score,
                    User = user,
                    Drink = drink
                };
                var debugReviewJson = new {
                    reviewId = review.ReviewId,
                    drinkId = review.DrinkId,
                    userId = review.UserId,
                    content = review.Content,
                    isActive = review.IsActive,
                    user = new {
                        userId = user.UserId,
                        username = user.Username,
                        passwordHash = user.PasswordHash,
                        twoFASecret = user.TwoFASecret,
                        emailAddress = user.EmailAddress,
                        numberOfDeletedReviews = user.NumberOfDeletedReviews,
                        hasSubmittedAppeal = user.HasSubmittedAppeal,
                        assignedRole = user.AssignedRole,
                        votes = user.Votes?.Select(v => new {
                            voteId = v.VoteId,
                            userId = v.UserId,
                            drinkId = v.DrinkId,
                            voteTime = v.VoteTime,
                            user = v.User?.Username ?? "string",
                            drink = v.Drink != null ? new {
                                drinkId = v.Drink.DrinkId,
                                drinkName = v.Drink.DrinkName,
                                drinkURL = v.Drink.DrinkURL,
                                brandId = v.Drink.BrandId ?? 0,
                                alcoholContent = (int)v.Drink.AlcoholContent,
                                brand = v.Drink.Brand != null ? new {
                                    brandId = v.Drink.Brand.BrandId,
                                    brandName = v.Drink.Brand.BrandName
                                } : new { brandId = 0, brandName = "string" },
                                drinkCategories = v.Drink.DrinkCategories?.Select(dc => new {
                                    drinkId = dc.DrinkId,
                                    categoryId = dc.CategoryId,
                                    drink = dc.Drink?.DrinkName ?? "string",
                                    category = dc.Category != null ? new {
                                        categoryId = dc.Category.CategoryId,
                                        categoryName = dc.Category.CategoryName,
                                        drinkCategories = new[] { "string" }
                                    } : new { categoryId = 0, categoryName = "string", drinkCategories = new[] { "string" } }
                                }).ToArray() ?? new[] { new { drinkId = 0, categoryId = 0, drink = "string", category = new { categoryId = 0, categoryName = "string", drinkCategories = new[] { "string" } } } },
                                votes = new[] { "string" },
                                userDrinks = new[] { "string" }
                            } : new { drinkId = 0, drinkName = "string", drinkURL = "string", brandId = 0, alcoholContent = 0, brand = new { brandId = 0, brandName = "string" }, drinkCategories = new[] { new { drinkId = 0, categoryId = 0, drink = "string", category = new { categoryId = 0, categoryName = "string", drinkCategories = new[] { "string" } } } }, votes = new[] { "string" }, userDrinks = new[] { "string" } }
                        }).ToArray() ?? new[] { new { voteId = 0, userId = user.UserId, drinkId = 0, voteTime = DateTime.UtcNow, user = "string", drink = new { drinkId = 0, drinkName = "string", drinkURL = "string", brandId = 0, alcoholContent = 0, brand = new { brandId = 0, brandName = "string" }, drinkCategories = new[] { new { drinkId = 0, categoryId = 0, drink = "string", category = new { categoryId = 0, categoryName = "string", drinkCategories = new[] { "string" } } } }, votes = new[] { "string" }, userDrinks = new[] { "string" } } } },
                        userDrinks = user.UserDrinks?.Select(ud => new {
                            userId = ud.UserId,
                            drinkId = ud.DrinkId,
                            user = ud.User?.Username ?? "string",
                            drink = ud.Drink != null ? new {
                                drinkId = ud.Drink.DrinkId,
                                drinkName = ud.Drink.DrinkName,
                                drinkURL = ud.Drink.DrinkURL,
                                brandId = ud.Drink.BrandId ?? 0,
                                alcoholContent = (int)ud.Drink.AlcoholContent,
                                brand = ud.Drink.Brand != null ? new {
                                    brandId = ud.Drink.Brand.BrandId,
                                    brandName = ud.Drink.Brand.BrandName
                                } : new { brandId = 0, brandName = "string" },
                                drinkCategories = ud.Drink.DrinkCategories?.Select(dc => new {
                                    drinkId = dc.DrinkId,
                                    categoryId = dc.CategoryId,
                                    drink = dc.Drink?.DrinkName ?? "string",
                                    category = dc.Category != null ? new {
                                        categoryId = dc.Category.CategoryId,
                                        categoryName = dc.Category.CategoryName,
                                        drinkCategories = new[] { "string" }
                                    } : new { categoryId = 0, categoryName = "string", drinkCategories = new[] { "string" } }
                                }).ToArray() ?? new[] { new { drinkId = 0, categoryId = 0, drink = "string", category = new { categoryId = 0, categoryName = "string", drinkCategories = new[] { "string" } } } },
                                votes = new[] { "string" },
                                userDrinks = new[] { "string" }
                            } : new { drinkId = 0, drinkName = "string", drinkURL = "string", brandId = 0, alcoholContent = 0, brand = new { brandId = 0, brandName = "string" }, drinkCategories = new[] { new { drinkId = 0, categoryId = 0, drink = "string", category = new { categoryId = 0, categoryName = "string", drinkCategories = new[] { "string" } } } }, votes = new[] { "string" }, userDrinks = new[] { "string" } }
                        }).ToArray() ?? new[] { new { userId = user.UserId, drinkId = 0, user = "string", drink = new { drinkId = 0, drinkName = "string", drinkURL = "string", brandId = 0, alcoholContent = 0, brand = new { brandId = 0, brandName = "string" }, drinkCategories = new[] { new { drinkId = 0, categoryId = 0, drink = "string", category = new { categoryId = 0, categoryName = "string", drinkCategories = new[] { "string" } } } }, votes = new[] { "string" }, userDrinks = new[] { "string" } } } }
                    },
                    drink = new {
                        drinkId = drink.DrinkId,
                        drinkName = drink.DrinkName,
                        drinkURL = drink.DrinkURL,
                        brandId = drink.BrandId ?? 0,
                        alcoholContent = (int)drink.AlcoholContent,
                        brand = drink.Brand != null ? new { brandId = drink.Brand.BrandId, brandName = drink.Brand.BrandName } : new { brandId = 0, brandName = "string" },
                        drinkCategories = drink.DrinkCategories?.Select(dc => new {
                            drinkId = dc.DrinkId,
                            categoryId = dc.CategoryId,
                            drink = dc.Drink?.DrinkName ?? "string",
                            category = dc.Category != null ? new {
                                categoryId = dc.Category.CategoryId,
                                categoryName = dc.Category.CategoryName,
                                drinkCategories = new[] { "string" }
                            } : new { categoryId = 0, categoryName = "string", drinkCategories = new[] { "string" } }
                        }).ToArray() ?? new[] { new { drinkId = 0, categoryId = 0, drink = "string", category = new { categoryId = 0, categoryName = "string", drinkCategories = new[] { "string" } } } },
                        votes = new[] { "string" },
                        userDrinks = new[] { "string" }
                    },
                    createdDate = review.CreatedDate,
                    numberOfFlags = review.NumberOfFlags,
                    isHidden = review.IsHidden,
                    ratingValue = review.RatingValue
                };
                TempData["DebugReviewJson"] = JsonSerializer.Serialize(debugReviewJson, new JsonSerializerOptions { WriteIndented = true });

                reviewService.AddReview(review).GetAwaiter().GetResult();
                TempData["SuccessMessage"] = "Your review has been submitted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"There was an error submitting your review. Please try again. {ex.Message} | DrinkId: {model.DrinkId}, UserId: {model.UserId}, Score: {model.Score}, ReviewContent: {model.ReviewContent}";
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
                drinkService.DeleteDrink(id);
                return RedirectToAction("Index", "HomePage");
            }
            catch (Exception ex)
            {
                return RedirectToAction("DrinkDetail", new { id });
            }
        }
    }
}