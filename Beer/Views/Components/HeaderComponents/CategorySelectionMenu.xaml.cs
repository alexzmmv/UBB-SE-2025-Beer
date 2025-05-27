namespace WinUIApp.Views.Components.HeaderComponents
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Microsoft.UI.Xaml.Controls;
    using WinUiApp.Data.Data;
    using WinUIApp.ProxyServices.Models;

    public sealed partial class CategorySelectionMenu : UserControl
    {
        private List<Category> originalCategories = new List<Category>();
        private HashSet<Category> selectedCategories = new HashSet<Category>();

        public CategorySelectionMenu()
        {
            this.InitializeComponent();
        }

        public ObservableCollection<Category> CurrentCategories { get; set; } = new ObservableCollection<Category>();

        public HashSet<Category> SelectedCategories => this.selectedCategories;

        public void PopulateCategories(List<Category> categories)
        {
            this.originalCategories = categories;
            this.CurrentCategories = new ObservableCollection<Category>(categories);
        }

        private void CategoryList_SelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            foreach (Category removedCategory in selectionChangedEventArgs.RemovedItems)
            {
                this.selectedCategories.Remove(removedCategory);
            }

            foreach (Category addedCategory in selectionChangedEventArgs.AddedItems)
            {
                this.selectedCategories.Add(addedCategory);
            }
        }

        private void CategorySearchBox_TextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            string searchQuery = this.CategorySearchBox.Text.ToLower();
            List<Category> filteredCategories = this.originalCategories
                .Where(category => category.CategoryName.ToLower().Contains(searchQuery))
                .ToList();
            this.CategoryList.SelectionChanged -= this.CategoryList_SelectionChanged;
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

            this.CategoryList.SelectionChanged += this.CategoryList_SelectionChanged;
        }
    }
}