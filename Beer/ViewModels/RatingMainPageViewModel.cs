namespace WinUIApp.ViewModels
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.UI.Xaml.Controls;
    using WinUiApp.Data.Data;
    using WinUIApp.ProxyServices.Models;

    public class RatingMainPageViewModel : ViewModelBase
    {
        private const int MinimumValidIndex = 0;
        private const int InvalidSelectionIndex = -1;

        private readonly IConfiguration configuration;
        private RatingViewModel ratingViewModel;
        private ReviewViewModel reviewViewModel;
        private int productId;

        public RatingMainPageViewModel(IConfiguration configuration, RatingViewModel ratingViewModel,
            ReviewViewModel reviewViewModel, int productId)
        {
            this.configuration = configuration;
            this.ratingViewModel = ratingViewModel;
            this.reviewViewModel = reviewViewModel;
            this.productId = productId;

            this.InitializeData();
        }

        public IConfiguration Configuration => this.configuration;

        public RatingViewModel RatingViewModel
        {
            get => this.ratingViewModel;
            set => this.SetProperty(ref this.ratingViewModel, value);
        }

        public ReviewViewModel ReviewViewModel
        {
            get => this.reviewViewModel;
            set => this.SetProperty(ref this.reviewViewModel, value);
        }

        public Rating? SelectedRating => this.ratingViewModel.SelectedRating;

        public void HandleRatingSelection(ListView listView)
        {
            this.HandleRatingSelectionInternal(listView?.SelectedIndex ?? InvalidSelectionIndex);
        }

        public void ClearSelectedRating()
        {
            this.ratingViewModel.SelectedRating = null!;
        }

        internal void HandleRatingSelectionInternal(int selectedIndex)
        {
            if (selectedIndex >= MinimumValidIndex && selectedIndex < this.ratingViewModel.Ratings.Count)
            {
                Rating selectedRating = this.ratingViewModel.Ratings[selectedIndex];
                this.ratingViewModel.SelectedRating = selectedRating;
                this.reviewViewModel.LoadReviewsForRatingAsync(selectedRating.RatingId);
            }
        }

        private void InitializeData()
        {
            this.ratingViewModel.LoadRatingsForProduct(this.productId);
        }
    }
}
