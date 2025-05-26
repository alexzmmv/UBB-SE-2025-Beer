using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUIApp.ProxyServices.Models;

namespace WinUIApp.Views.Components
{
    public sealed partial class UpdateDrinkButton : UserControl
    {
        public static readonly DependencyProperty DrinkProperty =
        DependencyProperty.Register(
        nameof(Drink),
        typeof(Drink),
        typeof(UpdateDrinkButton),
        new PropertyMetadata(null));

        public UpdateDrinkButton()
        {
            this.InitializeComponent();
        }

        public Action OnDrinkUpdated { get; set; }

        public Drink Drink
        {
            get => (Drink)this.GetValue(DrinkProperty);
            set => this.SetValue(DrinkProperty, value);
        }

        private void UpdateDrinkButton_Click(object sender, RoutedEventArgs eventArguments)
        {
            var userService = new Services.DummyServices.UserService();
            Flyout flyout = new Flyout
            {
                Content = new UpdateDrinkFlyout
                {
                    DrinkToUpdate = this.Drink,
                    UserId = userService.GetCurrentUserId(),
                },
            };

            flyout.Closed += (sender, arguments) =>
            {
                this.OnDrinkUpdated?.Invoke();
            };

            flyout.ShowAt(this.UpdateButton);
        }
    }
}