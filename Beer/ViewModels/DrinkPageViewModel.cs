namespace WinUIApp.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using WinUIApp.ProxyServices;
    using WinUIApp.Services.DummyServices;

    public class DrinkPageViewModel : INotifyPropertyChanged
    {
        private readonly IDrinkService drinkService;
        private readonly IUserService userService;
        private int userId;
        private int drinkId;
        private bool isInUserDrinksList;
        private string buttonText;

        public event PropertyChangedEventHandler PropertyChanged;

        public DrinkPageViewModel(IDrinkService drinkService, IUserService userService)
        {
            this.drinkService = drinkService;
            this.userService = userService;
            this.buttonText = "\U0001F5A4";
        }

        public DrinkPageViewModel(int drinkId, IDrinkService drinkService, IUserService userService)
        {
            this.drinkService = drinkService;
            this.userService = userService;
            this.drinkId = drinkId;
            this.buttonText = "\U0001F5A4";
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
                    if (this.GetCurrentUserId() > 0 && value > 0)
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
            if (this.GetCurrentUserId() <= 0 || this.DrinkId <= 0)
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
            if (this.GetCurrentUserId() <= 0 || this.DrinkId <= 0)
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

        private int GetCurrentUserId()
        {
            int userId = this.userService.GetCurrentUserId();
            return userId;
        }

        private void UpdateButtonText()
        {
            this.ButtonText = this.isInUserDrinksList ? "\u2665" : "\U0001F5A4";
        }
    }
}