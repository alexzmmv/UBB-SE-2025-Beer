using System;
using DataAccess.Service;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUiApp.Data.Data;
using WinUIApp.ProxyServices.Models;
using WinUIApp.WebAPI.Models;

namespace WinUIApp.Views.Components
{
    public sealed partial class UpdateDrinkButton : UserControl
    {
        public static readonly DependencyProperty DrinkProperty =
        DependencyProperty.Register(
        nameof(Drink),
        typeof(DrinkDTO),
        typeof(UpdateDrinkButton),
        new PropertyMetadata(null));

        public UpdateDrinkButton()
        {
            this.InitializeComponent();
        }

        public Action OnDrinkUpdated { get; set; }

        public DrinkDTO Drink
        {
            get => (DrinkDTO)this.GetValue(DrinkProperty);
            set => this.SetValue(DrinkProperty, value);
        }

        private void UpdateDrinkButton_Click(object sender, RoutedEventArgs eventArguments)
        {
            Flyout flyout = new Flyout
            {
                Content = new UpdateDrinkFlyout
                {
                    DrinkToUpdate = this.Drink,
                    UserId = App.CurrentUserId,
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