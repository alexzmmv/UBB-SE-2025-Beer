namespace WinUIApp.Views.Windows
{
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Input;
    using WinUIApp.ViewHelpers;
    using WinUIApp.ViewModels;

    public sealed partial class RatingWindow : Window
    {
        private const int BottleRatingToIndexOffset = 1;
        private readonly RatingViewModel ratingViewModel;
        private readonly int productId;

        public RatingWindow(RatingViewModel viewModel, int productId)
        {
            this.InitializeComponent();

            this.ratingViewModel = viewModel;
            this.productId = productId;
            this.rootGrid.DataContext = viewModel;
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

            int clickedBottleNumber = this.ratingViewModel.Bottles.IndexOf(clickedBottle) + BottleRatingToIndexOffset;
            this.ratingViewModel.UpdateBottleRating(clickedBottleNumber);
        }

        private void RateButton_Click(object sender, RoutedEventArgs e)
        {
            this.ratingViewModel.AddRating(this.productId);
            this.Close();
        }
    }
}
