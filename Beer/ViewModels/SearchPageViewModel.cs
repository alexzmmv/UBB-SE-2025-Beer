namespace WinUIApp.ViewModels
{
    using DataAccess.Service.Interfaces;
    using DrinkDb_Auth;
    using System.Collections.Generic;
    using System.Linq;
    using WinUiApp.Data.Data;
    using WinUIApp.ProxyServices;
    using WinUIApp.ProxyServices.Models;
    using WinUIApp.Views;
    using WinUIApp.Views.Components.SearchPageComponents;
    using WinUIApp.Views.Pages;
    using WinUIApp.WebAPI.Models;

    public class SearchPageViewModel(IDrinkService drinkService, IDrinkReviewService reviewService)
    {
        private const string NameField = "Name";

        private readonly IDrinkService drinkService = drinkService;
        private IDrinkReviewService reviewService = reviewService;

        private bool isAscending = true;
        private string fieldToSortBy = NameField;

        private List<string>? categoryFilter;
        private List<string>? brandFilter;
        private float? minAlcoholFilter;
        private float? maxAlcoholFilter;
        private float? minRatingFilter;
        private string? searchedTerms;

        public List<Category> InitialCategories { get; set; }

        public bool IsAscending
        {
            get => this.isAscending;
            set => this.isAscending = value;
        }

        public string FieldToSortBy
        {
            get => this.fieldToSortBy;
            set => this.fieldToSortBy = value;
        }

        public void OpenDrinkDetailPage(int id)
        {
            AuthenticationWindow.NavigationFrame.Navigate(typeof(DrinkDetailPage), id);
        }

        public void ClearFilters()
        {
            this.categoryFilter = null;
            this.brandFilter = null;
            this.minAlcoholFilter = null;
            this.maxAlcoholFilter = null;
            this.minRatingFilter = null;
            this.searchedTerms = null;
        }

        public IEnumerable<DrinkDisplayItem> GetDrinks()
        {
            List<DrinkDisplayItem> displayItems = new List<DrinkDisplayItem>();

            if (this.fieldToSortBy == NameField || this.fieldToSortBy == "Alcohol Content")
            {
                string sortField;
                if (this.fieldToSortBy == NameField)
                {
                    sortField = "DrinkName";
                }
                else
                {
                    sortField = "AlcoholContent";
                }

                Dictionary<string, bool> orderBy = new Dictionary<string, bool>
                {
                    { sortField, this.isAscending },
                };
                List<DrinkDTO> drinks = this.drinkService.GetDrinks(
                    searchKeyword: this.searchedTerms,
                    drinkBrandNameFilter: this.brandFilter,
                    drinkCategoryFilter: this.categoryFilter,
                    minimumAlcoholPercentage: this.minAlcoholFilter,
                    maximumAlcoholPercentage: this.maxAlcoholFilter,
                    orderingCriteria: orderBy);

                displayItems = new List<DrinkDisplayItem>();
                foreach (DrinkDTO drink in drinks.Where(d => !d.IsRequestingApproval))
                {
                    float averageScore = this.reviewService.GetReviewAverageByDrinkID(drink.DrinkId);
                    if (this.minRatingFilter == null)
                    {
                        displayItems.Add(new DrinkDisplayItem(drink, averageScore));
                    }
                    else
                    {
                        if (averageScore >= this.minRatingFilter)
                        {
                            displayItems.Add(new DrinkDisplayItem(drink, averageScore));
                        }
                    }
                }
            }
            else
            {
                List<DrinkDTO> drinks = this.drinkService.GetDrinks(
                    searchKeyword: this.searchedTerms,
                    drinkBrandNameFilter: this.brandFilter,
                    drinkCategoryFilter: this.categoryFilter,
                    minimumAlcoholPercentage: this.minAlcoholFilter,
                    maximumAlcoholPercentage: this.maxAlcoholFilter,
                    orderingCriteria: null);

                displayItems = new List<DrinkDisplayItem>();
                foreach (DrinkDTO drink in drinks.Where(d => !d.IsRequestingApproval))
                {
                    float averageScore = this.reviewService.GetReviewAverageByDrinkID(drink.DrinkId);
                    if (this.minRatingFilter == null)
                    {
                        displayItems.Add(new DrinkDisplayItem(drink, averageScore));
                    }
                    else
                    {
                        if (averageScore >= this.minRatingFilter)
                        {
                            displayItems.Add(new DrinkDisplayItem(drink, averageScore));
                        }
                    }
                }

                if (this.isAscending)
                {
                    displayItems = displayItems.OrderBy(item => item.AverageReviewScore).ToList();
                }
                else
                {
                    displayItems = displayItems.OrderByDescending(item => item.AverageReviewScore).ToList();
                }
            }

            return displayItems;
        }

        public IEnumerable<Category> GetCategories()
        {
            return this.drinkService.GetDrinkCategories();
        }

        public IEnumerable<Brand> GetBrands()
        {
            return this.drinkService.GetDrinkBrandNames();
        }

        public void SetSortByField(string sortByField)
        {
            this.fieldToSortBy = sortByField;
        }

        public void SetSortOrder(bool isAscending)
        {
            this.isAscending = isAscending;
        }

        public void SetCategoryFilter(List<string> categoryFilter)
        {
            this.categoryFilter = categoryFilter;
        }

        public void SetInitialCategoryFilter(List<Category> initialCategoties)
        {
            this.InitialCategories = initialCategoties;
            List<string> categories = new List<string>();
            foreach (Category category in this.InitialCategories)
            {
                categories.Add(category.CategoryName);
            }

            this.SetCategoryFilter(categories);
        }

        public void SetBrandFilter(List<string> brandFilter)
        {
            this.brandFilter = brandFilter;
        }

        public void SetMinAlcoholFilter(float minAlcoholFilter)
        {
            this.minAlcoholFilter = minAlcoholFilter;
        }

        public void SetMaxAlcoholFilter(float maxAlcoholFilter)
        {
            this.maxAlcoholFilter = maxAlcoholFilter;
        }

        public void SetMinRatingFilter(float minRatingFilter)
        {
            this.minRatingFilter = minRatingFilter;
        }

        public void SetSearchedTerms(string searchedTerms)
        {
            this.searchedTerms = searchedTerms;
        }
    }
}
