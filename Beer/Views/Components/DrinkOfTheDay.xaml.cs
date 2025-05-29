using System.Collections.Generic;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUiApp.Data.Data;
using WinUIApp.ProxyServices.Models;

namespace WinUIApp.Views.Components
{
    public sealed partial class DrinkOfTheDayComponent : UserControl
    {
        public static readonly DependencyProperty DrinkNameProperty =
            DependencyProperty.Register("DrinkName", typeof(string), typeof(DrinkOfTheDayComponent), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty DrinkBrandProperty =
            DependencyProperty.Register("DrinkBrand", typeof(string), typeof(DrinkOfTheDayComponent), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty DrinkCategoriesProperty =
            DependencyProperty.Register("DrinkCategories", typeof(List<Category>), typeof(DrinkOfTheDayComponent), new PropertyMetadata(new List<Category>()));

        public static readonly DependencyProperty AlcoholContentProperty =
            DependencyProperty.Register("AlcoholContent", typeof(float), typeof(DrinkOfTheDayComponent), new PropertyMetadata(DefaultFloatValue));

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string), typeof(DrinkOfTheDayComponent), new PropertyMetadata(string.Empty));

        private const float DefaultFloatValue = 0.0f;

        public DrinkOfTheDayComponent()
        {
            this.InitializeComponent();
        }

        public string DrinkName
        {
            get { return (string)this.GetValue(NameProperty); }
            set { this.SetValue(NameProperty, value); }
        }

        public string DrinkBrand
        {
            get { return (string)this.GetValue(DrinkBrandProperty); }
            set { this.SetValue(DrinkBrandProperty, value); }
        }

        public List<Category> DrinkCategories
        {
            get { return (List<Category>)this.GetValue(DrinkCategoriesProperty); }
            set { this.SetValue(DrinkCategoriesProperty, value); }
        }

        public float AlcoholContent
        {
            get { return (float)this.GetValue(AlcoholContentProperty); }
            set { this.SetValue(AlcoholContentProperty, value); }
        }

        public string ImageSource
        {
            get { return (string)this.GetValue(ImageSourceProperty); }
            set { this.SetValue(ImageSourceProperty, value); }
        }
    }
}