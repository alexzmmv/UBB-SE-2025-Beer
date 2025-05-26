// <copyright file="RatingWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Views.Windows
{
    using WinUIApp.ViewHelpers;
    using WinUIApp.ViewModels;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Input;
    using System.Diagnostics;
    using System.Linq;

    public sealed partial class RatingWindow : Window
    {
        private const int BottleRatingToIndexOffset = 1;
        private readonly RatingViewModel ratingViewModel;
        private readonly int productId; // Add this field to store the product ID

        /// <summary>
        /// Initializes a new instance of the <see cref="RatingWindow"/> class.
        /// </summary>
        /// <param name="viewModel">The rating view model.</param>
        /// <param name="productId">The ID of the product being rated.</param>
        public RatingWindow(RatingViewModel viewModel, int productId)
        {
            this.InitializeComponent();
            Debug.WriteLine($"Bottle count: {viewModel.Bottles?.Count ?? 0}");
            foreach (var bottle in viewModel.Bottles ?? Enumerable.Empty<BottleAsset>())
            {
                Debug.WriteLine($"Bottle image source: {bottle.ImageSource}");
            }

            this.ratingViewModel = viewModel;
            this.productId = productId; // Store the product ID
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
            this.ratingViewModel.AddRating(this.productId); // Pass the productId to AddRating
            this.Close();
        }
    }
}
