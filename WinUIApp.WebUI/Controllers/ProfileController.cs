using DataAccess.Constants;
using DataAccess.Model.AdminDashboard;
using DataAccess.Model.Authentication;
using DataAccess.Service;
using DataAccess.Service.Interfaces;
using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebServer.Models;
using WinUiApp.Data.Data;

namespace WebServer.Controllers
{
    public class ProfileController : Controller
    {
        private IUserService userService;
        private IReviewService reviewService;
        private IUpgradeRequestsService upgradeRequestsService;
        private IDrinkService drinkService;
        public ProfileController(IUserService userService, IReviewService reviewService, IDrinkService drinkService, IUpgradeRequestsService upgradeRequestsService)
        {
            this.userService = userService;
            this.reviewService = reviewService;
            this.drinkService = drinkService;
            this.upgradeRequestsService = upgradeRequestsService;
        }

        public async Task<IActionResult> UserPage()
        {
            // TO DO: The SET / GET approach for user / session id will probably not work with multiple browser connections
            Guid userId = AuthenticationService.GetCurrentUserId();
            User? currentUser = await this.userService.GetUserById(userId);

            if (currentUser == null)
            {
                ViewBag.ErrorMessage = "User not found. Please log in again.";
                return RedirectToAction("AuthenticationPage", "Auth");
            }

            IEnumerable<DataAccess.DTOModels.ReviewDTO> reviews = await reviewService.GetReviewsByUser(currentUser.UserId);
            bool hasPendingUpgradeRequest = false;

            if (currentUser.AssignedRole == RoleType.User)
            {
                hasPendingUpgradeRequest = await upgradeRequestsService.HasPendingUpgradeRequest(currentUser.UserId);
            }

            var favoriteDrinks = drinkService.GetUserPersonalDrinkList(userId);
            UserPageModel userPageModel = new UserPageModel()
            {
                CurrentUser = currentUser,
                CurrentUserReviews = reviews,
                FavoriteDrinks = favoriteDrinks.ToList(),
                HasPendingUpgradeRequest = hasPendingUpgradeRequest
            };
            return View(userPageModel);
        }

        [HttpGet]
        public async Task<IActionResult> SubmitAppeal()
        {
            Guid userId = AuthenticationService.GetCurrentUserId();
            User? currentUser = await this.userService.GetUserById(userId);

            if (currentUser == null)
            {
                return RedirectToAction("AuthenticationPage", "Auth");
            }

            if (currentUser.AssignedRole != RoleType.Banned)
            {
                TempData["ErrorMessage"] = "Only banned users can submit appeals.";
                return RedirectToAction("UserPage");
            }

            if (currentUser.HasSubmittedAppeal)
            {
                TempData["InfoMessage"] = "You have already submitted an appeal.";
                return RedirectToAction("UserPage");
            }

            currentUser.HasSubmittedAppeal = true;
            await this.userService.UpdateUser(currentUser);

            TempData["SuccessMessage"] = "Your appeal has been submitted successfully.";
            return RedirectToAction("UserPage");
        }

        [HttpGet]
        public async Task<IActionResult> RequestRoleUpgrade()
        {
            Guid userId = AuthenticationService.GetCurrentUserId();
            User? currentUser = await this.userService.GetUserById(userId);

            if (currentUser == null)
            {
                return RedirectToAction("AuthenticationPage", "Auth");
            }

            if (currentUser.AssignedRole != RoleType.User)
            {
                TempData["ErrorMessage"] = "Only regular users can request role upgrades.";
                return RedirectToAction("UserPage");
            }

            bool hasPendingRequest = await upgradeRequestsService.HasPendingUpgradeRequest(userId);
            if (hasPendingRequest)
            {
                TempData["InfoMessage"] = "You already have a pending upgrade request.";
                return RedirectToAction("UserPage");
            }

            try
            {
                await this.upgradeRequestsService.AddUpgradeRequest(userId);
                TempData["SuccessMessage"] = "Your role upgrade request has been submitted successfully.";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Failed to submit upgrade request. Please try again.";
                
            }

            return RedirectToAction("UserPage");
        }

        public IActionResult LogOut()
        {
            return RedirectToAction("AuthenticationPage", "Auth");
        }
    }
}