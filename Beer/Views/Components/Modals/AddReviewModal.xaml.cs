using System;
using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using WinUIApp.ViewHelpers;
using WinUIApp.ViewModels;

namespace WinUIApp.Views.Components.Modals
{
    public sealed partial class AddReviewModal : UserControl
    {
        private const int BOTTLE_RATING_TO_INDEX_OFFSET = 1;
        private const int INVALID_REVIEW_ID = 0;
        public event EventHandler CloseRequested;
        public event EventHandler RefreshReviewsRequested;
        private IReviewService reviewService;
        public AddReviewViewModel ViewModel { get; }
        public AddReviewModal()
        {
            this.InitializeComponent();
            this.reviewService = App.Host.Services.GetRequiredService<IReviewService>();
            this.ViewModel = new AddReviewViewModel(reviewService);
        }

        public int DrinkId
        {
            get => (int)GetValue(AddReviewModal.DrinkIdProperty);
            set => SetValue(AddReviewModal.DrinkIdProperty, value);
        }

        public static readonly DependencyProperty DrinkIdProperty =
                   DependencyProperty.Register(
                       "DrinkId",
                       typeof(int),
                       typeof(AddReviewModal),
                       new PropertyMetadata(0));

        private async void SaveReviewButton_Click(object sender, RoutedEventArgs e)
        {
            int newReviewId = await this.ViewModel.AddReview(DrinkId);
            if (newReviewId <= AddReviewModal.INVALID_REVIEW_ID)
            {
                return;
            }
            else
            {
                RefreshReviewsRequested?.Invoke(this, EventArgs.Empty);
                CloseRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        private void CancelReviewButton_Click(object sender, RoutedEventArgs e)
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }
        private void Bottle_Click(object sender, TappedRoutedEventArgs e)
        {
            if (sender is not Image clickedImage)
            {
                return;
            }

            if (clickedImage.DataContext is not BottleAsset clickedBottle)
            {
                return;
            }

            int clickedBottleNumber = this.ViewModel.Bottles.IndexOf(clickedBottle) + AddReviewModal.BOTTLE_RATING_TO_INDEX_OFFSET;
            this.ViewModel.UpdateBottleRating(clickedBottleNumber);
        }
    }
}
