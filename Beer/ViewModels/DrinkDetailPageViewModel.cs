namespace WinUIApp.Views.ViewModels
{
    using DataAccess.Constants;
    using DataAccess.DTOModels;
    using DataAccess.Service.Interfaces;
    using DrinkDb_Auth.ViewModel.AdminDashboard.Components;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using WinUiApp.Data.Data;
    using WinUIApp.ProxyServices;
    using WinUIApp.ProxyServices.Models;
    using WinUIApp.ViewModels;
    using WinUIApp.WebAPI.Models;

    public partial class DrinkDetailPageViewModel(IDrinkService drinkService, IDrinkReviewService reviewService, IUserService userService, IDrinkModificationRequestService modificationRequestService) : ViewModelBase
    {
        private readonly IDrinkService drinkService = drinkService;
        private readonly IDrinkReviewService reviewService = reviewService;
        private readonly IUserService userService = userService;
        private readonly IDrinkModificationRequestService modificationRequestService = modificationRequestService;
        private DrinkDTO drink;
        private float averageReviewScore;
        private const int NUMBER_OF_DECIMALS_DISPLAYED = 1;
        private bool isAdmin;
        private string drinkName;
        private string drinkURL;
        private string brandName;
        private string alcoholContent;
        private List<string> allCategories;
        private string content;

        public event PropertyChangedEventHandler PropertyChanged;

        private bool isPopupOpen;
        public bool IsPopupOpen
        {
            get => isPopupOpen;
            set
            {
                isPopupOpen = value;
                this.OnPropertyChanged(nameof(this.IsPopupOpen));
            }
        }

        public bool IsAdmin
        {
            get => isAdmin;
            set
            {
                isAdmin = value;
                this.OnPropertyChanged(nameof(this.IsAdmin));
            }
        }

        public event EventHandler RequestOpenPopup;
        public event EventHandler RequestClosePopup;

        public ICommand ShowPopupCommand => new RelayCommand(() => 
        {
            IsPopupOpen = true;
            RequestOpenPopup?.Invoke(this, EventArgs.Empty);
        });
        public ICommand ClosePopupCommand => new RelayCommand(() =>
        {
            IsPopupOpen = true;
            RequestClosePopup?.Invoke(this, EventArgs.Empty);
        });

        public DrinkDTO Drink
        {
            get => this.drink;
            set
            {
                this.drink = value;
                if (value != null)
                {
                    this.DrinkName = value.DrinkName;
                    this.DrinkURL = value.DrinkImageUrl;
                    this.BrandName = value.DrinkBrand?.BrandName ?? string.Empty;
                    this.AlcoholContent = value.AlcoholContent.ToString();
                }
                this.OnPropertyChanged(nameof(this.Drink));
            }
        }

        public DrinkDTO DrinkDTO => this.Drink;

        public string DrinkName
        {
            get => this.drinkName;
            set
            {
                this.drinkName = value;
                this.OnPropertyChanged(nameof(this.DrinkName));
            }
        }

        public string DrinkURL
        {
            get => this.drinkURL;
            set
            {
                this.drinkURL = value;
                this.OnPropertyChanged(nameof(this.DrinkURL));
            }
        }

        public string BrandName
        {
            get => this.brandName;
            set
            {
                this.brandName = value;
                this.OnPropertyChanged(nameof(this.BrandName));
            }
        }

        public string AlcoholContent
        {
            get => this.alcoholContent;
            set
            {
                this.alcoholContent = value;
                this.OnPropertyChanged(nameof(this.AlcoholContent));
            }
        }

        public List<string> AllCategories
        {
            get => this.allCategories;
            set
            {
                this.allCategories = value;
                this.OnPropertyChanged(nameof(this.AllCategories));
            }
        }

        public string Content
        {
            get => this.content;
            set
            {
                this.content = value;
                this.OnPropertyChanged(nameof(this.Content));
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

        public ObservableCollection<ReviewDTO> Reviews { get; set; } = new ObservableCollection<ReviewDTO>();

        public void LoadDrink(int drinkId)
        {
            this.Drink = this.drinkService.GetDrinkById(drinkId);
            this.RefreshReviews();
            this.LoadCategories();
        }

        private void LoadCategories()
        {
            List<Category> categories = this.drinkService.GetDrinkCategories();
            this.AllCategories = categories.Select(c => c.CategoryName).ToList();
        }

        public void RefreshReviews()
        {
            if (this.Drink == null) return;
            
            this.AverageReviewScore = (float)Math.Round(this.reviewService.GetReviewAverageByDrinkID(this.Drink.DrinkId), NUMBER_OF_DECIMALS_DISPLAYED);
            List<ReviewDTO> reviews = this.reviewService.GetReviewsByDrinkID(this.Drink.DrinkId);
            this.Reviews.Clear();
            foreach (ReviewDTO review in reviews)
            {
                this.Reviews.Add(review);
            }
        }

        public async Task<bool> IsCurrentUserAdminAsync()
        {
            return await this.userService.GetHighestRoleTypeForUser(App.CurrentUserId) == (RoleType.Admin);
        }

        public async Task RemoveDrinkAsync()
        {
            if (await this.IsCurrentUserAdminAsync())
            {
                this.drinkService.DeleteDrink(this.Drink.DrinkId, App.CurrentUserId);
            }
            else
            {
                this.modificationRequestService.AddRequest(
                    DrinkModificationRequestType.Remove,
                    this.Drink.DrinkId,
                    null,
                    App.CurrentUserId);
            }
        }

        public void VoteForDrink()
        {
            Guid userId = App.CurrentUserId;
            try
            {
                this.drinkService.VoteDrinkOfTheDay(userId, this.Drink.DrinkId);
            }
            catch (Exception voteForDrinkException)
            {
                throw new Exception("Error happened while voting for a drink:", voteForDrinkException);
            }
        }
    }
}
