using DataAccess.AutoChecker;
using DataAccess.Constants;
using DataAccess.DTOModels;
using DataAccess.Extensions;
using DataAccess.Model.AdminDashboard;
using DataAccess.Service.Interfaces;
using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebServer.Models;
using WinUiApp.Data.Data;
using WinUIApp.WebAPI.Models;

namespace WebServer.Controllers
{
    public class AdminController : Controller
    {
        private IReviewService reviewService;
        private IUpgradeRequestsService upgradeRequestService;
        private ICheckersService checkersService;
        private IUserService userService;
        private IAutoCheck autoCheckService;
        private IDrinkModificationRequestService drinkModificationRequestService;
        private IDrinkService drinkService;
        public AdminController(IReviewService newReviewService, IUpgradeRequestsService newUpgradeRequestService, IRolesService newRolesService,
            ICheckersService newCheckersService, IAutoCheck autoCheck, IUserService userService, IDrinkModificationRequestService drinkModificationRequestService, IDrinkService drinkService)
        {
            this.reviewService = newReviewService;
            this.upgradeRequestService = newUpgradeRequestService;
            this.checkersService = newCheckersService;
            this.autoCheckService = autoCheck;
            this.userService = userService;
            this.drinkModificationRequestService = drinkModificationRequestService;
            this.drinkService = drinkService;
        }

        public async Task<IActionResult> AdminDashboard(string? search)
        {
            Guid currentUserId = Guid.Parse(HttpContext.Session.GetString("UserId") ?? Guid.Empty.ToString());
            if (currentUserId == Guid.Empty)
                return RedirectToAction("AuthenticationPage", "Auth");

            IEnumerable<ReviewDTO> reviews = await this.reviewService.GetFlaggedReviews();

            if (!string.IsNullOrWhiteSpace(search))
            {
                reviews = reviews.Where(r => r.Content.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            IEnumerable<UpgradeRequest> upgradeRequests = await this.upgradeRequestService.RetrieveAllUpgradeRequests();
            IEnumerable<string> offensiveWords = await this.checkersService.GetOffensiveWordsList();
            List<User> users = await this.userService.GetAllUsers();
            IEnumerable<User> appealeadUsers = users.Where(user => user.HasSubmittedAppeal && user.AssignedRole == RoleType.Banned);
            IEnumerable<DrinkModificationRequestDTO> drinkModificationRequests = await this.drinkModificationRequestService.GetAllModificationRequests();
            IEnumerable<DrinkDTO> drinks = this.drinkService.GetDrinks(string.Empty, new List<string>(), new List<string>(), 0, 100, new Dictionary<string, bool>());

            AdminDashboardViewModel adminDashboardViewModel = new AdminDashboardViewModel()
            {
                Reviews = reviews,
                UpgradeRequests = upgradeRequests,
                OffensiveWords = offensiveWords,
                AppealsList = appealeadUsers,
                DrinkModificationRequests = drinkModificationRequests,
                Drinks = drinks
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

        public async Task<IActionResult> AICheckReview(int reviewId)
        {
            try
            {
                ReviewDTO? review = await this.reviewService.GetReviewById(reviewId);

                if (review == null)
                {
                    ViewBag.ErrorMessage = "Review not found. Please try again.";
                    return RedirectToAction("AdminDashboard");
                }
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
                // Map ReviewDTO to Review entity for AI check


                this.checkersService.RunAICheckForOneReviewAsync(reviewEntity);
            }
            catch (Exception exception)
            {
                Debug.WriteLine("Couldn't run AiChecker. Make sure you have your token set correctly:", exception.Message);
            }
            return RedirectToAction("AdminDashboard");
        }
        public async Task PrepareReviewForCheck(Review review)
        {
            User? user = await this.userService.GetUserById(review.UserId);
            DrinkDTO? drink = this.drinkService.GetDrinkById(review.DrinkId);

            if (user == null || drink == null)
            {
                return;
            }

            Drink regularDrink = DrinkExtensions.ConvertDTOToEntity(drink);
            regularDrink.UserDrinks = new List<UserDrink>();
            regularDrink.Votes = new List<Vote>();
            regularDrink.DrinkCategories = new List<DrinkCategory>();
            review.User = user;
            review.Drink = regularDrink;
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
        [HttpPost]
        public async Task<IActionResult> AcceptDrinkModification(int drinkModificationRequestId, Guid userId)
        {
            await drinkModificationRequestService.ApproveRequest(drinkModificationRequestId, userId);
            return RedirectToAction("AdminDashboard");
        }
        [HttpPost]
        public async Task<IActionResult> DenyDrinkModification(int drinkModificationRequestId, Guid userId)
        {
            await drinkModificationRequestService.DenyRequest(drinkModificationRequestId, userId);
            return RedirectToAction("AdminDashboard");
        }
    }
}