namespace WinUIApp.Views.Components.SearchPageComponents
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Microsoft.UI.Xaml.Controls;
    using WinUIApp.ProxyServices.Models;

    public sealed partial class BrandFilterComponent : UserControl
    {
        private List<Brand> originalBrands = new List<Brand>();
        private HashSet<Brand> selectedBrands = new HashSet<Brand>();

        public BrandFilterComponent()
        {
            this.InitializeComponent();
        }

        public event EventHandler<List<string>> BrandChanged;

        public ObservableCollection<Brand> CurrentBrands { get; set; } = new ObservableCollection<Brand>();

        public void SetBrandFilter(IEnumerable<Brand> brands)
        {
            this.originalBrands = brands?.ToList() ?? new List<Brand>();
            this.CurrentBrands.Clear();
            foreach (Brand brand in this.originalBrands)
            {
                this.CurrentBrands.Add(brand);
            }

            this.BrandList.SelectedItems.Clear();
            foreach (Brand brand in this.CurrentBrands)
            {
                if (this.selectedBrands.Contains(brand))
                {
                    this.BrandList.SelectedItems.Add(brand);
                }
            }
        }

        public void ClearSelection()
        {
            this.BrandList.SelectedItems.Clear();
            this.selectedBrands.Clear();
            this.BrandChanged?.Invoke(this, new List<string>());
        }

        private void BrandListView_SelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            foreach (Brand removedBrand in selectionChangedEventArgs.RemovedItems)
            {
                this.selectedBrands.Remove(removedBrand);
            }

            foreach (Brand addedBrand in selectionChangedEventArgs.AddedItems)
            {
                this.selectedBrands.Add(addedBrand);
            }

            this.BrandChanged?.Invoke(this, this.selectedBrands.Select(brand => brand.BrandName).ToList());
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            string searchQuery = this.SearchBox.Text.ToLower();
            List<Brand> filteredBrands = this.originalBrands
                .Where(brand => brand.BrandName.ToLower().Contains(searchQuery))
                .ToList();

            this.BrandList.SelectionChanged -= this.BrandListView_SelectionChanged;

            this.CurrentBrands.Clear();
            foreach (Brand brand in filteredBrands)
            {
                this.CurrentBrands.Add(brand);
            }

            this.BrandList.SelectedItems.Clear();
            foreach (Brand brand in filteredBrands)
            {
                if (this.selectedBrands.Contains(brand))
                {
                    this.BrandList.SelectedItems.Add(brand);
                }
            }

            this.BrandList.SelectionChanged += this.BrandListView_SelectionChanged;
        }
    }
}