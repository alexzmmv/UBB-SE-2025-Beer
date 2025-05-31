using DataAccess.Constants;
using DataAccess.DTOModels;
using DataAccess.Service.Interfaces;
using DrinkDb_Auth;
using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
using MailKit.Net.Imap;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Navigation;
using System;
using Windows.UI.Notifications;
using WinUIApp.ProxyServices;
using WinUIApp.ViewModels;
using WinUIApp.Views.Components.Modals;
using WinUIApp.Views.ViewModels;
using WinUIApp.Views.Windows;

namespace WinUIApp.Views.Pages
{
    public sealed partial class DrinkDetailPage : Page
    {
        private IDrinkService drinkService;
        private IDrinkReviewService drinkReviewService;
        private IUserService userService;
        private IReviewService reviewService;
        private ICheckersService checkersService;
        private bool isAdmin;
        private MenuFlyoutItem flagReviewMenuItem;
        private MenuFlyoutItem hideReviewMenuItem;
        private MenuFlyoutItem aiCheckMenuItem;

        public DrinkDetailPage()
        {
            this.InitializeComponent();

            this.drinkService = App.Host.Services.GetRequiredService<IDrinkService>();
            this.drinkReviewService = App.Host.Services.GetRequiredService<IDrinkReviewService>();
            this.userService = App.Host.Services.GetRequiredService<IUserService>();
            this.reviewService = App.Host.Services.GetRequiredService<IReviewService>();
            this.checkersService = App.Host.Services.GetRequiredService<ICheckersService>();

            // Initialize menu items
            this.flagReviewMenuItem = new MenuFlyoutItem
            {
                Text = "Flag Review",
                Icon = new FontIcon { Glyph = "\uE7E7" }
            };
            this.flagReviewMenuItem.Click += FlagReviewMenuItem_Click;

            this.hideReviewMenuItem = new MenuFlyoutItem
            {
                Text = "Hide Review",
                Icon = new FontIcon { Glyph = "\uE711" }
            };
            this.hideReviewMenuItem.Click += HideReviewMenuItem_Click;

            this.aiCheckMenuItem = new MenuFlyoutItem
            {
                Text = "AI Check",
                Icon = new FontIcon { Glyph = "\uE721" }
            };
            this.aiCheckMenuItem.Click += AICheckMenuItem_Click;

            this.ViewModel = new DrinkDetailPageViewModel(
                this.drinkService,
                this.drinkReviewService,
                this.userService);

            this.DataContext = this.ViewModel;

            this.UpdateRemoveButtonText();

            this.UpdateButton.OnDrinkUpdated = () =>
            {
                this.ViewModel.LoadDrink(this.ViewModel.Drink.DrinkId);
            };

            this.ViewModel.RequestOpenPopup += OpenAddReviewModal;
            this.ViewModel.RequestClosePopup += CloseAddReviewModal;

            this.HideButtonsOnBan();
            InitializeAdminStatus();
        }

        private async void InitializeAdminStatus()
        {
            isAdmin = await userService.GetHighestRoleTypeForUser(App.CurrentUserId) == RoleType.Admin;
            ViewModel.IsAdmin = isAdmin;
        }

        private void FlagReviewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem menuItem && menuItem.DataContext is ReviewDTO review)
            {
                try
                {
                    reviewService.UpdateNumberOfFlagsForReview(review.ReviewId, review.NumberOfFlags + 1);
                    ViewModel.RefreshReviews();
                }
                catch (Exception ex)
                {
                    var dialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = "Failed to flag review: " + ex.Message,
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    };
                    dialog.ShowAsync();
                }
            }
        }

        private void HideReviewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem menuItem && menuItem.DataContext is ReviewDTO review)
            {
                try
                {
                    reviewService.HideReview(review.ReviewId);
                    ViewModel.RefreshReviews();
                }
                catch (Exception ex)
                {
                    var dialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = "Failed to hide review: " + ex.Message,
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    };
                    dialog.ShowAsync();
                }
            }
        }

        private async void AICheckMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem menuItem && menuItem.DataContext is ReviewDTO review)
            {
                //try
                //{
                //    var result = await reviewService.AICheckReview(review.ReviewId);
                //    var dialog = new ContentDialog
                //    {
                //        Title = "AI Check Result",
                //        Content = result,
                //        CloseButtonText = "OK",
                //        XamlRoot = this.XamlRoot
                //    };
                //    await dialog.ShowAsync();
                //}
                //catch (Exception ex)
                //{
                //    var dialog = new ContentDialog
                //    {
                //        Title = "Error",
                //        Content = "Failed to perform AI check: " + ex.Message,
                //        CloseButtonText = "OK",
                //        XamlRoot = this.XamlRoot
                //    };
                //    await dialog.ShowAsync();
                //}
            }
        }

        public DrinkDetailPageViewModel ViewModel { get; }

        public async void HideButtonsOnBan()
        {
            RoleType? role = await this.userService.GetHighestRoleTypeForUser(App.CurrentUserId);

            if (role == null)
            {
                return;
            }

            if (role == RoleType.Banned)
            {
                this.RemoveButton.Visibility = Visibility.Collapsed;
                this.ReviewButton.Visibility = Visibility.Collapsed;
                this.UpdateButton.Visibility = Visibility.Collapsed;
                this.VoteButton.Visibility = Visibility.Collapsed;
            }
        }

        public async void UpdateRemoveButtonText()
        {
            if (await this.ViewModel.IsCurrentUserAdminAsync())
            {
                this.RemoveButtonText.Text = "Remove drink";
            }
            else
            {
                this.RemoveButtonText.Text = "Send remove drink request";
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs eventArguments)
        {
            base.OnNavigatedTo(eventArguments);
            if (eventArguments.Parameter is int drinkId)
            {
                this.ViewModel.LoadDrink(drinkId);
            }

            AuthenticationWindow.PreviousPage = AuthenticationWindow.CurrentPage;
            AuthenticationWindow.CurrentPage = typeof(DrinkDetailPage);
        }

        private void ConfirmRemoveButton_Click(object sender, RoutedEventArgs eventArguments)
        {
            this.ViewModel.RemoveDrinkAsync();
            AuthenticationWindow.NavigationFrame.Navigate(AuthenticationWindow.PreviousPage);
        }

        private void VoteButton_Click(object sender, RoutedEventArgs eventArguments)
        {
            this.ViewModel.VoteForDrink();
        }

        private void OpenAddReviewModal(object sender, EventArgs e)
        {
            AddReviewModalOverlay.Visibility = Visibility.Visible;
        }

        private void CloseAddReviewModal(object sender,EventArgs e)
        {

            AddReviewModalOverlay.Visibility = Visibility.Collapsed;
        }
        private void RefreshReviews(object sender, EventArgs e)
        {
            this.ViewModel.RefreshReviews();
        }
    }
}