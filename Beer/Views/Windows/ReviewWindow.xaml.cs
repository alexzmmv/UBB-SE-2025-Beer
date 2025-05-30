namespace WinUIApp.Views.Windows
{
    using System;
    using WinUIApp.ViewModels;
    using Microsoft.Extensions.Configuration;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;

    public sealed partial class ReviewWindow : Window
    {
        private readonly IConfiguration configuration;
        private readonly RatingViewModel ratingViewModel;
        private readonly ReviewViewModel reviewViewModel;

        public ReviewWindow(IConfiguration configuration, RatingViewModel ratingViewModel, ReviewViewModel reviewViewModel)
        {
            this.configuration = configuration;
            this.ratingViewModel = ratingViewModel;
            this.reviewViewModel = reviewViewModel;

            this.InitializeComponent();
            this.rootGrid.DataContext = reviewViewModel;
            this.reviewViewModel.RequestClose += this.CloseWindow;
        }

        public void CloseWindow(object? sender, EventArgs e)
        {
            this.Close();
        }

        private async void SubmitReview_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.reviewViewModel.ReviewContent))
                {
                    await this.EmptyReviewDialog.ShowAsync();
                    return;
                }

                if (this.ratingViewModel.SelectedRating == null)
                {
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

                //this.reviewViewModel.AddReview(this.ratingViewModel.SelectedRating.RatingId);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
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

        private void GenerateAIReview_Click(object sender, RoutedEventArgs e)
        {
            AIReviewWindow aiReviewWindow = new AIReviewWindow(this.configuration, this.OnAIReviewGenerated);
            aiReviewWindow.Activate();
        }

        private void OnAIReviewGenerated(string aiReview)
        {
            this.reviewViewModel.ReviewContent = aiReview;
        }
    }
}