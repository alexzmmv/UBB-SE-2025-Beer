namespace WinUIApp.Views.Windows
{
    using System;
    using System.Threading.Tasks;
    using CommunityToolkit.WinUI;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using WinUIApp.ViewModels;

    public sealed partial class RatingReviewWindow : Window
    {
        private readonly RatingMainPageViewModel ratingMainPageViewModel;
        private readonly int productId;

        public RatingReviewWindow(RatingMainPageViewModel ratingMainPageViewModel, int productId)
        {
            if (ratingMainPageViewModel == null)
            {
                throw new ArgumentNullException(nameof(ratingMainPageViewModel));
            }
            if (ratingMainPageViewModel.RatingViewModel == null)
            {
                throw new ArgumentException("RatingViewModel cannot be null", nameof(ratingMainPageViewModel));
            }
            if (ratingMainPageViewModel.ReviewViewModel == null)
            {
                throw new ArgumentException("ReviewViewModel cannot be null", nameof(ratingMainPageViewModel));
            }

            this.InitializeComponent();
            this.ratingMainPageViewModel = ratingMainPageViewModel;
            this.productId = productId;
            this.rootGrid.DataContext = ratingMainPageViewModel;

            ratingMainPageViewModel.RatingViewModel.LoadRatingsForProduct(productId);
        }

        private async void AddReview_Click(object sender, RoutedEventArgs e)
        {
            /*if (this.ratingMainPageViewModel.SelectedRating != null)
            {
                ReviewWindow reviewWindow = new ReviewWindow(
                    this.ratingMainPageViewModel.Configuration,
                    this.ratingMainPageViewModel.RatingViewModel,
                    this.ratingMainPageViewModel.ReviewViewModel);
                reviewWindow.Activate();
            }
            else
            {
                await this.NoRatingSelectedDialog.ShowAsync();
            }*/
        }

        private void AddRating_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.ratingMainPageViewModel == null)
                {
                    throw new InvalidOperationException("MainRatingReviewViewModel is null");
                }

                if (this.ratingMainPageViewModel.RatingViewModel == null)
                {
                    throw new InvalidOperationException("RatingViewModel is null");
                }

                this.ratingMainPageViewModel.ClearSelectedRating();
                RatingWindow ratingWindow = new RatingWindow(
                    this.ratingMainPageViewModel.RatingViewModel,
                    this.productId);
                ratingWindow.Activate();
            }
            catch (Exception ex)
            {
                _ = this.ShowErrorDialogAsync($"Failed to open rating window: {ex.Message}");
            }
        }

        private async Task ShowErrorDialogAsync(string message)
        {
            await this.DispatcherQueue.EnqueueAsync(async () =>
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = message,
                    CloseButtonText = "OK"
                };
                await dialog.ShowAsync();
            });
        }

        private void RatingSelection_Changed(object sender, RoutedEventArgs eventArguments)
        {
            if (sender is ListView listView)
            {
                this.ratingMainPageViewModel.HandleRatingSelection(listView);
            }
        }
    }
}