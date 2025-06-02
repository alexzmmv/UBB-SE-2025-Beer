using System.Diagnostics;
using DataAccess.AutoChecker;
using DataAccess.Constants;
using DataAccess.Model.AdminDashboard;
using DataAccess.Service.Interfaces;
using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebServer.Models;
using WinUiApp.Data.Data;
using DataAccess.DTOModels;
using WinUIApp.WebAPI.Models;
using DataAccess.Service.Components;
using Newtonsoft.Json;
namespace WebServer.Controllers
{
    public class AdminController : Controller
    {
        private IReviewService reviewService;
        private IUpgradeRequestsService upgradeRequestService;
        private ICheckersService checkersService;
        private IUserService userService;
        private IAutoCheck autoCheckService;
        private IDrinkService drinkService;

        public AdminController(IReviewService newReviewService, IUpgradeRequestsService newUpgradeRequestService, IRolesService newRolesService,
            ICheckersService newCheckersService, IAutoCheck autoCheck, IUserService userService, IDrinkService drinkService)
        {
            this.reviewService = newReviewService;
            this.upgradeRequestService = newUpgradeRequestService;
            this.checkersService = newCheckersService;
            this.autoCheckService = autoCheck;
            this.userService = userService;
            this.drinkService = drinkService;
        }

        public async Task<IActionResult> AdminDashboard()
        {
            IEnumerable<ReviewDTO> reviews = await this.reviewService.GetFlaggedReviews();
            IEnumerable<UpgradeRequest> upgradeRequests = await this.upgradeRequestService.RetrieveAllUpgradeRequests();
            IEnumerable<string> offensiveWords = await this.checkersService.GetOffensiveWordsList();
            List<User> users = await this.userService.GetAllUsers();
            IEnumerable<User> appealeadUsers = users.Where(user => user.HasSubmittedAppeal && user.AssignedRole == RoleType.Banned);

            AdminDashboardViewModel adminDashboardViewModel = new AdminDashboardViewModel()
            {
                Reviews = reviews,
                UpgradeRequests = upgradeRequests,
                OffensiveWords = offensiveWords,
                AppealsList = appealeadUsers
            };

            List<AppealDetailsViewModel> appealsWithDetails = new List<AppealDetailsViewModel>();

            foreach (User user in appealeadUsers)
            {
                List<DataAccess.DTOModels.ReviewDTO> userReviews = await this.reviewService.GetReviewsByUser(user.UserId);
                appealsWithDetails.Add(new AppealDetailsViewModel()
                {
                    User = user,
                    Reviews = userReviews
                });
            }
            adminDashboardViewModel.AppealsWithDetails = appealsWithDetails;

            return View(adminDashboardViewModel);
        }

        public async Task<IActionResult> AcceptReview(int reviewId)
        {
            await this.reviewService.ResetReviewFlags(reviewId);
            return RedirectToAction("AdminDashboard");
        }

        public async Task<IActionResult> HideReview(int reviewId)
        {
            await this.reviewService.HideReview(reviewId);
            return RedirectToAction("AdminDashboard");
        }

        [HttpPost]
        public async Task<IActionResult> AICheckReview(int reviewId)
        {
            try
            {
                ReviewDTO? review = await this.reviewService.GetReviewById(reviewId);
                if (review == null)
                {
                    TempData["ErrorMessage"] = "Review not found. Please try again.";
                    return RedirectToAction("AdminDashboard");
                }

                bool shouldHideReview = false;
                string detectionMessage = "";

                // Step 1: Check against offensive words list
                var offensiveWords = await this.checkersService.GetOffensiveWordsList();
                var containsOffensiveWords = false;
                var foundWord = "";

                // Normalize and split the text
                var normalizedContent = review.Content.ToLowerInvariant();
                var words = normalizedContent.Split(new[] { ' ', ',', '.', '!', '?', ';', ':', '\n', '\r', '\t', '(', ')', '[', ']', '{', '}', '"', '\'', '*', '/', '\\', '<', '>', '|' }, 
                    StringSplitOptions.RemoveEmptyEntries);
                
                foreach (var reviewWord in words)
                {
                    var cleanWord = reviewWord.Trim(new[] { '.', ',', '!', '?', ';', ':', '"', '\'', '*', '/', '\\' });
                    if (offensiveWords.Contains(cleanWord, StringComparer.OrdinalIgnoreCase))
                    {
                        containsOffensiveWords = true;
                        foundWord = cleanWord;
                        shouldHideReview = true;
                        detectionMessage = $"Warning: Review contains offensive word '{foundWord}'!";
                        break;
                    }
                }

                // Step 2: Get associated user and drink data for AI check
                User? user = await this.userService.GetUserById(review.UserId);
                DrinkDTO? drink = this.drinkService.GetDrinkById(review.DrinkId);

                if (user == null || drink == null)
                {
                    TempData["ErrorMessage"] = "Could not load complete review data.";
                    return RedirectToAction("AdminDashboard");
                }

                // Create the review entity for AI check
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
                    User = user,
                    Drink = new Drink 
                    {
                        DrinkId = drink.DrinkId,
                        DrinkName = drink.DrinkName,
                        UserDrinks = new List<UserDrink>(),
                        Votes = new List<Vote>(),
                        DrinkCategories = new List<DrinkCategory>()
                    }
                };

                // Step 3: Run AI Check
                bool isOffensiveByAI = false;
                try
                {
                    Debug.WriteLine($"Sending to AI check: '{review.Content}'");
                    string response = OffensiveTextDetector.DetectOffensiveContent(review.Content);
                    Debug.WriteLine($"AI Response: {response}");

                    if (!string.IsNullOrEmpty(response) && !response.StartsWith("Error:"))
                    {
                        var arrayResults = JsonConvert.DeserializeObject<List<List<Dictionary<string, object>>>>(response);
                        if (arrayResults?.Count > 0 && arrayResults[0]?.Count > 0)
                        {
                            var prediction = arrayResults[0][0];
                            if (prediction != null &&
                                prediction.TryGetValue("label", out object labelObj) &&
                                prediction.TryGetValue("score", out object scoreObj))
                            {
                                string label = labelObj.ToString().ToLower();
                                if (double.TryParse(scoreObj.ToString(), out double score))
                                {
                                    Debug.WriteLine($"AI Label: {label}, Score: {score}");
                                    if (label == "offensive" && score > 0.4) // Lowered threshold for AI detection
                                    {
                                        isOffensiveByAI = true;
                                        shouldHideReview = true;
                                        if (string.IsNullOrEmpty(detectionMessage))
                                        {
                                            detectionMessage = "AI detected offensive content";
                                        }
                                        else
                                        {
                                            detectionMessage += " AI also flagged this content as offensive.";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"AI Check Error: {ex.Message}");
                    // Continue with word-based check results even if AI fails
                }

                // Step 4: Take action based on either check
                if (shouldHideReview)
                {
                    await this.reviewService.HideReview(reviewId);
                    await this.reviewService.ResetReviewFlags(reviewId);
                    TempData[$"ReviewCheck_{reviewId}"] = $"AI Check completed. {detectionMessage} Review has been hidden.";
                }
                else
                {
                    TempData[$"ReviewCheck_{reviewId}"] = $"AI Check completed. No offensive content detected in: '{review.Content}'";
                }

                return RedirectToAction("AdminDashboard");
            }
            catch (Exception exception)
            {
                TempData["ErrorMessage"] = $"AI Check failed: {exception.Message}";
                Debug.WriteLine("Couldn't run AiChecker. Make sure you have your token set correctly:", exception.Message);
                return RedirectToAction("AdminDashboard");
            }
        }

        public async Task<IActionResult> AutomaticallyCheckReviews()
        {
            List<ReviewDTO> reviews = await this.reviewService.GetFlaggedReviews();
            var reviewEntities = reviews.Select(review => new WinUiApp.Data.Data.Review
            {
                ReviewId = review.ReviewId,
                DrinkId = review.DrinkId,
                UserId = review.UserId,
                Content = review.Content,
                RatingValue = review.RatingValue,
                CreatedDate = review.CreatedDate,
                NumberOfFlags = review.NumberOfFlags,
                IsHidden = review.IsHidden
            }).ToList();
            List<string> messages = await Task.Run(() => this.checkersService.RunAutoCheck(reviewEntities));

            return RedirectToAction("AdminDashboard");
        }

        public async Task<IActionResult> Accept(int id)
        {
            await this.upgradeRequestService.ProcessUpgradeRequest(true, id);
            await this.upgradeRequestService.RemoveUpgradeRequestByIdentifier(id);
            return RedirectToAction("AdminDashboard");
        }

        public async Task<IActionResult> Decline(int id)
        {
            await this.upgradeRequestService.ProcessUpgradeRequest(false, id);
            await this.upgradeRequestService.RemoveUpgradeRequestByIdentifier(id);
            return RedirectToAction("AdminDashboard");
        }

        [HttpPost]
        public async Task<IActionResult> AddOffensiveWord(string word)
        {
            if (!string.IsNullOrWhiteSpace(word))
            {
                await this.checkersService.AddOffensiveWordAsync(word);
            }
            return Json(await this.checkersService.GetOffensiveWordsList());
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOffensiveWord(string word)
        {
            if (!string.IsNullOrWhiteSpace(word))
            {
                await this.checkersService.DeleteOffensiveWordAsync(word);
            }
            return Json(await this.checkersService.GetOffensiveWordsList());
        }

        [HttpPost]
        public async Task<IActionResult> AcceptAppeal(Guid userId)
        {
            User? user = await this.userService.GetUserById(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = "User not found. Please try again.";
                return RedirectToAction("AdminDashboard");
            }

            user.AssignedRole = RoleType.User;
            user.HasSubmittedAppeal = false;
            await this.userService.UpdateUser(user);

            return RedirectToAction("AdminDashboard");
        }

        [HttpPost]
        public async Task<IActionResult> KeepBan(Guid userId)
        {
            User? user = await this.userService.GetUserById(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = "User not found. Please try again.";
                return RedirectToAction("AdminDashboard");
            }

            user.HasSubmittedAppeal = false;
            await this.userService.UpdateUser(user);

            return RedirectToAction("AdminDashboard");
        }

        [HttpPost]
        public async Task<IActionResult> CloseAppealCase(Guid userId)
        {
            User? user = await this.userService.GetUserById(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = "User not found. Please try again.";
                return RedirectToAction("AdminDashboard");
            }

            user.HasSubmittedAppeal = false;
            await this.userService.UpdateUser(user);

            return RedirectToAction("AdminDashboard");
        }
    }
}