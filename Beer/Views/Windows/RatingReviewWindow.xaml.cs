// <copyright file="RatingReviewWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Views.Windows
{
    using System;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using WinUIApp.Views.Windows;
    using WinUIApp.ViewModels;
    using CommunityToolkit.WinUI;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents the main window of the application.
    /// </summary>
    public sealed partial class RatingReviewWindow : Window
    {
        private const int MaxContentLength = 500;
        private readonly RatingMainPageViewModel RatingMainPageViewModel;
        private readonly int productId; // Add productId field

        public RatingReviewWindow(RatingMainPageViewModel ratingMainPageViewModel, int productId)
        {
            if (ratingMainPageViewModel == null)
                throw new ArgumentNullException(nameof(ratingMainPageViewModel));
            if (ratingMainPageViewModel.RatingViewModel == null)
                throw new ArgumentException("RatingViewModel cannot be null", nameof(ratingMainPageViewModel));
            if (ratingMainPageViewModel.ReviewViewModel == null)
                throw new ArgumentException("ReviewViewModel cannot be null", nameof(ratingMainPageViewModel));

            this.InitializeComponent();
            this.RatingMainPageViewModel = ratingMainPageViewModel;
            this.productId = productId; // Store the productId
            this.rootGrid.DataContext = ratingMainPageViewModel;

            // Initialize ratings for this product
            ratingMainPageViewModel.RatingViewModel.LoadRatingsForProduct(productId);
        }

        /// <summary>
        /// Handles the click event for the Add Review button.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private async void AddReview_Click(object sender, RoutedEventArgs e)
        {
            if (this.RatingMainPageViewModel.SelectedRating != null)
            {
                var reviewWindow = new ReviewWindow(
                    this.RatingMainPageViewModel.Configuration,
                    this.RatingMainPageViewModel.RatingViewModel,
                    this.RatingMainPageViewModel.ReviewViewModel);
                reviewWindow.Activate();
            }
            else
            {
                await this.NoRatingSelectedDialog.ShowAsync();
            }
        }

        /// <summary>
        /// Handles the click event for the Add Rating button.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void AddRating_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.RatingMainPageViewModel == null)
                {
                    throw new InvalidOperationException("MainRatingReviewViewModel is null");
                }

                if (this.RatingMainPageViewModel.RatingViewModel == null)
                {
                    throw new InvalidOperationException("RatingViewModel is null");
                }

                this.RatingMainPageViewModel.ClearSelectedRating();
                // Pass the productId to RatingWindow constructor
                var ratingWindow = new RatingWindow(
                    this.RatingMainPageViewModel.RatingViewModel,
                    this.productId);
                ratingWindow.Activate();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in AddRating_Click: {ex}");
                _ = this.ShowErrorDialogAsync($"Failed to open rating window: {ex.Message}");
            }
        }

        private async Task ShowErrorDialogAsync(string message)
        {
            await this.DispatcherQueue.EnqueueAsync(async () =>
            {
                var dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = message,
                    CloseButtonText = "OK"
                };
                await dialog.ShowAsync();
            });
        }

        /// <summary>
        /// Handles the rating selection changed event.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="eventArguments">The event arguments.</param>
        private void RatingSelection_Changed(object sender, RoutedEventArgs eventArguments)
        {
            if (sender is ListView listView)
            {
                this.RatingMainPageViewModel.HandleRatingSelection(listView);
            }
        }
    }
}