namespace WinUIApp.Views.Components.SearchPageComponents
{
    using System;
    using System.Collections.Generic;
    using Microsoft.UI.Xaml.Controls;

    public sealed partial class VerticalDrinkListComponent : UserControl
    {
        public VerticalDrinkListComponent()
        {
            this.InitializeComponent();
        }

        public event EventHandler<int> DrinkClicked;

        public IEnumerable<DrinkDisplayItem> DrinksList { get; set; }

        public void SetDrinks(IEnumerable<DrinkDisplayItem> drinks)
        {
            this.DrinksList = drinks;
            this.DrinkListView.ItemsSource = this.DrinksList;
        }

        private void DrinkList_ItemClick(object sender, ItemClickEventArgs itemClickEventArgs)
        {
            if (itemClickEventArgs.ClickedItem is DrinkDisplayItem selectedDrink)
            {
                this.DrinkClicked?.Invoke(this, selectedDrink.Drink.DrinkId);
            }
        }
    }
}