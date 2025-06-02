using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using DataAccess.Service.Interfaces;
using WinUiApp.Data.Data;

namespace WinUIApp.ViewModels
{
    public partial class AddDrinkMenuViewModel(IDrinkService drinkService) : INotifyPropertyChanged
    {
        private const float MAX_ALCOHOL_CONTENT = 100.0f;
        private const float MIN_ALCOHOL_CONTENT = 0.0f;

        private readonly IDrinkService drinkService = drinkService;

        private string newDrinkName = string.Empty;
        private string newDrinkURL = string.Empty;
        private string newDrinkBrandName = string.Empty;
        private string newDrinkAlcoholContent = string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<string> AllCategories { get; set; } = new ();

        public ObservableCollection<string> SelectedCategoryNames { get; set; } = new ();

        public List<Category> AllCategoryObjects { get; set; } = new ();

        public List<Brand> AllBrands { get; set; } = new ();

        public string DrinkName
        {
            get => this.newDrinkName;
            set
            {
                if (this.newDrinkName != value)
                {
                    this.newDrinkName = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public string DrinkURL
        {
            get => this.newDrinkURL;
            set
            {
                if (this.newDrinkURL != value)
                {
                    this.newDrinkURL = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public string BrandName
        {
            get => this.newDrinkBrandName;
            set
            {
                if (this.newDrinkBrandName != value)
                {
                    this.newDrinkBrandName = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public string AlcoholContent
        {
            get => this.newDrinkAlcoholContent;
            set
            {
                if (this.newDrinkAlcoholContent != value)
                {
                    this.newDrinkAlcoholContent = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public List<Category> GetSelectedCategories()
        {
            return this.SelectedCategoryNames
                .Select(name => AllCategoryObjects.FirstOrDefault(drinkCategory => drinkCategory.CategoryName == name))
                .Where(selectedCategory => selectedCategory != null)
                .ToList();
        }

        public void ValidateUserDrinkInput()
        {
            if (string.IsNullOrWhiteSpace(this.DrinkName))
            {
                throw new ArgumentException("Drink name is required");
            }

            if (string.IsNullOrWhiteSpace(this.BrandName))
            {
                throw new ArgumentException("Brand is required");
            }

            if (!float.TryParse(this.AlcoholContent, out var alcoholContentValue) ||
                alcoholContentValue < AddDrinkMenuViewModel.MIN_ALCOHOL_CONTENT ||
                alcoholContentValue > AddDrinkMenuViewModel.MAX_ALCOHOL_CONTENT)
            {
                throw new ArgumentException("Valid alcohol content (0–100%) is required");
            }

            if (this.SelectedCategoryNames.Count == 0)
            {
                throw new ArgumentException("At least one drinkCategory must be selected");
            }
        }

        public void InstantAddDrink()
        {
            try
            {
                List<Category> categories = this.GetSelectedCategories();
                float alcoholContent = float.Parse(this.AlcoholContent);

                this.drinkService.AddDrink(
                    inputtedDrinkName: this.DrinkName,
                    inputtedDrinkPath: this.DrinkURL,
                    inputtedDrinkCategories: categories,
                    inputtedDrinkBrandName: this.BrandName,
                    inputtedAlcoholPercentage: alcoholContent,
                    userId: App.CurrentUserId);
            }
            catch
            {
                throw;
            }
        }

        public void SendAddDrinkRequest()
        {
            try
            {
                List<Category> categories = this.GetSelectedCategories();
                float alcoholContent = float.Parse(this.AlcoholContent);

                List<Brand> existingBrands = this.drinkService.GetDrinkBrandNames();
                Brand? brand = existingBrands.FirstOrDefault(brand => brand.BrandName.Equals(this.BrandName, StringComparison.OrdinalIgnoreCase));

                if (brand == null)
                {
                    throw new ArgumentException($"Brand '{this.BrandName}' does not exist. Please select an existing brand.");
                }

                this.drinkService.AddDrink(
                    inputtedDrinkName: this.DrinkName,
                    inputtedDrinkPath: this.DrinkURL,
                    inputtedDrinkCategories: categories,
                    inputtedDrinkBrandName: brand.BrandName,
                    inputtedAlcoholPercentage: alcoholContent,
                    userId: App.CurrentUserId,
                    isDrinkRequestingApproval: true);
            }
            catch
            {
                throw;
            }
        }

        public void ClearForm()
        {
            this.DrinkName = string.Empty;
            this.DrinkURL = string.Empty;
            this.BrandName = string.Empty;
            this.AlcoholContent = string.Empty;
            this.SelectedCategoryNames.Clear();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}