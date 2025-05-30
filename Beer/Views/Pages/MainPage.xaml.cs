namespace WinUIApp.Views.Pages
{
    using DataAccess.Service;
    using DataAccess.Service.Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Input;
    using Microsoft.UI.Xaml.Navigation;
    using WinUIApp.ProxyServices;
    using WinUIApp.ViewModels;

    public sealed partial class MainPage : Page
    {
        private readonly MainPageViewModel viewModel;
        private IDrinkReviewService drinkReviewService;
        private IDrinkService drinkService;
        private IUserService userService;

        public MainPage()
        {
            this.InitializeComponent();

            drinkService = App.Host.Services.GetRequiredService<IDrinkService>();
            drinkReviewService = App.Host.Services.GetRequiredService<IDrinkReviewService>();
            userService = App.Host.Services.GetRequiredService<IUserService>();

            this.viewModel = new MainPageViewModel(drinkService, userService);
            this.DataContext = this.viewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs eventArguments)
        {
            base.OnNavigatedTo(eventArguments);
            MainWindow.PreviousPage = typeof(MainPage);
        }

        private void DrinkOfTheDayComponent_Tapped(object sender, TappedRoutedEventArgs eventArguments)
        {
            var drinkId = this.viewModel.GetDrinkOfTheDayId();
            MainWindow.AppMainFrame.Navigate(typeof(DrinkDetailPage), drinkId);
        }
    }
}