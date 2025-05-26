namespace WinUIApp.Views.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using WinUIApp.ProxyServices;
    using WinUIApp.ProxyServices.Models;
    using WinUIApp.Services.DummyServices;

    public partial class DrinkDetailPageViewModel(IDrinkService drinkService, IDrinkReviewService reviewService, IUserService userService, IAdminService adminService) : INotifyPropertyChanged
    {
        private const string CategorySeparator = ", ";
        private readonly IDrinkService drinkService = drinkService;
        private readonly IDrinkReviewService reviewService = reviewService;
        private readonly IUserService userService = userService;
        private readonly IAdminService adminService = adminService;
        private Drink drink;
        private float averageReviewScore;

        public event PropertyChangedEventHandler PropertyChanged;

        public Drink Drink
        {
            get
            {
                return this.drink;
            }

            set
            {
                this.drink = value;
                this.OnPropertyChanged(nameof(this.Drink));
                this.OnPropertyChanged(nameof(this.CategoriesDisplay));
            }
        }

        public string CategoriesDisplay
        {
            get
            {
                if (this.Drink != null && this.Drink.CategoryList != null)
                {
                    return string.Join(CategorySeparator, this.Drink.CategoryList.Select(drinkCategory => drinkCategory.CategoryName));
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public float AverageReviewScore
        {
            get => this.averageReviewScore;
            set
            {
                this.averageReviewScore = value;
                this.OnPropertyChanged(nameof(this.AverageReviewScore));
            }
        }

        public ObservableCollection<Review> Reviews { get; set; } = new ObservableCollection<Review>();

        public void LoadDrink(int drinkId)
        {
            this.Drink = this.drinkService.GetDrinkById(drinkId);
            this.AverageReviewScore = this.reviewService.GetReviewAverageByDrinkID(drinkId);
            List<Review> reviews = this.reviewService.GetReviewsByDrinkID(drinkId);
            this.Reviews.Clear();
            foreach (Review review in reviews)
            {
                this.Reviews.Add(review);
            }
        }

        public bool IsCurrentUserAdmin()
        {
            return this.adminService.IsAdmin(this.userService.GetCurrentUserId());
        }

        public void RemoveDrink()
        {
            if (this.IsCurrentUserAdmin())
            {
                this.drinkService.DeleteDrink(this.Drink.DrinkId);
            }
            else
            {
                this.adminService.SendNotificationFromUserToAdmin(this.userService.GetCurrentUserId(), "Removal of drink with id:" + this.Drink.DrinkId + " and name:" + this.Drink.DrinkName, "User requested removal of drink from database.");
            }
        }

        public void VoteForDrink()
        {
            int userId = this.userService.GetCurrentUserId();
            try
            {
                this.drinkService.VoteDrinkOfTheDay(userId, this.Drink.DrinkId);
            }
            catch (Exception voteForDrinkException)
            {
                throw new Exception("Error happened while voting for a drink:", voteForDrinkException);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}