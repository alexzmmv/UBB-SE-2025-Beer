namespace WinUIApp.Views.Components.SearchPageComponents
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.UI.Xaml.Controls;
    using WinUiApp.Data.Data;

    public sealed partial class CategoryFilterComponent : UserControl
    {
        private const int SELECTION_DELAY_MILLISECONDS = 50;
        private List<Category> originalCategories = new List<Category>();
        private HashSet<Category> selectedCategories = new HashSet<Category>();

        public CategoryFilterComponent()
        {
            this.InitializeComponent();
        }

        public event EventHandler<List<string>> CategoryChanged;

        public ObservableCollection<Category> CurrentCategories { get; set; } = new ObservableCollection<Category>();

        public void CategoryListView_SelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            foreach (Category removedCategory in selectionChangedEventArgs.RemovedItems)
            {
                this.selectedCategories.Remove(removedCategory);
            }

            foreach (Category addedCategory in selectionChangedEventArgs.AddedItems)
            {
                this.selectedCategories.Add(addedCategory);
            }

            this.CategoryChanged?.Invoke(this, this.selectedCategories.Select(category => category.CategoryName).ToList());
        }

        public async void SetCategoriesFilter(IEnumerable<Category> categories, IEnumerable<Category> initialCategories)
        {
            this.originalCategories = categories.ToList();
            this.CurrentCategories.Clear();
            foreach (Category category in this.originalCategories)
            {
                this.CurrentCategories.Add(category);
            }

            HashSet<int> categoryIdentifiers = new HashSet<int>();
            if (initialCategories != null)
            {
                foreach (Category category in initialCategories)
                {
                    categoryIdentifiers.Add(category.CategoryId);
                }
            }

            this.CategoryList.SelectedItems.Clear();
            await Task.Delay(SELECTION_DELAY_MILLISECONDS);
            foreach (Category category in this.originalCategories)
            {
                if (categoryIdentifiers.Contains(category.CategoryId))
                {
                    this.CategoryList.SelectedItems.Add(category);
                    this.selectedCategories.Add(category);
                }
            }
        }

        public void ClearSelection()
        {
            this.CategoryList.SelectedItems.Clear();
            this.selectedCategories.Clear();
            this.CategoryChanged?.Invoke(this, new List<string>());
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            string searchQuery = this.SearchBox.Text.ToLower();
            List<Category> filteredCategories = this.originalCategories
                .Where(category => category.CategoryName.ToLower().Contains(searchQuery))
                .ToList();

            this.CategoryList.SelectionChanged -= this.CategoryListView_SelectionChanged;

            this.CurrentCategories.Clear();
            foreach (Category category in filteredCategories)
            {
                this.CurrentCategories.Add(category);
            }

            this.CategoryList.SelectedItems.Clear();
            foreach (Category category in filteredCategories)
            {
                if (this.selectedCategories.Contains(category))
                {
                    this.CategoryList.SelectedItems.Add(category);
                }
            }

            this.CategoryList.SelectionChanged += this.CategoryListView_SelectionChanged;
        }
    }
}