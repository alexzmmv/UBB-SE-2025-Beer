using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUiApp.Data.Data;
using WinUIApp.ProxyServices.Models;
using WinUIApp.Views.Pages;
using WinUIApp.WebAPI.Models;

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
            get
            {
                var dtos = (List<DrinkDTO>)this.GetValue(DrinksProperty);

                if (dtos == null)
                    return new List<Drink>();

                return dtos.Select(dto => ToEntity(dto)).ToList();
            }
            set => this.SetValue(DrinksProperty, value);
        }

        public static Drink ToEntity(DrinkDTO dto)
        {
            return new Drink
            {
                DrinkId = dto.DrinkId,
                DrinkName = dto.DrinkName ?? string.Empty,
                DrinkURL = dto.DrinkImageUrl,
                BrandId = dto.DrinkBrand?.BrandId,
                Brand = dto.DrinkBrand,
                AlcoholContent = (decimal)dto.AlcoholContent,
                DrinkCategories = dto.CategoryList.Select(c => new DrinkCategory
                {
                    CategoryId = c.CategoryId,
                    Category = c
                }).ToList()
            };
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