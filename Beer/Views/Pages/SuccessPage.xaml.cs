using DrinkDb_Auth.View;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using WinUIApp.Views.Pages;

namespace DrinkDb_Auth
{
    public sealed partial class SuccessPage : Page
    {
        private AuthenticationWindow mainWindow;

        public SuccessPage()
        {
            this.InitializeComponent();
            Loaded += SuccessPage_Loaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is AuthenticationWindow window)
            {
                this.mainWindow = window;
            }
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.mainWindow != null)
            {
                AuthenticationWindow.NavigationFrame.Navigate(typeof(WinUIApp.Views.Pages.MainPage));
            }

            //MainWindow mainWindow = new MainWindow();
            //mainWindow.Activate();
        }

        private void SuccessPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.CircleAnimation.Begin();
        }
    }
}