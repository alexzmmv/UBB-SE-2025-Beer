using DataAccess.Service.Interfaces;
using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Navigation;
using System;
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

        public DrinkDetailPage()
        {
            this.InitializeComponent();

            this.drinkService = App.Host.Services.GetRequiredService<IDrinkService>();
            this.drinkReviewService = App.Host.Services.GetRequiredService<IDrinkReviewService>();
            this.userService = App.Host.Services.GetRequiredService<IUserService>();

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
        }

        public DrinkDetailPageViewModel ViewModel { get; }

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
        }

        private void ConfirmRemoveButton_Click(object sender, RoutedEventArgs eventArguments)
        {
            this.ViewModel.RemoveDrinkAsync();
            MainWindow.AppMainFrame.Navigate(MainWindow.PreviousPage);
        }

        private void VoteButton_Click(object sender, RoutedEventArgs eventArguments)
        {
            this.ViewModel.VoteForDrink();
        }

        private void AddRatingButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ViewModel?.Drink == null)
            {
                return;
            }

            int productId = this.ViewModel.Drink.DrinkId;

            IConfiguration configuration = App.Host.Services.GetRequiredService<IConfiguration>();
            IReviewService reviewService = App.Host.Services.GetRequiredService<IReviewService>();
            IUserService userService = App.Host.Services.GetRequiredService<IUserService>();

            ReviewViewModel reviewViewModel = new ReviewViewModel(reviewService, userService);

        }

        private void OpenAddReviewModal(object sender, EventArgs e)
        {
            AddReviewModalOverlay.Visibility = Visibility.Visible;
        }

        private void CloseAddReviewModal(object sender,EventArgs e)
        {

            AddReviewModalOverlay.Visibility = Visibility.Collapsed;
        }
}
}