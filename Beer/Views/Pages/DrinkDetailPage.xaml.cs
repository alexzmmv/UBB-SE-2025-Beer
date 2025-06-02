using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Constants;
using DataAccess.DTOModels;
using DataAccess.Extensions;
using DataAccess.Service.Interfaces;
using DrinkDb_Auth;
using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using WinUiApp.Data.Data;
using WinUIApp.ProxyServices;
using WinUIApp.Views.ViewModels;
using WinUIApp.WebAPI.Models;

namespace WinUIApp.Views.Pages
{
    public sealed partial class DrinkDetailPage : Page, INotifyPropertyChanged
    {
        private IDrinkService drinkService;
        private IDrinkReviewService drinkReviewService;
        private IUserService userService;
        private IReviewService reviewService;
        private IDrinkModificationRequestService modificationRequestService;
        private ICheckersService checkersService;
        private RoleType userRole;

        public event PropertyChangedEventHandler PropertyChanged;

        public RoleType UserRole
        {
            get => userRole;
            private set
            {
                if (userRole != value)
                {
                    userRole = value;
                    OnPropertyChanged();
                }
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public DrinkDetailPage()
        {
            this.InitializeComponent();

            this.drinkService = App.Host.Services.GetRequiredService<IDrinkService>();
            this.drinkReviewService = App.Host.Services.GetRequiredService<IDrinkReviewService>();
            this.userService = App.Host.Services.GetRequiredService<IUserService>();
            this.reviewService = App.Host.Services.GetRequiredService<IReviewService>();
            this.modificationRequestService = App.Host.Services.GetRequiredService<IDrinkModificationRequestService>();
            this.checkersService = App.Host.Services.GetRequiredService<ICheckersService>();

            this.ViewModel = new DrinkDetailPageViewModel(
                this.drinkService,
                this.drinkReviewService,
                this.userService,
                this.modificationRequestService);

            this.DataContext = this.ViewModel;

            this.UpdateRemoveButtonText();

            this.UpdateButton.OnDrinkUpdated = () =>
            {
                this.ViewModel.LoadDrink(this.ViewModel.Drink.DrinkId);
            };

            this.ViewModel.RequestOpenPopup += OpenAddReviewModal;
            this.ViewModel.RequestClosePopup += CloseAddReviewModal;

            this.HideButtonsOnBan();
            InitializeUserRole();
        }

        private async void InitializeUserRole()
        {
            this.UserRole = await this.userService.GetHighestRoleTypeForUser(App.CurrentUserId) ?? RoleType.User;
            this.ViewModel.IsAdmin = this.UserRole == RoleType.Admin;
        }

        private void FlagReviewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem menuItem && menuItem.DataContext is ReviewDTO review)
            {
                try
                {
                    this.reviewService.UpdateNumberOfFlagsForReview(review.ReviewId, review.NumberOfFlags + 1);
                    this.ViewModel.RefreshReviews();

                    Thread.Sleep(1000);
                    AuthenticationWindow.NavigationFrame.Navigate(typeof(DrinkDetailPage), this.ViewModel.Drink.DrinkId);
                }
                catch (Exception ex)
                {
                    ContentDialog dialog = new ContentDialog
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
                    this.reviewService.HideReview(review.ReviewId);
                    this.ViewModel.RefreshReviews();

                    // I have to do this since RefreshReviews is not async
                    Thread.Sleep(1000);
                    AuthenticationWindow.NavigationFrame.Navigate(typeof(DrinkDetailPage), this.ViewModel.Drink.DrinkId);
                }
                catch (Exception ex)
                {
                    ContentDialog dialog = new ContentDialog
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

        private async void AICheckMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem menuItem && menuItem.DataContext is ReviewDTO review)
            {
                Review regularReview = ReviewMapper.ToEntity(review);
                try
                {
                    await this.PrepareReviewForCheck(regularReview);

                    // If somebody with an actual key is checking this, this might not update because the function is of type void
                    // and not task
                    this.checkersService.RunAICheckForOneReviewAsync(regularReview);
                    this.ViewModel.RefreshReviews();

                    Thread.Sleep(1000);
                    AuthenticationWindow.NavigationFrame.Navigate(typeof(DrinkDetailPage), this.ViewModel.Drink.DrinkId);
                }
                catch
                {
                }
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
            this.AddReviewModalOverlay.Visibility = Visibility.Visible;
        }

        private void CloseAddReviewModal(object sender,EventArgs e)
        {
            this.AddReviewModalOverlay.Visibility = Visibility.Collapsed;
        }

        private void RefreshReviews(object sender, EventArgs e)
        {
            this.ViewModel.RefreshReviews();
        }
    }
}