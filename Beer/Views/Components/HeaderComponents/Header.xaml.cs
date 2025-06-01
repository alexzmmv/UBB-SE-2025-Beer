namespace WinUIApp.Views.Components.HeaderComponents
{
    using System;
    using System.Linq;
    using DataAccess.Service.Interfaces;
    using DrinkDb_Auth;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using WinUIApp.ProxyServices;
    using WinUIApp.Utils.NavigationParameters;
    using WinUIApp.Views.Pages;
    using WinUIApp.Views.ViewModels;

    public sealed partial class Header : UserControl
    {
        private readonly HeaderViewModel viewModel;
        private IDrinkService drinkService;

        public Header()
        {
            drinkService = App.Host.Services.GetRequiredService<IDrinkService>();

            this.InitializeComponent();
            this.viewModel = new HeaderViewModel(drinkService);
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            if (AuthenticationWindow.PreviousPage != null)
            {
                AuthenticationWindow.NavigationFrame.Navigate(AuthenticationWindow.PreviousPage);
            }
            else
            {
                AuthenticationWindow.NavigationFrame.Navigate(typeof(MainPage));
            }
        }

        private void Logo_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            AuthenticationWindow.NavigationFrame.Navigate(typeof(MainPage));
        }

        public Visibility GoBackButtonVisibility { get; set; } = Visibility.Collapsed;

        public void SetGoBackButtonVisibility(bool isVisible)
        {
            GoBackButtonVisibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            // this.Bindings.Update();
        }

        public void UpdateHeaderComponentsVisibility(Type currentPageType)
        {
            bool shouldShowGoBackButton = currentPageType != typeof(MainPage);
            SetGoBackButtonVisibility(shouldShowGoBackButton);
        }
    }
}