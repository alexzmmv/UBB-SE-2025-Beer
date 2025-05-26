namespace WinUIApp.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using WinUIApp.ProxyServices;
    using WinUIApp.ProxyServices.Models;
    using WinUIApp.Services.DummyServices;

    public class ReviewViewModel : ViewModelBase
    {

        private readonly IReviewService reviewService;
        private ObservableCollection<Review> reviews;
        private Review? selectedReview;
        private string reviewContent = string.Empty;
        private int userId;

        public ReviewViewModel(IReviewService reviewService, IUserService userService)
        {
            this.reviewService = reviewService ?? throw new ArgumentNullException(nameof(reviewService));
            this.reviews = new ObservableCollection<Review>();
            this.userId = userService.CurrentUserId;
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

        public virtual void LoadReviewsForRating(int ratingId)
        {
            IEnumerable<Review> reviewsList = this.reviewService.GetReviewsByRating(ratingId);
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

            newReview.Activate(); // Use the Activate method to set IsActive and CreationDate

            try
            {
                this.reviewService.AddReview(newReview);
                this.LoadReviewsForRating(ratingId);
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