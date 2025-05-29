namespace WinUIApp.Views.Components.SearchPageComponents
{
    using System;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Controls.Primitives;

    public sealed partial class AlcoholContentFilterComponent : UserControl
    {
        private const double MinimumAlcoholPercentage = 0;
        private const double MaximumAlcoholPercentage = 100;

        public AlcoholContentFilterComponent()
        {
            this.InitializeComponent();
        }

        public event EventHandler<double> MinimumAlcoholContentChanged;

        public event EventHandler<double> MaximumAlcoholContentChanged;

        public double MinimumAlcoholContent
        {
            get => this.MinValueSlider.Value;
            set
            {
                this.MinValueSlider.Value = value;
            }
        }

        public double MaximumAlcoholContent
        {
            get => this.MaxValueSlider.Value;
            set
            {
                this.MaxValueSlider.Value = value;
            }
        }
        public void ResetMinSlider()
        {
            this.MinValueSlider.Value = MinimumAlcoholPercentage;
            this.MinimumAlcoholContentChanged?.Invoke(this, this.MinValueSlider.Value);
        }
        public void ResetMaxSlider()
        {
            this.MaxValueSlider.Value = MaximumAlcoholPercentage;
            this.MaximumAlcoholContentChanged?.Invoke(this, this.MaxValueSlider.Value);
        }

        public void ResetSliders()
        {
            this.ResetMinSlider();
            this.ResetMaxSlider();
        }

        private void MinValueSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs valueChangedEventArgs)
        {
            if (this.MinValueSlider.Value > this.MaxValueSlider.Value)
            {
                this.MinValueSlider.Value = MinimumAlcoholPercentage;
                return;
            }

            this.MinimumAlcoholContentChanged?.Invoke(this, this.MinValueSlider.Value);
        }

        private void MaxValueSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs valueChangedEventArgs)
        {
            if (this.MaxValueSlider.Value < this.MinValueSlider.Value)
            {
                this.MaxValueSlider.Value = MaximumAlcoholPercentage;
                return;
            }

            this.MaximumAlcoholContentChanged?.Invoke(this, this.MaxValueSlider.Value);
        }
    }
}