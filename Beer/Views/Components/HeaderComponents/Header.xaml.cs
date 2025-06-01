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
        public bool isInitialized = false;

        public Header()
        {
            this.InitializeComponent();
        }

        public void Initialize()
        {
            if (!isInitialized)
            {
                this.NavMenu.Initialize();
                isInitialized = true;
            }
        }

        private void GoHomeButton_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            AuthenticationWindow.NavigationFrame.Navigate(typeof(MainPage));
        }

        public void HideAddButton()
        {
            this.AddDrinkButtonControl.Visibility = Visibility.Collapsed;
        }

        public void ShowAddButton()
        {
            this.AddDrinkButtonControl.Visibility = Visibility.Visible;
        }
    }
}