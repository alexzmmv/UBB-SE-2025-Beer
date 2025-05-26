namespace WinUIApp.Views.Components.SearchPageComponents
{
    using System;
    using Microsoft.UI.Xaml.Controls;

    public sealed partial class SortSelectorComponent : UserControl
    {
        private const string SortFieldName = "Name";
        private const string SortFieldAlcoholContent = "Alcohol Content";
        private const string SortFieldAverageReviewScore = "Average Review Score";
        private const string SortOrderAscending = "Ascending";
        private const int SortByNameIndex = 0;
        private const int SortByAlcoholContentIndex = 1;
        private const int SortByAverageReviewScoreIndex = 2;
        private const int SortOrderAscendingIndex = 0;
        private const int SortOrderDescendingIndex = 1;

        public SortSelectorComponent()
        {
            this.InitializeComponent();
        }

        public event EventHandler<bool> SortOrderChanged;

        public event EventHandler<string> SortByChanged;

        public void SetSortOrder(bool isAscending)
        {
            this.SortOrderComboBox.SelectedIndex = isAscending ? SortOrderAscendingIndex : SortOrderDescendingIndex;
        }

        public void SetSortBy(string sortField)
        {
            switch (sortField)
            {
                case SortFieldName:
                    this.SortByComboBox.SelectedIndex = SortByNameIndex;
                    break;
                case SortFieldAlcoholContent:
                    this.SortByComboBox.SelectedIndex = SortByAlcoholContentIndex;
                    break;
                case SortFieldAverageReviewScore:
                    this.SortByComboBox.SelectedIndex = SortByAverageReviewScoreIndex;
                    break;
            }
        }

        private void SortByComboBox_SelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (this.SortByComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string sortField = selectedItem.Content.ToString();
                this.SortByChanged?.Invoke(this, sortField);
            }
        }

        private void SortOrderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (this.SortOrderComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                bool isAscending = selectedItem.Content.ToString() == SortOrderAscending;
                this.SortOrderChanged?.Invoke(this, isAscending);
            }
        }
    }
}
