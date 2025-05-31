namespace WinUIApp.Views.Pages
{
    using DataAccess.Service;
    using DataAccess.Service.Interfaces;
    using DataAccess.Constants;
    using DrinkDb_Auth;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Input;
    using Microsoft.UI.Xaml.Navigation;
    using WinUIApp.ProxyServices;
    using WinUIApp.ViewModels;
    using WinUIApplication.Views.Components;

    public sealed partial class MainPage : Page
    {
        private readonly MainPageViewModel viewModel;
        private IDrinkReviewService drinkReviewService;
        private IDrinkService drinkService;
        private IUserService userService;
        private bool isAdmin;

        public bool IsAdmin => isAdmin;

        public MainPage()
        {
            this.InitializeComponent();

            drinkService = App.Host.Services.GetRequiredService<IDrinkService>();
            drinkReviewService = App.Host.Services.GetRequiredService<IDrinkReviewService>();
            userService = App.Host.Services.GetRequiredService<IUserService>();

            this.viewModel = new MainPageViewModel(drinkService, userService);
            this.DataContext = this.viewModel;
            InitializeAdminStatus();
        }

        private async void InitializeAdminStatus()
        {
            isAdmin = await userService.GetHighestRoleTypeForUser(App.CurrentUserId) == RoleType.Admin;
        }

        protected override void OnNavigatedTo(NavigationEventArgs eventArguments)
        {
            base.OnNavigatedTo(eventArguments);
            AuthenticationWindow.CurrentPage = typeof(MainPage);
            AuthenticationWindow.PreviousPage = typeof(MainPage);
        }

        private void DrinkOfTheDayComponent_Tapped(object sender, TappedRoutedEventArgs eventArguments)
        {
            AuthenticationWindow.NavigationFrame.Navigate(typeof(DrinkDetailPage), this.viewModel.GetDrinkOfTheDayId());
        }
    }
}