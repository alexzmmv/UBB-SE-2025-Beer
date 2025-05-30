using DataAccess.Service.Interfaces;
using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinUiApp.Data.Data;
using WinUIApp.ViewHelpers;
using WinUIApp.ViewModels;
using WinUIApp.Views.ViewModels;
using WinUIApp.WebAPI.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUIApp.Views.Components.Modals
{
    public sealed partial class AddReviewModal : UserControl
    {

        private const int BottleRatingToIndexOffset = 1;
        public event EventHandler CloseRequested;
        private IReviewService reviewService;
        public RatingViewModel ViewModel { get; }
        public AddReviewModal()
        {
            this.InitializeComponent();
            this.reviewService = App.Host.Services.GetRequiredService<IReviewService>();
            this.ViewModel = new RatingViewModel(reviewService);
            this.DataContext = this.ViewModel;
        }

        public int DrinkId
        {
            get => (int)GetValue(DrinkIdProperty);
            set => SetValue(DrinkIdProperty, value);
        }

        public static readonly DependencyProperty DrinkIdProperty =
            DependencyProperty.Register(nameof(DrinkId), typeof(int), typeof(AddReviewModal),
                new PropertyMetadata(0));


        private void SaveReviewButton_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.AddReview(DrinkId);
            CloseRequested?.Invoke(this, EventArgs.Empty);
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

            int clickedBottleNumber = this.ViewModel.Bottles.IndexOf(clickedBottle) + BottleRatingToIndexOffset;
            this.ViewModel.UpdateBottleRating(clickedBottleNumber);
        }
    }
}
