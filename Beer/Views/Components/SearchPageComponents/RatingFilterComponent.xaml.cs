namespace WinUIApp.Views.Components.SearchPageComponents
{
    using System;
    using Microsoft.UI.Xaml.Controls;

    public sealed partial class RatingFilterComponent : UserControl
    {
        private const int MaximumStarRating = 5;
        private const string FiveStarsOption = "5 Stars";
        private const string FourStarsOption = "4 Stars";
        private const string ThreeStarsOption = "3 Stars";
        private const string TwoStarsOption = "2 Stars";
        private const string OneStarOption = "1 Star";

        public RatingFilterComponent()
        {
            this.InitializeComponent();
        }

        public event EventHandler<float> RatingChanged;

        public void SetSortBy(string sortField)
        {
            switch (sortField)
            {
                case FiveStarsOption:
                    this.RatingComboBox.SelectedIndex = 0;
                    break;
                case FourStarsOption:
                    this.RatingComboBox.SelectedIndex = 1;
                    break;
                case ThreeStarsOption:
                    this.RatingComboBox.SelectedIndex = 2;
                    break;
                case TwoStarsOption:
                    this.RatingComboBox.SelectedIndex = 3;
                    break;
                case OneStarOption:
                    this.RatingComboBox.SelectedIndex = 4;
                    break;
            }
        }

        public void ClearSelection()
        {
            this.RatingComboBox.SelectedItem = null;
        }

        private void RatingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (this.RatingComboBox.SelectedItem is ComboBoxItem)
            {
                float selectedStarRating = MaximumStarRating - this.RatingComboBox.SelectedIndex;
                this.RatingChanged?.Invoke(this, selectedStarRating);
            }
        }
    }
}