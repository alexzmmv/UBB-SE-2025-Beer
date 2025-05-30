namespace WinUIApp.Views.Components.HeaderComponents
{
    using System.Linq;
    using DataAccess.Service.Interfaces;
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
            if (MainWindow.PreviousPage != null)
            {
                MainWindow.AppMainFrame.Navigate(MainWindow.PreviousPage);
            }
            else
            {
                MainWindow.AppMainFrame.Navigate(typeof(MainPage));
            }
        }

        private void Logo_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            MainWindow.AppMainFrame.Navigate(typeof(MainPage));
        }

        private void SearchDrinksButton_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            SearchPageNavigationParameters navigationParameters = new SearchPageNavigationParameters
            {
                SelectedCategoryFilters = this.CategoryMenu.SelectedCategories.ToList(),
                InputSearchKeyword = this.DrinkSearchBox.Text,
            };
            MainWindow.PreviousPage = MainWindow.CurrentPage;
            MainWindow.AppMainFrame.Navigate(typeof(SearchPage), navigationParameters);
        }
    }
}