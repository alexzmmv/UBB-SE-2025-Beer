using DataAccess.Constants;
using DataAccess.Model.AdminDashboard;
using DataAccess.Model.Authentication;
using DataAccess.Service;
using DataAccess.Service.Interfaces;
using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebServer.Models;
using WinUiApp.Data.Data;
using WinUiApp.Data;

namespace WebServer.Controllers
{
    public class ProfileController : Controller
    {
        private IUserService userService;
        private IReviewService reviewService;
        private IUpgradeRequestsService upgradeRequestsService;
        private readonly AppDbContext context;

        public ProfileController(IUserService userService, IReviewService reviewService, IUpgradeRequestsService upgradeRequestsService, AppDbContext context)
        {
            this.userService = userService;
            this.reviewService = reviewService;
            this.upgradeRequestsService = upgradeRequestsService;
            this.context = context;
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

            IEnumerable<Review> reviews = await reviewService.GetReviewsByUser(currentUser.UserId);
            bool hasPendingUpgradeRequest = false;

            if (currentUser.AssignedRole == RoleType.User)
            {
                hasPendingUpgradeRequest = await HasPendingUpgradeRequest(currentUser.UserId);
            }

            UserPageModel userPageModel = new UserPageModel()
            {
                CurrentUser = currentUser,
                CurrentUserReviews = reviews,
                CurrentUserDrinks = new List<string>() { "beer", "lemonade", "vodka" },
                HasPendingUpgradeRequest = hasPendingUpgradeRequest
            };
            return View(userPageModel);
        }

        private async Task<bool> HasPendingUpgradeRequest(Guid userId)
        {
            // Check if user has any pending upgrade requests
            return await context.UpgradeRequests
                .AnyAsync(ur => ur.RequestingUserIdentifier == userId);
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

            bool hasPendingRequest = await HasPendingUpgradeRequest(userId);
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
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to submit upgrade request. Please try again.";
                // Log the exception
            }

            return RedirectToAction("UserPage");
        }

        public IActionResult LogOut()
        {
            return RedirectToAction("AuthenticationPage", "Auth");
        }
    }
}