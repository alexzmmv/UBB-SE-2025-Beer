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

        private void GoHomeButton_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            AuthenticationWindow.NavigationFrame.Navigate(typeof(MainPage));
        }
    }
}