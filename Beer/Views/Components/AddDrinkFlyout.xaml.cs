namespace WinUIApp.Views.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DataAccess.Service;
    using DrinkDb_Auth.ServiceProxy;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using WinUIApp.ProxyServices;
    using WinUIApp.ViewModels;

    public sealed partial class AddDrinkFlyout : UserControl
    {
        private readonly AdminService adminService;
        private AddDrinkMenuViewModel viewModel;

        public AddDrinkFlyout()
        {
            this.adminService = new AdminService();
            this.InitializeComponent();
            this.Loaded += this.AddDrinkFlyout_Loaded;
            this.CategoryList.SelectionChanged += this.CategoryList_SelectionChanged;

            this.SearchBox.TextChanged += (sender, eventArguments) =>
            {
                string query = this.SearchBox.Text.ToLower();

                List<string> filteredCategories = this.viewModel.AllCategories
                    .Where(category => category.ToLower().Contains(query))
                    .ToList();

                this.CategoryList.SelectionChanged -= this.CategoryList_SelectionChanged;
                this.CategoryList.ItemsSource = filteredCategories;

                this.DispatcherQueue.TryEnqueue(() =>
                {
                    foreach (var category in filteredCategories)
                    {
                        if (this.viewModel.SelectedCategoryNames.Contains(category))
                        {
                            this.CategoryList.SelectedItems.Add(category);
                        }
                    }

                    this.CategoryList.SelectionChanged += this.CategoryList_SelectionChanged;
                });
            };
        }

        public int UserId { get; set; }

        private void AddDrinkFlyout_Loaded(object sender, RoutedEventArgs eventArguments)
        {
            var drinkService = new ProxyDrinkService();
            var userService = new UserServiceProxy();
            bool isAdmin = this.adminService.IsAdmin(this.UserId);

            var allBrands = drinkService.GetDrinkBrandNames();
            var allCategories = drinkService.GetDrinkCategories();

            this.viewModel = new AddDrinkMenuViewModel(
                drinkService,
                userService,
                this.adminService)
            {
                AllBrands = allBrands,
                AllCategoryObjects = allCategories,
                AllCategories = allCategories.Select(category => category.CategoryName).ToList(),
            };

            this.DataContext = this.viewModel;

            if (isAdmin)
            {
                this.SaveButton.Content = "Add Drink";
            }
            else
            {
                this.SaveButton.Content = "Send Request to Admin";
            }

            this.CategoryList.ItemsSource = this.viewModel.AllCategories;
        }

        private void CategoryList_SelectionChanged(object sender, SelectionChangedEventArgs eventArguments)
        {
            if (this.viewModel == null)
            {
                return;
            }

            foreach (var removedCategory in eventArguments.RemovedItems.Cast<string>())
            {
                this.viewModel.SelectedCategoryNames.Remove(removedCategory);
            }

            foreach (var addedCategory in eventArguments.AddedItems.Cast<string>())
            {
                if (!this.viewModel.SelectedCategoryNames.Contains(addedCategory))
                {
                    this.viewModel.SelectedCategoryNames.Add(addedCategory);
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs eventArguments)
        {
            try
            {
                this.viewModel.ValidateUserDrinkInput();

                bool isAdmin = this.adminService.IsAdmin(this.UserId);

                string message;

                if (isAdmin)
                {
                    this.viewModel.InstantAddDrink();
                    message = "Drink added successfully.";
                }
                else
                {
                    this.viewModel.SendAddDrinkRequest();
                    message = "A request was sent to the admin.";
                }

                ContentDialog dialog = new ContentDialog
                {
                    Title = "Success",
                    Content = message,
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot,
                };
                _ = dialog.ShowAsync();

                this.viewModel.ClearForm();
            }
            catch (Exception exception)
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = exception.Message,
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot,
                };
                _ = dialog.ShowAsync();
            }
        }
    }
}