using System.Collections.Generic;
using System.Linq;
using DataAccess.Service.Interfaces;
using DataAccess.Constants;
using DrinkDb_Auth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using WinUiApp.Data.Data;
using WinUIApp.ProxyServices;
using WinUIApp.Utils.NavigationParameters;
using WinUIApp.ViewModels;
using WinUIApp.Views.Components.SearchPageComponents;

namespace WinUIApp.Views.Pages
{
    public sealed partial class MainPage : Page
    {
        private readonly MainPageViewModel viewModel;
        private SearchPageViewModel searchPageViewModel;
        private IDrinkReviewService drinkReviewService;
        private IDrinkService drinkService;
        private IUserService userService;
        private bool isAdmin;

        public bool IsAdmin => isAdmin;

        public MainPage()
        {
            this.InitializeComponent();

            this.drinkService = App.Host.Services.GetRequiredService<IDrinkService>();
            this.drinkReviewService = App.Host.Services.GetRequiredService<IDrinkReviewService>();
            this.userService = App.Host.Services.GetRequiredService<IUserService>();

            this.viewModel = new MainPageViewModel(this.drinkService);
            this.DataContext = this.viewModel;
            InitializeAdminStatus();
        }

        private async void InitializeAdminStatus()
        {
            this.isAdmin = await userService.GetHighestRoleTypeForUser(App.CurrentUserId) == RoleType.Admin;
        }

        protected override void OnNavigatedTo(NavigationEventArgs eventArguments)
        {
            base.OnNavigatedTo(eventArguments);
            this.searchPageViewModel = new SearchPageViewModel(this.drinkService, this.drinkReviewService);
            this.SortSelectorControl.SetSortOrder(this.searchPageViewModel.IsAscending);
            if (eventArguments.Parameter is SearchPageNavigationParameters parameters)
            {
                if (parameters.SelectedCategoryFilters != null)
                {
                    this.searchPageViewModel.SetInitialCategoryFilter(parameters.SelectedCategoryFilters);
                }

                if (parameters.InputSearchKeyword != null)
                {
                    this.searchPageViewModel.SetSearchedTerms(parameters.InputSearchKeyword);
                }
            }

            AuthenticationWindow.CurrentPage = typeof(MainPage);
            AuthenticationWindow.PreviousPage = typeof(MainPage);

            this.LoadDrinks();
            this.LoadCategoriesFilter();
            this.LoadBrandsFilter();
        }

        public Visibility SearchBarVisibility { get; set; } = Visibility.Collapsed;

        private void SearchDrinksButton_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            List<Category> categories = new List<Category>();
            SearchPageNavigationParameters navigationParameters = new SearchPageNavigationParameters
            {
                SelectedCategoryFilters = categories,
                InputSearchKeyword = this.DrinkSearchBox.Text,
            };
            AuthenticationWindow.NavigationFrame.Navigate(typeof(MainPage), navigationParameters);
        }

        private void DrinkOfTheDayComponent_Tapped(object sender, TappedRoutedEventArgs eventArguments)
        {
            AuthenticationWindow.NavigationFrame.Navigate(typeof(DrinkDetailPage), this.viewModel.GetDrinkOfTheDayId());
        }

        private void FilterButtonClick(object sender, RoutedEventArgs eventArguments)
        {
            this.LoadDrinks();
        }

        private void ClearFiltersClick(object sender, RoutedEventArgs eventArguments)
        {
            this.searchPageViewModel.ClearFilters();
            this.CategoryFilterControl.ClearSelection();
            this.BrandFilterControl.ClearSelection();
            this.AlcoholContentFilterControl.ResetSliders();
            this.RatingFilterControl.ClearSelection();
            this.LoadDrinks();
        }

        private void VerticalDrinkListControl_DrinkClicked(object sender, int drinkId)
        {
            this.searchPageViewModel.OpenDrinkDetailPage(drinkId);
        }

        private void LoadDrinks()
        {
            IEnumerable<DrinkDisplayItem> drinks = this.searchPageViewModel.GetDrinks();

            if (drinks.Count() == 0)
            {
                this.SecondColumnLine.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.SecondColumnLine.Visibility = Visibility.Visible;
            }

            this.VerticalDrinkListControl.SetDrinks(drinks);
        }

        private void SortByDropdownControl_SortByChanged(object sender, string sortField)
        {
            this.searchPageViewModel.SetSortByField(sortField);
            this.LoadDrinks();
        }

        private void SortSelectorControl_SortOrderChanged(object sender, bool isAscending)
        {
            this.searchPageViewModel.SetSortOrder(isAscending);
            this.LoadDrinks();
        }

        private void LoadCategoriesFilter()
        {
            IEnumerable<Category> categories = this.searchPageViewModel.GetCategories();
            IEnumerable<Category> initialCategories = this.GetInitialCategories();
            this.CategoryFilterControl.SetCategoriesFilter(categories, initialCategories);
        }

        private void CategoryFilterControl_CategoryChanged(object sender, List<string> categories)
        {
            this.searchPageViewModel.SetCategoryFilter(categories);
        }

        private List<Category> GetInitialCategories()
        {
            return this.searchPageViewModel.InitialCategories;
        }

        private void LoadBrandsFilter()
        {
            IEnumerable<Brand> brands = this.searchPageViewModel.GetBrands();
            this.BrandFilterControl.SetBrandFilter(brands);
        }

        private void BrandFilterControl_BrandChanged(object sender, List<string> brands)
        {
            this.searchPageViewModel.SetBrandFilter(brands);
        }

        private void AlcoholContentFilterControl_MinimumAlcoholContentChanged(object sender, double minimumAlcoholContent)
        {
            float minAlcoholContent = (float)minimumAlcoholContent;
            this.searchPageViewModel.SetMinAlcoholFilter(minAlcoholContent);
        }

        private void AlcoholContentFilterControl_MaximumAlcoholContentChanged(object sender, double maximumAlcoholContent)
        {
            float maxAlcoholContent = (float)maximumAlcoholContent;
            this.searchPageViewModel.SetMaxAlcoholFilter(maxAlcoholContent);
        }

        private void RatingFilterControl_RatingChanged(object sender, float rating)
        {
            this.searchPageViewModel.SetMinRatingFilter(rating);
        }
    }
}