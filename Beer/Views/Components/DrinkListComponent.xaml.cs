using System.Collections.Generic;
using DrinkDb_Auth;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUIApp.Views.Pages;
using WinUIApp.WebAPI.Models;

namespace WinUIApp.Views.Components
{
    public sealed partial class DrinkListComponent : UserControl
    {
        public static readonly DependencyProperty DrinksProperty =
           DependencyProperty.Register(
               "Drinks",
               typeof(List<DrinkDTO>),
               typeof(DrinkListComponent),
               new PropertyMetadata(null));

        public DrinkListComponent()
        {
            this.InitializeComponent();
        }

        public List<DrinkDTO> Drinks
        {
            get => (List<DrinkDTO>)this.GetValue(DrinksProperty);
            set => this.SetValue(DrinksProperty, value);
        }

        private void DrinkItem_Click(object sender, RoutedEventArgs eventArguments)
        {
            if (sender is Button button && button.Tag is int drinkId)
            {
                AuthenticationWindow.NavigationFrame.Navigate(typeof(DrinkDetailPage), drinkId);
            }
        }
    }
}