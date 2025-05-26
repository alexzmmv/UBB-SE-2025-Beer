// <copyright file="ReviewWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Views.Windows
{
    using System;
    using WinUIApp.ViewModels;
    using Microsoft.Extensions.Configuration;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;

    /// <summary>
    /// A window for submitting or generating a review.
    /// </summary>
    public sealed partial class ReviewWindow : Window
    {
        private readonly IConfiguration configuration;
        private readonly RatingViewModel ratingViewModel;
        private readonly ReviewViewModel reviewViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReviewWindow"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="ratingViewModel">The rating view model.</param>
        /// <param name="reviewViewModel">The review view model.</param>
        public ReviewWindow(IConfiguration configuration, RatingViewModel ratingViewModel, ReviewViewModel reviewViewModel)
        {
            this.configuration = configuration;
            this.ratingViewModel = ratingViewModel;
            this.reviewViewModel = reviewViewModel;

            this.InitializeComponent();
            this.rootGrid.DataContext = reviewViewModel;
            this.reviewViewModel.RequestClose += this.CloseWindow;
        }

        /// <summary>
        /// Handles the window close event.
        /// </summary>
        /// <param name="sender">Sender object of the event.</param>
        /// <param name="e">Parameters sent to event.</param>
        public void CloseWindow(object? sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Handles the Submit Review button click event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        private async void SubmitReview_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Debug to verify method is being called
                System.Diagnostics.Debug.WriteLine("SubmitReview_Click method called");

                // Check if review content is empty
                if (string.IsNullOrWhiteSpace(this.reviewViewModel.ReviewContent))
                {
                    System.Diagnostics.Debug.WriteLine("Review content is empty");
                    await this.EmptyReviewDialog.ShowAsync();
                    return;
                }

                // Check if rating is selected
                if (this.ratingViewModel.SelectedRating == null)
                {
                    System.Diagnostics.Debug.WriteLine("No rating is selected");

                    ContentDialog noRatingDialog = new ContentDialog
                    {
                        XamlRoot = this.Content.XamlRoot,
                        Title = "No Rating Selected",
                        Content = "Please select a rating before submitting your review.",
                        CloseButtonText = "OK"
                    };

                    await noRatingDialog.ShowAsync();
                    return;
                }

                // If we get here, we have both a rating and review content
                System.Diagnostics.Debug.WriteLine($"Adding review with RatingId: {this.ratingViewModel.SelectedRating.RatingId}");
                this.reviewViewModel.AddReview(this.ratingViewModel.SelectedRating.RatingId);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                // Log SQL exceptions
                System.Diagnostics.Debug.WriteLine($"SQL Exception: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Error Number: {ex.Number}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");

                ContentDialog errorDialog = new ContentDialog
                {
                    XamlRoot = this.Content.XamlRoot,
                    Title = "Database Error",
                    Content = $"An error occurred while connecting to the database: {ex.Message}",
                    CloseButtonText = "OK"
                };

                await errorDialog.ShowAsync();
            }
            catch (Exception ex)
            {
                // Log general exceptions
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");

                ContentDialog errorDialog = new ContentDialog
                {
                    XamlRoot = this.Content.XamlRoot,
                    Title = "Error",
                    Content = $"An error occurred: {ex.Message}",
                    CloseButtonText = "OK"
                };

                await errorDialog.ShowAsync();
            }
        }

        /// <summary>
        /// Handles the Generate AI Review button click event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        private void GenerateAIReview_Click(object sender, RoutedEventArgs e)
        {
            var aiReviewWindow = new AIReviewWindow(this.configuration, this.OnAIReviewGenerated);
            aiReviewWindow.Activate();
        }

        /// <summary>
        /// Callback when an AI-generated review is created.
        /// </summary>
        /// <param name="aiReview">The generated review text.</param>
        private void OnAIReviewGenerated(string aiReview)
        {
            this.reviewViewModel.ReviewContent = aiReview;
        }
    }
}