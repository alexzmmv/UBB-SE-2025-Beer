namespace WinUIApp.ViewModels
{
    using DataAccess.Service.Interfaces;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using WinUIApp.ProxyServices;
    using WinUIApp.WebAPI.Models;

    public class DrinkPageViewModel : INotifyPropertyChanged
    {
        private readonly IDrinkService drinkService;
        private readonly IUserService userService;
        private int userId;
        private int drinkId;
        private bool isInUserDrinksList;
        private string buttonText;
        private DrinkDTO drink;

        public event PropertyChangedEventHandler PropertyChanged;

        public DrinkDTO Drink
        {
            get => this.drink;
            set
            {
                this.drink = value;
                if (value != null)
                {
                    this.DrinkId = value.DrinkId;
                }
                this.OnPropertyChanged(nameof(this.Drink));
            }
        }

        public DrinkPageViewModel(IDrinkService drinkService, IUserService userService)
        {
            this.drinkService = drinkService;
            this.userService = userService;
            this.buttonText = "Add to Favorites";
        }

        public DrinkPageViewModel(int drinkId, IDrinkService drinkService, IUserService userService)
        {
            this.drinkService = drinkService;
            this.userService = userService;
            this.drinkId = drinkId;
            this.buttonText = "Add to Favorites";
        }

        public int UserId
        {
            get => this.userId;
            set
            {
                if (this.userId != value)
                {
                    this.userId = value;
                    this.OnPropertyChanged();
                    if (this.DrinkId > 0 && value > 0)
                    {
                        this.CheckIfInListAsync();
                    }
                }
            }
        }

        public int DrinkId
        {
            get => this.drinkId;
            set
            {
                if (this.drinkId != value)
                {
                    this.drinkId = value;
                    this.OnPropertyChanged();
                    if (value > 0)
                    {
                        this.CheckIfInListAsync();
                    }
                }
            }
        }

        public string ButtonText
        {
            get => this.buttonText;
            set
            {
                if (this.buttonText != value)
                {
                    this.buttonText = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public async Task CheckIfInListAsync()
        {
            if (this.DrinkId <= 0)
            {
                this.isInUserDrinksList = false;
                this.UpdateButtonText();
                return;
            }

            try
            {
                this.isInUserDrinksList = await Task.Run(() => this.drinkService.IsDrinkInUserPersonalList(this.GetCurrentUserId(), this.DrinkId));
                this.UpdateButtonText();
            }
            catch (Exception checkingDrinkListException)
            {
                Debug.WriteLine($"DrinkPageViewModel: Error checking drink list: {checkingDrinkListException.Message}");
            }
        }

        public async Task AddRemoveFromListAsync()
        {
            if (this.DrinkId <= 0)
            {
                return;
            }

            try
            {
                bool isOperationSuccessful;
                if (this.isInUserDrinksList)
                {
                    isOperationSuccessful = await Task.Run(() => this.drinkService.DeleteFromUserPersonalDrinkList(this.GetCurrentUserId(), this.DrinkId));
                    if (isOperationSuccessful)
                    {
                        this.isInUserDrinksList = false;
                    }
                }
                else
                {
                    isOperationSuccessful = await Task.Run(() => this.drinkService.AddToUserPersonalDrinkList(this.GetCurrentUserId(), this.DrinkId));
                    if (isOperationSuccessful)
                    {
                        this.isInUserDrinksList = true;
                    }
                }

                this.UpdateButtonText();
            }
            catch (Exception updateDrinkListException)
            {
                Debug.WriteLine($"DrinkPageViewModel: Error updating drink list: {updateDrinkListException.Message}");
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private Guid GetCurrentUserId()
        {
            Guid userId = App.CurrentUserId;
            return userId;
        }

        private void UpdateButtonText()
        {
            this.ButtonText = this.isInUserDrinksList ? "Remove from Favorites" : "Add to Favorites";
        }
    }
}