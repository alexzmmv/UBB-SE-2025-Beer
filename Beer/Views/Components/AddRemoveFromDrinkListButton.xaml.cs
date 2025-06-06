namespace WinUIApp.Views.Components
{
    using DataAccess.Service.Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using WinUIApp.ViewModels;

    public sealed partial class AddRemoveFromDrinkListButton : UserControl
    {
        public static readonly DependencyProperty DrinkIdProperty =
            DependencyProperty.Register(
                "DrinkId",
                typeof(int),
                typeof(AddRemoveFromDrinkListButton),
                new PropertyMetadata(DEFAULT_INT_VALUE, new PropertyChangedCallback(OnDrinkIdChanged)));

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(DrinkPageViewModel),
                typeof(AddRemoveFromDrinkListButton),
                new PropertyMetadata(null, OnViewModelPropertyChanged));

        private const int DEFAULT_INT_VALUE = 0;
        private IDrinkService drinkService;
        private IUserService userService;

        public AddRemoveFromDrinkListButton()
        {
            this.InitializeComponent();

            this.drinkService = App.Host.Services.GetRequiredService<IDrinkService>();
            this.userService = App.Host.Services.GetRequiredService<IUserService>();

            this.Loaded += this.AddRemoveFromDrinkListButton_Loaded;
        }

        public int DrinkId
        {
            get { return (int)this.GetValue(DrinkIdProperty); }
            set { this.SetValue(DrinkIdProperty, value); }
        }

        public DrinkPageViewModel ViewModel
        {
            get { return (DrinkPageViewModel)this.GetValue(ViewModelProperty); }
            set { this.SetValue(ViewModelProperty, value); }
        }

        private static void OnDrinkIdChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArguments)
        {
            if (dependencyObject is AddRemoveFromDrinkListButton button && (int)eventArguments.NewValue > AddRemoveFromDrinkListButton.DEFAULT_INT_VALUE)
            {
                if (button.ViewModel == null)
                {
                    button.ViewModel = new DrinkPageViewModel((int)eventArguments.NewValue, button.drinkService);
                }
                else
                {
                    button.ViewModel.DrinkId = (int)eventArguments.NewValue;
                }
            }
        }

        private static void OnViewModelPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArguments)
        {
            if (dependencyObject is AddRemoveFromDrinkListButton button && eventArguments.NewValue != null)
            {
                button.DataContext = eventArguments.NewValue;
            }
        }

        private async void AddRemoveFromDrinkListButton_Loaded(object sender, RoutedEventArgs eventArguments)
        {
            if (this.ViewModel == null && this.DrinkId > DEFAULT_INT_VALUE)
            {
                this.ViewModel = new DrinkPageViewModel(this.DrinkId, this.drinkService);
            }

            if (this.ViewModel != null)
            {
                this.DataContext = this.ViewModel;
                await this.ViewModel.CheckIfInListAsync();
            }
        }

        private async void AddRemoveButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs eventArguments)
        {
            if (this.ViewModel != null)
            {
                await this.ViewModel.AddRemoveFromListAsync();
            }
        }
    }
}