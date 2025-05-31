using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Constants;
using DataAccess.Model.AdminDashboard;
using DataAccess.Model.Authentication;
using DataAccess.Service.Interfaces;
using DataAccess.Service.Interfaces;
using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
using DrinkDb_Auth.View;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using WinUiApp.Data.Data;
using WinUIApp;
using Microsoft.UI.Xaml.Navigation;
using WinUIApplication.Views.Components;

namespace DrinkDb_Auth
{
    public sealed partial class ProfilePage : Page
    {
        private readonly IUserService userService;
        private readonly IAuthenticationService authenticationService;
        private readonly IReviewService reviewService;
        private readonly IUpgradeRequestsService upgradeRequestsService;
        private User? currentUser;
        private bool isAdmin;

        public bool IsAdmin => isAdmin;

        public ProfilePage()
        {
            this.InitializeComponent();
            this.Loaded += UserPage_Loaded;
            this.userService = App.Host.Services.GetRequiredService<IUserService>();
            this.authenticationService = App.Host.Services.GetRequiredService<IAuthenticationService>();
            this.reviewService = App.Host.Services.GetRequiredService<IReviewService>();
            this.upgradeRequestsService = App.Host.Services.GetRequiredService<IUpgradeRequestsService>();
            InitializeAdminStatus();

            this.RequestAdminButtonVisibility();
        }

        private async void InitializeAdminStatus()
        {
            isAdmin = await userService.GetHighestRoleTypeForUser(App.CurrentUserId) == RoleType.Admin;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            AuthenticationWindow.CurrentPage = typeof(ProfilePage);
            LoadUserData();
        }

        private void UserPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadUserData();
            LoadUserReviews();
        }

        private async void LoadUserData()
        {
            Guid currentUserId = App.CurrentUserId;

            if (currentUserId != Guid.Empty)
            {
                this.currentUser = await userService.GetUserById(currentUserId);

                if (currentUser != null)
                {
                    NameTextBlock.Text = currentUser.Username;
                    UsernameTextBlock.Text = "@" + currentUser.Username;

                    if (currentUser.AssignedRole == RoleType.Banned && !currentUser.HasSubmittedAppeal)
                    {
                        AppealButton.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        AppealButton.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    NameTextBlock.Text = "User not found";
                    UsernameTextBlock.Text = string.Empty;
                    AppealButton.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                NameTextBlock.Text = "No user logged in";
                UsernameTextBlock.Text = string.Empty;
                AppealButton.Visibility = Visibility.Collapsed;
            }
        }

        private void AdminDashboardButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                AdminPage mainPage = App.Host.Services.GetRequiredService<AdminPage>();
                this.Frame.Navigate(typeof(AdminPage), mainPage);
            }
        }

        private async void LoadUserReviews()
        {
            //Guid currentUserId = App.CurrentUserId;
            //if (currentUserId == Guid.Empty)
            //{
            //    return;
            //}

            //List<Review> userReviews = (await this.reviewService.GetReviewsByUser(currentUserId))
            //    .Where(review => !review.IsHidden)
            //    .OrderByDescending(review => review.CreatedDate)
            //    .ToList();

            //this.ReviewsItemsControl.Items.Clear();
            //foreach (Review review in userReviews)
            //{
            //    Border border = new Border
            //    {
            //        BorderBrush = new SolidColorBrush(Microsoft.UI.Colors.Black),
            //        BorderThickness = new Thickness(1),
            //        CornerRadius = new CornerRadius(8),
            //        Margin = new Thickness(0, 0, 0, 10),
            //        Padding = new Thickness(12)
            //    };
            //    StackPanel reviewStack = new StackPanel { Spacing = 4 };
            //    string stars = new string('★', (int)review.Rating.RatingValue) + new string('☆', 5 - (int)review.Rating.RatingValue);
            //    TextBlock starsText = new TextBlock
            //    {
            //        Text = stars,
            //        FontSize = 20,
            //        Foreground = new SolidColorBrush(Microsoft.UI.Colors.Gold)
            //    };
            //    reviewStack.Children.Add(starsText);
            //    TextBlock dateText = new TextBlock
            //    {
            //        Text = review.CreatedDate.ToShortDateString(),
            //        FontSize = 12,
            //        Foreground = new SolidColorBrush(Microsoft.UI.Colors.Gray)
            //    };
            //    reviewStack.Children.Add(dateText);
            //    TextBlock commentText = new TextBlock
            //    {
            //        Text = review.Content,
            //        FontSize = 14
            //    };
            //    reviewStack.Children.Add(commentText);
            //    border.Child = reviewStack;
            //    this.ReviewsItemsControl.Items.Add(border);
            //}
        }

        private async void AppealButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentUser != null)
            {
                try
                {
                    await userService.UpdateUserAppleaed(currentUser, true);
                    AppealButton.Visibility = Visibility.Collapsed;
                }
                catch (Exception ex)
                {
                    ContentDialog dialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = "Failed to submit appeal. Please try again later.",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    };
                    await dialog.ShowAsync();
                }
            }
        }

        public async void RequestAdminButtonVisibility()
        {
            RoleType? role = await this.userService.GetHighestRoleTypeForUser(App.CurrentUserId);

            if (role == null)
            {
                return;
            }

            if (role != RoleType.User)
            {
                this.RequestAdminButton.Visibility = Visibility.Collapsed;
                return;
            }

            List<UpgradeRequest> requests = await this.upgradeRequestsService.RetrieveAllUpgradeRequests();

            foreach (UpgradeRequest request in requests)
            {
                if (request.RequestingUserIdentifier == App.CurrentUserId)
                {
                    this.RequestAdminButton.Visibility = Visibility.Collapsed;
                    return;
                }
            }
        }

        private async void RequestAdminButton_Click(object sender, RoutedEventArgs e)
        {
            await this.upgradeRequestsService.AddUpgradeRequest(App.CurrentUserId);
            this.RequestAdminButton.Visibility = Visibility.Collapsed;
        }
    }

    public class ReviewModel
    {
        public DateTime Date { get; set; } = DateTime.Now;
        public int Rating { get; set; } = 0;
        public string Comment { get; set; } = string.Empty;
    }
}