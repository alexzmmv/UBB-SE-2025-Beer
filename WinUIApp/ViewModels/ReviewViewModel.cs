// <copyright file="ReviewViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using Windows.Security.Authentication.OnlineId;
    using WinUIApp.ProxyServices;
    using WinUIApp.ProxyServices.Models;
    using WinUIApp.Services;
    using WinUIApp.Services;
    using WinUIApp.Services.DummyServices;
    using WinUIApp.ViewModels;

    /// <summary>
    /// ViewModel for managing reviews associated with ratings.
    /// </summary>
    public class ReviewViewModel : ViewModelBase
    {

        private readonly IReviewService reviewService;
        private ObservableCollection<Review> reviews;
        private Review? selectedReview;
        private string reviewContent = string.Empty;
        private int userId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReviewViewModel"/> class.
        /// </summary>
        /// <param name="reviewService">The service used to manage reviews.</param>
        public ReviewViewModel(IReviewService reviewService, IUserService userService)
        {
            this.reviewService = reviewService ?? throw new ArgumentNullException(nameof(reviewService));
            this.reviews = new ObservableCollection<Review>();
            this.userId = userService.CurrentUserId;
        }

        /// <summary>
        /// Event triggered when the window should be closed.
        /// </summary>
        public event EventHandler? RequestClose;

        /// <summary>
        /// Gets or sets the collection of reviews.
        /// </summary>
        public virtual ObservableCollection<Review> Reviews
        {
            get => this.reviews;
            set => this.SetProperty(ref this.reviews, value);
        }

        /// <summary>
        /// Gets or sets the selected review.
        /// </summary>
        public virtual Review? SelectedReview
        {
            get => this.selectedReview;
            set => this.SetProperty(ref this.selectedReview, value);
        }

        /// <summary>
        /// Gets or sets the content of the review.
        /// </summary>
        public virtual string ReviewContent
        {
            get => this.reviewContent;
            set => this.SetProperty(ref this.reviewContent, value);
        }

        /// <summary>
        /// Loads reviews for a specific rating based on its ID.
        /// </summary>
        /// <param name="ratingId">The ID of the rating whose reviews are to be loaded.</param>
        public virtual void LoadReviewsForRating(int ratingId)
        {
            var reviewsList = this.reviewService.GetReviewsByRating(ratingId);
            this.Reviews.Clear();
            foreach (var review in reviewsList)
            {
                this.Reviews.Add(review);
            }
        }

        /// <summary>
        /// Adds a new review for a specified rating.
        /// </summary>
        /// <param name="ratingId">The ID of the rating to which the review is to be added.</param>
        public virtual void AddReview(int ratingId)
        {
            if (string.IsNullOrWhiteSpace(this.ReviewContent))
            {
                return;
            }

            var newReview = new Review
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
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine($"Exception in AddReview: {exception.Message}");
                throw; // Re-throw to let the calling method handle it
            }
        }

        /// <summary>
        /// Clears the content of the current review.
        /// </summary>
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