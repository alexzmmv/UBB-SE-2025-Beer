using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Constants;
using DataAccess.Model.AdminDashboard;
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
using Microsoft.UI.Xaml.Media.Imaging;
using DataAccess.DTOModels;
using WinUIApp.WebAPI.Models;
using Microsoft.UI.Text;
using Microsoft.UI;
using Windows.UI;
using WinUIApp.ViewModels;
using Microsoft.UI.Xaml.Media.Animation;

namespace DrinkDb_Auth
{
    public sealed partial class ProfilePage : Page
    {
        private readonly IUserService userService;
        private readonly IAuthenticationService authenticationService;
        private readonly IReviewService reviewService;
        private readonly IUpgradeRequestsService upgradeRequestsService;
        private readonly IDrinkService drinkService;
        private ProfilePageViewModel profilePageViewModel;
        private User? currentUser;
        private bool isAdmin;
        private bool isDrawerOpen = false;
        private bool reviewsCollapsed = false;
        private bool favoriteDrinksCollapsed = false;

        public bool IsAdmin => isAdmin;

        public ProfilePage()
        {
            this.InitializeComponent();

            this.profilePageViewModel = new ProfilePageViewModel();
            this.DataContext = profilePageViewModel;

            this.Loaded += UserPage_Loaded;

            this.userService = App.Host.Services.GetRequiredService<IUserService>();
            this.authenticationService = App.Host.Services.GetRequiredService<IAuthenticationService>();
            this.reviewService = App.Host.Services.GetRequiredService<IReviewService>();
            this.upgradeRequestsService = App.Host.Services.GetRequiredService<IUpgradeRequestsService>();
            this.drinkService = App.Host.Services.GetRequiredService<IDrinkService>();

            this.InitializeAdminStatus();
            this.RequestAdminButtonVisibility();
        }

        private async void InitializeAdminStatus()
        {
            this.isAdmin = await userService.GetHighestRoleTypeForUser(App.CurrentUserId) == RoleType.Admin;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            AuthenticationWindow.CurrentPage = typeof(ProfilePage);
            this.LoadUserData();
        }

        private void UserPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.LoadUserData();
            this.LoadUserReviews();
            this.SetProfileImage();
        }

        private async void LoadUserData()
        {
            Guid currentUserId = App.CurrentUserId;

            if (currentUserId != Guid.Empty)
            {
                this.currentUser = await this.userService.GetUserById(currentUserId);

                if (currentUser != null)
                {
                    this.NameTextBlock.Text = this.currentUser.Username;
                    this.UsernameTextBlock.Text = "@" + this.currentUser.Username;

                    if (this.currentUser.AssignedRole == RoleType.Banned && !this.currentUser.HasSubmittedAppeal)
                    {
                        this.AppealButton.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        this.AppealButton.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    this.NameTextBlock.Text = "User not found";
                    this.UsernameTextBlock.Text = string.Empty;
                    this.AppealButton.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                this.NameTextBlock.Text = "No user logged in";
                this.UsernameTextBlock.Text = string.Empty;
                this.AppealButton.Visibility = Visibility.Collapsed;
            }
        }

        private async void AppealButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.currentUser != null)
            {
                try
                {
                    await this.userService.UpdateUserAppleaed(this.currentUser, true);
                    this.AppealButton.Visibility = Visibility.Collapsed;
                }
                catch
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

        private void SetProfileImage()
        {
            switch (AuthenticationWindow.OAuthService)
            {
                case DataAccess.Service.OAuthService.None:
                    this.ProfileImageBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/user.png"));
                    break;
                case DataAccess.Service.OAuthService.Google:
                    this.ProfileImageBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/google_logo.png"));
                    break;
                case DataAccess.Service.OAuthService.Facebook:
                    this.ProfileImageBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/facebook_logo.png"));
                    break;
                case DataAccess.Service.OAuthService.Twitter:
                    this.ProfileImageBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/x-twitter-logo-on-black"));
                    break;
                case DataAccess.Service.OAuthService.GitHub:
                    this.ProfileImageBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/github_logo.png"));
                    break;
                case DataAccess.Service.OAuthService.LinkedIn:
                    this.ProfileImageBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/linkedin_logo.png"));
                    break;
            }
        }

        private async void LoadUserReviews()
        {
            Guid currentUserId = App.CurrentUserId;

            if (currentUserId == Guid.Empty)
            {
                return;
            }

            List<ReviewDTO> userReviews = (await this.reviewService.GetReviewsByUser(currentUserId)).Where(review => !review.IsHidden)
                                            .OrderByDescending(review => review.CreatedDate).ToList();

            this.ReviewsItemsControl.Items.Clear();

            foreach (ReviewDTO review in userReviews)
            {
                DrinkDTO? drink = this.drinkService.GetDrinkById(review.DrinkId);
                if (drink == null)
                {
                    continue;
                }

                Border border = new Border
                {
                    BorderBrush = new SolidColorBrush(Color.FromArgb(255, 75, 108, 219)),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(8),
                    Background = new SolidColorBrush(Color.FromArgb(255, 26, 26, 46)),
                    Margin = new Thickness(0, 0, 0, 16),
                    Padding = new Thickness(16)
                };

                StackPanel mainPanel = new StackPanel { Spacing = 12 };
                StackPanel drinkHeader = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 12 };

                Border imageBorder = new Border
                {
                    Width = 60,
                    Height = 60,
                    CornerRadius = new CornerRadius(4),
                    BorderBrush = new SolidColorBrush(Color.FromArgb(255, 75, 205, 219)),
                    BorderThickness = new Thickness(1)
                };

                Image drinkImage = new Image
                {
                    Source = new BitmapImage(new Uri(drink.DrinkImageUrl)),
                    Stretch = Stretch.UniformToFill
                };
                imageBorder.Child = drinkImage;
                drinkHeader.Children.Add(imageBorder);

                StackPanel drinkInfo = new StackPanel { VerticalAlignment = VerticalAlignment.Center };

                TextBlock drinkName = new TextBlock
                {
                    Text = drink.DrinkName,
                    FontSize = 18,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Color.FromArgb(255, 75, 205, 219))
                };
                drinkInfo.Children.Add(drinkName);

                StackPanel categoriesPanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 6 };
                foreach (var category in drink.CategoryList.Take(3))
                {
                    Border categoryBorder = new Border
                    {
                        Background = new SolidColorBrush(Colors.Transparent),
                        BorderBrush = new SolidColorBrush(Color.FromArgb(255, 75, 108, 219)),
                        BorderThickness = new Thickness(1),
                        CornerRadius = new CornerRadius(4),
                        Padding = new Thickness(6, 2, 6, 2)
                    };
                    TextBlock categoryText = new TextBlock
                    {
                        Text = category.CategoryName,
                        FontSize = 12,
                        Foreground = new SolidColorBrush(Colors.White)
                    };
                    categoryBorder.Child = categoryText;
                    categoriesPanel.Children.Add(categoryBorder);
                }
                drinkInfo.Children.Add(categoriesPanel);
                drinkHeader.Children.Add(drinkInfo);
                mainPanel.Children.Add(drinkHeader);

                if (review.RatingValue == null)
                {
                    continue;
                }

                string stars = new string('★', (int)review.RatingValue);
                TextBlock starsText = new TextBlock
                {
                    Text = stars,
                    FontSize = 24,
                    Foreground = new SolidColorBrush(Color.FromArgb(255, 219, 75, 205))
                };
                mainPanel.Children.Add(starsText);

                TextBlock contentText = new TextBlock
                {
                    Text = review.Content,
                    FontSize = 14,
                    Foreground = new SolidColorBrush(Colors.White),
                    TextWrapping = TextWrapping.Wrap
                };
                mainPanel.Children.Add(contentText);

                TextBlock dateText = new TextBlock
                {
                    Text = review.CreatedDate.ToString("MMMM dd, yyyy"),
                    FontSize = 12,
                    Foreground = new SolidColorBrush(Color.FromArgb(255, 160, 160, 160))
                };
                mainPanel.Children.Add(dateText);

                border.Child = mainPanel;
                this.ReviewsItemsControl.Items.Add(border);
            }
        }

        private void DrawerToggleButton_Click(object sender, RoutedEventArgs e)
        {
            this.ToggleDrawer();
        }

        private void ToggleDrawer()
        {
            Storyboard storyboard = new Storyboard();
            DoubleAnimation animation = new DoubleAnimation();

            animation.Duration = TimeSpan.FromMilliseconds(300);
            animation.EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut };

            if (isDrawerOpen)
            {
                animation.To = 1200;
                this.DrawerToggleButton.Content = "<";
            }
            else
            {
                animation.To = 0;
                this.DrawerToggleButton.Content = ">";
            }

            Storyboard.SetTarget(animation, this.DrawerTransform);
            Storyboard.SetTargetProperty(animation, "X");
            storyboard.Children.Add(animation);

            storyboard.Begin();
            this.isDrawerOpen = !isDrawerOpen;
        }

        private void ReviewsToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleSection(ReviewsContent, ReviewsToggleButton, ref reviewsCollapsed);
        }

        private void FavoriteDrinksToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleSection(FavoriteDrinksContent, FavoriteDrinksToggleButton, ref favoriteDrinksCollapsed);
        }


        private void ToggleSection(FrameworkElement content, Button toggleButton, ref bool isCollapsed)
        {
            Storyboard storyboard = new Storyboard();

            DoubleAnimation heightAnimation = new DoubleAnimation
            {
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
            };

            DoubleAnimation opacityAnimation = new DoubleAnimation
            {
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
            };

            if (isCollapsed)
            {
                content.Visibility = Visibility.Visible;
                heightAnimation.From = 0;
                heightAnimation.To = 0;
                opacityAnimation.From = 0;
                opacityAnimation.To = 1;
                toggleButton.Content = "−";

                UpdateToggleButtonStyle(toggleButton, false);
            }
            else
            {
                heightAnimation.From = content.ActualHeight;
                heightAnimation.To = 0;
                opacityAnimation.From = 1;
                opacityAnimation.To = 0;
                toggleButton.Content = "+";

                this.UpdateToggleButtonStyle(toggleButton, true);

                storyboard.Completed += (s, e) =>
                {
                    content.Visibility = Visibility.Collapsed;
                };
            }

            Storyboard.SetTarget(heightAnimation, content);
            Storyboard.SetTargetProperty(heightAnimation, "Height");

            Storyboard.SetTarget(opacityAnimation, content);
            Storyboard.SetTargetProperty(opacityAnimation, "Opacity");

            storyboard.Children.Add(heightAnimation);
            storyboard.Children.Add(opacityAnimation);

            isCollapsed = !isCollapsed;

            storyboard.Begin();
        }

        private void UpdateToggleButtonStyle(Button button, bool isCollapsed)
        {
            ColorAnimation colorAnimation = new ColorAnimation
            {
                Duration = TimeSpan.FromMilliseconds(200),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            if (isCollapsed)
            {
                button.Opacity = 0.7;
                button.BorderThickness = new Thickness(1);
            }
            else
            {
                button.Opacity = 1.0;
                button.BorderThickness = new Thickness(2);
            }
        }

        public List<DrinkDTO> PersonalDrinks { get; set; } = new List<DrinkDTO>();
    }

    public class ReviewModel
    {
        public DateTime Date { get; set; } = DateTime.Now;
        public int Rating { get; set; } = 0;
        public string Comment { get; set; } = string.Empty;
    }
}