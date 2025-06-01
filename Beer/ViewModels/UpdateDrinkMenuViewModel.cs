namespace WinUIApp.ViewModels
{
    using DataAccess.Service.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using WinUiApp.Data.Data;
    using WinUIApp.ProxyServices;
    using WinUIApp.ProxyServices.Models;
    using WinUIApp.WebAPI.Models;

    public partial class UpdateDrinkMenuViewModel(DrinkDTO drinkToUpdate, IDrinkService drinkService,
        IUserService userService) : INotifyPropertyChanged
    {
        private const float MaxAlcoholContent = 100.0f;
        private const float MinAlcoholContent = 0.0f;
        private readonly IDrinkService drinkService = drinkService;
        private readonly IUserService userService = userService;
        private DrinkDTO drinkToUpdate = drinkToUpdate;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<string> AllCategories { get; set; } = new ();

        public ObservableCollection<string> SelectedCategoryNames { get; set; } = new ();

        public List<Category> AllCategoryObjects { get; set; } = new ();

        public List<Brand> AllBrands { get; set; } = new ();

        public DrinkDTO DrinkToUpdate
        {
            get => this.drinkToUpdate;
            set
            {
                this.drinkToUpdate = value;
                this.OnPropertyChanged();
            }
        }

        public string DrinkName
        {
            get => this.DrinkToUpdate.DrinkName;
            set
            {
                if (this.DrinkToUpdate.DrinkName != value)
                {
                    this.DrinkToUpdate.DrinkName = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public string DrinkURL
        {
            get => this.DrinkToUpdate.DrinkImageUrl;
            set
            {
                if (this.DrinkToUpdate.DrinkImageUrl != value)
                {
                    this.DrinkToUpdate.DrinkImageUrl = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public string BrandName
        {
            get
            {
                if (this.DrinkToUpdate.DrinkImageUrl == null)
                {
                    return string.Empty;
                }

                return this.DrinkToUpdate.DrinkBrand.BrandName;
            }

            set
            {
                if (this.DrinkToUpdate.DrinkBrand == null || this.DrinkToUpdate.DrinkBrand.BrandName != value)
                {
                    this.DrinkToUpdate.DrinkBrand = new Brand { BrandId = 0, BrandName = value };
                    this.OnPropertyChanged();
                }
            }
        }

        public string AlcoholContent
        {
            get => this.DrinkToUpdate.AlcoholContent.ToString();
            set
            {
                if (decimal.TryParse(value, out decimal parsedAlcoholContent) && this.DrinkToUpdate.AlcoholContent != (float)parsedAlcoholContent)
                {
                    this.DrinkToUpdate.AlcoholContent = (float)parsedAlcoholContent;
                    this.OnPropertyChanged();
                }
            }
        }

        public List<Category> GetSelectedCategories()
        {
            return this.SelectedCategoryNames
                .Select(name => AllCategoryObjects.FirstOrDefault(category => category.CategoryName == name))
                .Where(SelectedCategory => SelectedCategory != null)
                .ToList();
        }

        public void ValidateUpdatedDrinkDetails()
        {
            if (string.IsNullOrWhiteSpace(this.DrinkName))
            {
                throw new ArgumentException("Drink name is required");
            }

            if (string.IsNullOrWhiteSpace(this.BrandName))
            {
                throw new ArgumentException("Brand is required.");
            }

            Brand? validBrand = this.AllBrands.FirstOrDefault(brand => brand.BrandName.Equals(this.BrandName, StringComparison.OrdinalIgnoreCase));
            if (validBrand == null)
            {
                throw new ArgumentException("The brand you entered does not exist.");
            }

            this.DrinkToUpdate.DrinkBrand = validBrand;

            if (!float.TryParse(this.AlcoholContent, out var alcoholContent) || alcoholContent < MinAlcoholContent || alcoholContent > MaxAlcoholContent)
            {
                throw new ArgumentException("Valid alcohol content (0–100%) is required");
            }

            if (this.SelectedCategoryNames.Count == 0)
            {
                throw new ArgumentException("At least one category must be selected");
            }
        }

        public void InstantUpdateDrink()
        {
            try
            {
                this.DrinkToUpdate.DrinkBrand = this.FindBrandByName(this.BrandName);
                this.DrinkToUpdate.CategoryList = (List<Category>)this.GetSelectedCategories();
                this.drinkService.UpdateDrink(this.DrinkToUpdate, App.CurrentUserId);
                Debug.WriteLine("Drink updated successfully (admin).");
            }
            catch (Exception instantUpdateDrinkException)
            {
                Debug.WriteLine($"Error updating drinkToUpdate: {instantUpdateDrinkException.Message}");
            }
        }

        public void SendUpdateDrinkRequest()
        {
            try
            {
                // TODO: Admin Service Notify
            }
            catch (Exception sendUpdateDrinkRequestException)
            {
                Debug.WriteLine($"Error sending update request: {sendUpdateDrinkRequestException.Message}");
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private Brand FindBrandByName(string searchedBrandName)
        {
            List<Brand> existingBrands = this.drinkService.GetDrinkBrandNames();
            Brand? match = existingBrands.FirstOrDefault(searchedBrand => searchedBrand.BrandName.Equals(searchedBrandName, StringComparison.OrdinalIgnoreCase));

            if (match == null)
            {
                throw new ArgumentException("The brand you tried to add was not found.");
            }

            return match;
        }
    }
}