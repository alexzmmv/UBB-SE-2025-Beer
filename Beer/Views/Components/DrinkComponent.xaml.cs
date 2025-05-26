using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace WinUIApp.Views.Components
{
    public sealed partial class DrinkComponent : UserControl
    {
        public static readonly DependencyProperty BrandProperty =
            DependencyProperty.Register("Brand", typeof(string), typeof(DrinkComponent), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty CategoryProperty =
            DependencyProperty.Register("Category", typeof(string), typeof(DrinkComponent), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty AlcoholProperty =
            DependencyProperty.Register("Alcohol", typeof(string), typeof(DrinkComponent), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string), typeof(DrinkComponent), new PropertyMetadata(string.Empty));

        public DrinkComponent()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public string Brand
        {
            get { return (string)this.GetValue(BrandProperty); }
            set { this.SetValue(BrandProperty, value); }
        }

        public string Category
        {
            get { return (string)this.GetValue(CategoryProperty); }
            set { this.SetValue(CategoryProperty, value); }
        }

        public string Alcohol
        {
            get { return (string)this.GetValue(AlcoholProperty); }
            set { this.SetValue(AlcoholProperty, value); }
        }

        public string ImageSource
        {
            get { return (string)this.GetValue(ImageSourceProperty); }
            set { this.SetValue(ImageSourceProperty, value); }
        }
    }
}