using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using WinUIApp;
using WinUIApp.Views.Pages;

namespace DrinkDb_Auth
{
    public sealed partial class ConfirmLogoutPage : Page
    {
        public ConfirmLogoutPage()
        {
            this.InitializeComponent();
            Loaded += ConfirmLogoutPage_Loaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            AuthenticationWindow.CurrentPage = typeof(SuccessPage);
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            App.CurrentUserId = Guid.Empty;
            App.CurrentSessionId = Guid.Empty;

            var window = (AuthenticationWindow)App.MainWindow;
            window.HandleLogout();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            AuthenticationWindow.NavigationFrame.Navigate(typeof(MainPage));
        }

        private void ConfirmLogoutPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.CircleAnimation.Begin();
        }
    }
}