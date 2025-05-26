namespace WinUIApp.Views.Pages
{
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Input;
    using Microsoft.UI.Xaml.Navigation;
    using WinUIApp.ProxyServices;
    using WinUIApp.Services.DummyServices;
    using WinUIApp.ViewModels;

    public sealed partial class MainPage : Page
    {
        private readonly MainPageViewModel viewModel;

        public MainPage()
        {
            this.InitializeComponent();
            this.viewModel = new MainPageViewModel(new ProxyDrinkService(), new UserService());
            this.DataContext = this.viewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs eventArguments)
        {
            base.OnNavigatedTo(eventArguments);
            MainWindow.PreviousPage = typeof(MainPage);
        }

        private void DrinkOfTheDayComponent_Tapped(object sender, TappedRoutedEventArgs eventArguments)
        {
            MainWindow.AppMainFrame.Navigate(typeof(DrinkDetailPage), this.viewModel.GetDrinkOfTheDayId());
        }
    }
}