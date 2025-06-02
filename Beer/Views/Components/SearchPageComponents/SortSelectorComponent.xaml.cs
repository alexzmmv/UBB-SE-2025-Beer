namespace WinUIApp.Views.Components.SearchPageComponents
{
    using System;
    using Microsoft.UI.Xaml.Controls;

    public sealed partial class SortSelectorComponent : UserControl
    {
        private const string SORT_FIELD_NAME = "Name";
        private const string SORT_FIELD_ALCOHOL_CONTENT = "Alcohol Content";
        private const string SORT_FIELD_AVERAGE_REVIEW_SCORE = "Average Review Score";
        private const string SORT_ORDER_ASCENDING = "Ascending";
        private const int SORT_BY_NAME_INDEX = 0;
        private const int SORT_BY_ALCOHOL_CONTENT_INDEX = 1;
        private const int SORT_BY_AVERAGE_REVIEW_SCORE_INDEX = 2;
        private const int SORT_ORDER_ASCENDING_INDEX = 0;
        private const int SORT_ORDER_DESCENDING_INDEX = 1;

        public SortSelectorComponent()
        {
            this.InitializeComponent();
        }

        public event EventHandler<bool> SortOrderChanged;

        public event EventHandler<string> SortByChanged;

        public void SetSortOrder(bool isAscending)
        {
            this.SortOrderComboBox.SelectedIndex = isAscending ? SortSelectorComponent.SORT_ORDER_ASCENDING_INDEX : SortSelectorComponent.SORT_ORDER_DESCENDING_INDEX;
        }

        public void SetSortBy(string sortField)
        {
            switch (sortField)
            {
                case SortSelectorComponent.SORT_FIELD_NAME:
                    this.SortByComboBox.SelectedIndex = SortSelectorComponent.SORT_BY_NAME_INDEX;
                    break;
                case SortSelectorComponent.SORT_FIELD_ALCOHOL_CONTENT:
                    this.SortByComboBox.SelectedIndex = SortSelectorComponent.SORT_BY_ALCOHOL_CONTENT_INDEX;
                    break;
                case SortSelectorComponent.SORT_FIELD_AVERAGE_REVIEW_SCORE:
                    this.SortByComboBox.SelectedIndex = SortSelectorComponent.SORT_BY_AVERAGE_REVIEW_SCORE_INDEX;
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
                bool isAscending = selectedItem.Content.ToString() == SortSelectorComponent.SORT_ORDER_ASCENDING;
                this.SortOrderChanged?.Invoke(this, isAscending);
            }
        }
    }
}
