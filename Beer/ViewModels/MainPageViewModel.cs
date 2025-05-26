namespace WinUIApp.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using WinUIApp.ProxyServices;
    using WinUIApp.ProxyServices.Models;
    using WinUIApp.Services.DummyServices;

    internal class MainPageViewModel
    {
        private const int HardCodedNumberOfDrinks = 5;
        private IDrinkService drinkService;
        private IUserService userService;
        private string imageSource;
        private string drinkName;
        private string drinkBrand;
        private List<Category> drinkCategories;
        private float alcoholContent;

        public MainPageViewModel(IDrinkService drinkService, IUserService userService)
        {
            this.drinkService = drinkService;
            this.userService = userService;
            this.LoadDrinkOfTheDayData();
            this.LoadPersonalDrinkListData();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string ImageSource
        {
            get => this.imageSource;
            set => this.SetField(ref this.imageSource, value);
        }

        public string DrinkName
        {
            get => this.drinkName;
            set => this.SetField(ref this.drinkName, value);
        }

        public string DrinkBrand
        {
            get => this.drinkBrand;
            set => this.SetField(ref this.drinkBrand, value);
        }

        public List<Category> DrinkCategories
        {
            get => this.drinkCategories;
            set => this.SetField(ref this.drinkCategories, value);
        }

        public float AlcoholContent
        {
            get => this.alcoholContent;
            set => this.SetField(ref this.alcoholContent, value);
        }

        public List<Drink> PersonalDrinks { get; set; } = new List<Drink>();

        public void LoadDrinkOfTheDayData()
        {
            var drink = this.drinkService.GetDrinkOfTheDay();

            this.ImageSource = drink.DrinkImageUrl;
            this.DrinkName = drink.DrinkName;
            this.DrinkBrand = drink.DrinkBrand.BrandName;
            this.DrinkCategories = drink.CategoryList;
            this.AlcoholContent = drink.AlcoholContent;
        }

        public int GetDrinkOfTheDayId()
        {
            return this.drinkService.GetDrinkOfTheDay().DrinkId;
        }

        protected bool SetField<TProperty>(ref TProperty field, TProperty value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<TProperty>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void LoadPersonalDrinkListData()
        {
            int userId = this.userService.GetCurrentUserId();
            this.PersonalDrinks = this.drinkService.GetUserPersonalDrinkList(userId, HardCodedNumberOfDrinks);
        }
    }
}