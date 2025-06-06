using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Constants;
using DataAccess.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using WinUiApp.Data.Data;
using WinUIApp.ViewModels;
using WinUIApp.WebAPI.Models;

namespace WinUIApp.Views.Components
{
    public sealed partial class UpdateDrinkFlyout : UserControl
    {
        private UpdateDrinkMenuViewModel viewModel;

        private bool isAdmin;

        private IDrinkService drinkService;

        private IUserService userService;

        private IDrinkModificationRequestService modificationRequestService;

        public UpdateDrinkFlyout()
        {
            this.Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
            this.InitializeComponent();
            this.drinkService = App.Host.Services.GetRequiredService<IDrinkService>();
            this.userService = App.Host.Services.GetRequiredService<IUserService>();
            this.modificationRequestService = App.Host.Services.GetRequiredService<IDrinkModificationRequestService>();

            this.Loaded += this.UpdateDrinkFlyout_LoadedAsync;
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
                    foreach (string category in filteredCategories)
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

        public DrinkDTO DrinkToUpdate { get; set; }

        public Guid UserId { get; set; }

        private async void UpdateDrinkFlyout_LoadedAsync(object sender, RoutedEventArgs eventArguments)
        {
            this.isAdmin = await this.userService.GetHighestRoleTypeForUser(App.CurrentUserId) == RoleType.Admin;

            List<Brand> allBrands = this.drinkService.GetDrinkBrandNames();
            List<Category> allCategories = this.drinkService.GetDrinkCategories();

            this.viewModel = new UpdateDrinkMenuViewModel(
                this.DrinkToUpdate,
                drinkService)
            {
                AllBrands = allBrands,
                AllCategoryObjects = allCategories,
                AllCategories = allCategories.Select(category => category.CategoryName).ToList(),
                BrandName = this.DrinkToUpdate.DrinkBrand?.BrandName ?? string.Empty,
            };

            foreach (Category category in this.DrinkToUpdate.CategoryList)
            {
                this.viewModel.SelectedCategoryNames.Add(category.CategoryName);
            }

            this.DataContext = this.viewModel;

            if (isAdmin)
            {
                this.SaveButton.Content = "Save";
            }
            else
            {
                this.SaveButton.Content = "Send Request to Admin";
            }

            this.CategoryList.ItemsSource = this.viewModel.AllCategories;

            foreach (string category in this.viewModel.AllCategories)
            {
                if (this.viewModel.SelectedCategoryNames.Contains(category))
                {
                    this.CategoryList.SelectedItems.Add(category);
                }
            }
        }

        private void CategoryList_SelectionChanged(object sender, SelectionChangedEventArgs eventArguments)
        {
            if (this.viewModel == null)
            {
                return;
            }

            foreach (string removedCategory in eventArguments.RemovedItems.Cast<string>())
            {
                this.viewModel.SelectedCategoryNames.Remove(removedCategory);
            }

            foreach (string addedCategory in eventArguments.AddedItems.Cast<string>())
            {
                if (!this.viewModel.SelectedCategoryNames.Contains(addedCategory))
                {
                    this.viewModel.SelectedCategoryNames.Add(addedCategory);
                }
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs eventArguments)
        {
            if (this.DrinkToUpdate == null)
            {
                return;
            }

            try
            {
                this.viewModel.ValidateUpdatedDrinkDetails();
                this.DrinkToUpdate.CategoryList = this.viewModel.GetSelectedCategories();

                this.isAdmin = await this.userService.GetHighestRoleTypeForUser(App.CurrentUserId) == RoleType.Admin;

                string message;

                if (this.isAdmin)
                {
                    this.viewModel.InstantUpdateDrink();
                    message = "Drink updated successfully.";
                }
                else
                {
                    this.viewModel.SendUpdateDrinkRequest();
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