using System.Collections.Generic;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUIApp.ProxyServices.Models;
using WinUIApp.Views.Pages;

namespace WinUIApp.Views.Components
{
    public sealed partial class DrinkListComponent : UserControl
    {
        public static readonly DependencyProperty DrinksProperty =
           DependencyProperty.Register(
               "Drinks",
               typeof(List<Drink>),
               typeof(DrinkListComponent),
               new PropertyMetadata(null));

        public DrinkListComponent()
        {
            this.InitializeComponent();
        }

        public List<Drink> Drinks
        {
            get => (List<Drink>)this.GetValue(DrinksProperty);
            set => this.SetValue(DrinksProperty, value);
        }

        private void DrinkItem_Click(object sender, RoutedEventArgs eventArguments)
        {
            if (sender is Button button && button.Tag is int drinkId)
            {
                MainWindow.AppMainFrame.Navigate(typeof(DrinkDetailPage), drinkId);
            }
        }
    }
}