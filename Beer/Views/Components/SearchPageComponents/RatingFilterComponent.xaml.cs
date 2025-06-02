namespace WinUIApp.Views.Components.SearchPageComponents
{
    using System;
    using Microsoft.UI.Xaml.Controls;

    public sealed partial class RatingFilterComponent : UserControl
    {
        private const int MAXIMUM_STAR_RATING = 5;
        private const string FIVE_STARS_OPTION = "5 Stars";
        private const string FOUR_STARS_OPTION = "4 Stars";
        private const string THREE_STARTS_OPTION = "3 Stars";
        private const string TWO_STARS_OPTION = "2 Stars";
        private const string ONE_STAR_OPTION = "1 Star";

        public RatingFilterComponent()
        {
            this.InitializeComponent();
        }

        public event EventHandler<float> RatingChanged;

        public void SetSortBy(string sortField)
        {
            switch (sortField)
            {
                case RatingFilterComponent.FIVE_STARS_OPTION:
                    this.RatingComboBox.SelectedIndex = 0;
                    break;
                case RatingFilterComponent.FOUR_STARS_OPTION:
                    this.RatingComboBox.SelectedIndex = 1;
                    break;
                case RatingFilterComponent.THREE_STARTS_OPTION:
                    this.RatingComboBox.SelectedIndex = 2;
                    break;
                case RatingFilterComponent.TWO_STARS_OPTION:
                    this.RatingComboBox.SelectedIndex = 3;
                    break;
                case RatingFilterComponent.ONE_STAR_OPTION:
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
                float selectedStarRating = RatingFilterComponent.MAXIMUM_STAR_RATING - this.RatingComboBox.SelectedIndex;
                this.RatingChanged?.Invoke(this, selectedStarRating);
            }
        }
    }
}