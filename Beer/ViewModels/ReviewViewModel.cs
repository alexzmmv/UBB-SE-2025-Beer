namespace WinUIApp.ViewModels
{
    using DataAccess.Service.Interfaces;
    using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using WinUiApp.Data.Data;
    using WinUIApp.ProxyServices;
    using WinUIApp.ProxyServices.Models;

    public class ReviewViewModel : ViewModelBase
    {

        private readonly IReviewService reviewService;
        private ObservableCollection<Review> reviews;
        private Review? selectedReview;
        private string reviewContent = string.Empty;
        private Guid userId;

        public ReviewViewModel(IReviewService reviewService, IUserService userService)
        {
            this.reviewService = reviewService ?? throw new ArgumentNullException(nameof(reviewService));
            this.reviews = new ObservableCollection<Review>();
            this.userId = App.CurrentUserId;
        }

        public event EventHandler? RequestClose;

        public virtual ObservableCollection<Review> Reviews
        {
            get => this.reviews;
            set => this.SetProperty(ref this.reviews, value);
        }

        public virtual Review? SelectedReview
        {
            get => this.selectedReview;
            set => this.SetProperty(ref this.selectedReview, value);
        }

        public virtual string ReviewContent
        {
            get => this.reviewContent;
            set => this.SetProperty(ref this.reviewContent, value);
        }

        public virtual async void LoadReviewsForRatingAsync(int ratingId)
        {
            IEnumerable<Review> reviewsList = await this.reviewService.GetReviewsByRating(ratingId);
            this.Reviews.Clear();
            foreach (Review review in reviewsList)
            {
                this.Reviews.Add(review);
            }
        }

        public virtual void AddReview(int ratingId)
        {
            if (string.IsNullOrWhiteSpace(this.ReviewContent))
            {
                return;
            }

            Review newReview = new Review
            {
                RatingId = ratingId,
                UserId = this.userId,
                Content = this.ReviewContent
            };

            newReview.IsActive = true; // Use the Activate method to set IsActive and CreationDate
            newReview.CreatedDate = DateTime.Now;

            try
            {
                this.reviewService.AddReview(newReview);
                this.LoadReviewsForRatingAsync(ratingId);
                this.ReviewContent = string.Empty;
                this.CloseWindow();
            }
            catch
            {
                throw;
            }
        }

        public virtual void ClearReviewContent()
        {
            this.ReviewContent = string.Empty;
        }

        private void CloseWindow()
        {
            this.RequestClose?.Invoke(this, EventArgs.Empty);
        }
    }
}