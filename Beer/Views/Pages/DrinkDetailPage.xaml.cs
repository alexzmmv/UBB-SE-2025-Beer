using Microsoft.Extensions.Configuration;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using WinUIApp.ProxyServices;
using WinUIApp.Services.DummyServices;
using WinUIApp.ViewModels;
using WinUIApp.Views.ViewModels;
using WinUIApp.Views.Windows;

namespace WinUIApp.Views.Pages
{
    public sealed partial class DrinkDetailPage : Page
    {
        public DrinkDetailPage()
        {
            this.InitializeComponent();
            this.DataContext = this.ViewModel;
            if (this.ViewModel.IsCurrentUserAdmin())
            {
                this.RemoveButtonText.Text = "Remove drink";
            }
            else
            {
                this.RemoveButtonText.Text = "Send remove drink request";
            }

            this.UpdateButton.OnDrinkUpdated = () =>
            {
                this.ViewModel.LoadDrink(this.ViewModel.Drink.DrinkId);
            };
        }

        public DrinkDetailPageViewModel ViewModel { get; } = new DrinkDetailPageViewModel(new ProxyDrinkService(), new ProxyDrinkReviewService(), new Services.DummyServices.UserService(), new Services.DummyServices.AdminService());

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
            this.ViewModel.RemoveDrink();
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

            IConfiguration configuration = App.GetService<IConfiguration>();
            IRatingService ratingService = App.GetService<IRatingService>();
            IReviewService reviewService = App.GetService<IReviewService>();
            IUserService userService = App.GetService<IUserService>();

            RatingViewModel ratingViewModel = new RatingViewModel(ratingService);
            ReviewViewModel reviewViewModel = new ReviewViewModel(reviewService, userService);

            RatingMainPageViewModel mainVm = new RatingMainPageViewModel(configuration, ratingViewModel, reviewViewModel, productId);

            ratingViewModel.LoadRatingsForProduct(productId);

            RatingReviewWindow window = new RatingReviewWindow(mainVm, productId);
            window.Activate();
        }
    }
}