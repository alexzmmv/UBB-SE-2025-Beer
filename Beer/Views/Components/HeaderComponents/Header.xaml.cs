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
        private HeaderViewModel viewModel;
        private IDrinkService drinkService;
        private bool isInitialized = false;

        public Header()
        {
            this.InitializeComponent();
        }

        public void Initialize()
        {
            if (!isInitialized)
            {
                drinkService = App.Host.Services.GetRequiredService<IDrinkService>();
                this.viewModel = new HeaderViewModel(drinkService);
                this.NavMenu.Initialize();
                isInitialized = true;
            }
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