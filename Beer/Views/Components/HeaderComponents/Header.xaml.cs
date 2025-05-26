namespace WinUIApp.Views.Components.HeaderComponents
{
    using System.Linq;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using WinUIApp.ProxyServices;
    using WinUIApp.Utils.NavigationParameters;
    using WinUIApp.Views.Pages;
    using WinUIApp.Views.ViewModels;

    public sealed partial class Header : UserControl
    {
        private readonly HeaderViewModel viewModel;

        public Header()
        {
            this.InitializeComponent();
            this.viewModel = new HeaderViewModel(new ProxyDrinkService());
            this.CategoryMenu.PopulateCategories(this.viewModel.GetCategories());
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
            MainWindow.AppMainFrame.Navigate(typeof(SearchPage), navigationParameters);
        }
    }
}