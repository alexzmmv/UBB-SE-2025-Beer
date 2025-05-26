namespace WinUIApp.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using WinUIApp.ProxyServices;
    using WinUIApp.ProxyServices.Models;
    using WinUIApp.Services.DummyServices;

    public partial class UpdateDrinkMenuViewModel(Drink drinkToUpdate, IDrinkService drinkService,
        IUserService userService, IAdminService adminService) : INotifyPropertyChanged
    {
        private const float MaxAlcoholContent = 100.0f;
        private const float MinAlcoholContent = 0.0f;
        private readonly IDrinkService drinkService = drinkService;
        private readonly IUserService userService = userService;
        private readonly IAdminService adminService = adminService;
        private Drink drinkToUpdate = drinkToUpdate;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<string> AllCategories { get; set; } = new ();

        public ObservableCollection<string> SelectedCategoryNames { get; set; } = new ();

        public List<Category> AllCategoryObjects { get; set; } = new ();

        public List<Brand> AllBrands { get; set; } = new ();

        public Drink DrinkToUpdate
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
                if (this.DrinkToUpdate.DrinkBrand == null)
                {
                    return string.Empty;
                }

                return this.DrinkToUpdate.DrinkBrand.BrandName;
            }

            set
            {
                if (this.DrinkToUpdate.DrinkBrand == null || this.DrinkToUpdate.DrinkBrand.BrandName != value)
                {
                    this.DrinkToUpdate.DrinkBrand = new Brand(0, value);
                    this.OnPropertyChanged();
                }
            }
        }

        public string AlcoholContent
        {
            get => this.DrinkToUpdate.AlcoholContent.ToString();
            set
            {
                if (float.TryParse(value, out float parsedAlcoholContent) && this.DrinkToUpdate.AlcoholContent != parsedAlcoholContent)
                {
                    this.DrinkToUpdate.AlcoholContent = parsedAlcoholContent;
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
                this.DrinkToUpdate.CategoryList = this.GetSelectedCategories();
                this.drinkService.UpdateDrink(this.DrinkToUpdate);
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
                this.adminService.SendNotificationFromUserToAdmin(
                senderUserId: this.userService.CurrentUserId,
                userModificationRequestType: "Drink Update Request",
                userModificationRequestDetails: $"User requested to update drinkToUpdate: {this.DrinkToUpdate.DrinkName}");
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