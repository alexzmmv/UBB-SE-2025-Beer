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
            this.CategoryMenu.PopulateCategories(this.viewModel.GetCategories());
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

        private void SearchDrinksButton_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            SearchPageNavigationParameters navigationParameters = new SearchPageNavigationParameters
            {
                SelectedCategoryFilters = this.CategoryMenu.SelectedCategories.ToList(),
                InputSearchKeyword = this.DrinkSearchBox.Text,
            };
            AuthenticationWindow.PreviousPage = AuthenticationWindow.CurrentPage;
            AuthenticationWindow.NavigationFrame.Navigate(typeof(SearchPage), navigationParameters);
        }

        public Visibility SearchBarVisibility { get; set; } = Visibility.Collapsed;

        public Visibility GoBackButtonVisibility { get; set; } = Visibility.Collapsed;

        public void SetSearchBarVisibility(bool isVisible)
        {
            SearchBarVisibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            this.Bindings.Update();
        }

        public void SetGoBackButtonVisibility(bool isVisible)
        {
            GoBackButtonVisibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            this.Bindings.Update();
        }

        public void UpdateHeaderComponentsVisibility(Type currentPageType)
        {
            bool shouldShowSearchBar = currentPageType == typeof(MainPage) || currentPageType == typeof(SearchPage);
            SetSearchBarVisibility(shouldShowSearchBar);

            bool shouldShowGoBackButton = currentPageType != typeof(MainPage);
            SetGoBackButtonVisibility(shouldShowGoBackButton);
        }
    }
}