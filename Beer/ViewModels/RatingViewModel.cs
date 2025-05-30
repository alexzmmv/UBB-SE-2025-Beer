using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
using WinUiApp.Data.Data;
using WinUIApp.ProxyServices;
using WinUIApp.ProxyServices.Models;
using WinUIApp.ViewHelpers;

namespace WinUIApp.ViewModels
{
    public class RatingViewModel : ViewModelBase
    {
        private const int BottleRatingToIndexOffset = 1;
        private const int RatingsCountToUserOffset = 1;

        private ObservableCollection<float> ratings;
        private float? selectedRating;
        private double averageRating;
        private ObservableCollection<BottleAsset> bottles;
        private int ratingScore;
        private string content;
        private IReviewService reviewService;
        private const int MINIMUM_RATING = 1;
        private const int MAXIMUM_RATING = 5;

        public RatingViewModel(IReviewService reviewService)
        {
            this.reviewService = reviewService ?? throw new ArgumentNullException(nameof(reviewService));
            this.ratings = new ObservableCollection<float>();
            this.bottles = new ObservableCollection<BottleAsset>();
            this.InitializeBottles();
        }


        public string Content
        {
            get => content;
            set
            {
                if (content != value)
                {
                    content = value;
                    OnPropertyChanged();
                }
            }
        }

        public virtual ObservableCollection<float> Ratings
        {
            get => this.ratings;
            set => this.SetProperty(ref this.ratings, value);
        }

        public virtual float? SelectedRating
        {
            get => this.selectedRating;
            set => this.SetProperty(ref this.selectedRating, value);
        }

        public virtual double AverageRating
        {
            get => this.averageRating;
            set => this.SetProperty(ref this.averageRating, Math.Round(value, 2));
        }
        public virtual ObservableCollection<BottleAsset> Bottles
        {
            get => this.bottles;
            set => this.SetProperty(ref this.bottles, value);
        }

        public virtual int RatingScore
        {
            get => this.ratingScore;
            set => this.SetProperty(ref this.ratingScore, value);
        }

        public virtual void UpdateBottleRating(int clickedBottleNumber)
        {
            foreach (int currentRatingBottle in Enumerable.Range(RatingViewModel.MINIMUM_RATING, RatingViewModel.MAXIMUM_RATING))
            {
                int bottleIndex = currentRatingBottle - BottleRatingToIndexOffset;
                this.Bottles[bottleIndex].ImageSource = currentRatingBottle <= clickedBottleNumber
                    ? AssetConstants.FilledBottlePath
                    : AssetConstants.EmptyBottlePath;
            }

            this.RatingScore = clickedBottleNumber;
        }

        public virtual void AddReview(int drinkId)
        {
            if (this.RatingScore < RatingViewModel.MINIMUM_RATING)
            {
                return;
            }

            Review newReview = new Review
            {
                ReviewId = 0,
                DrinkId = drinkId,
                Content = Content,
                RatingValue = this.RatingScore,
                UserId = App.CurrentUserId,
                User = new User(),
                Drink = new Drink(),
                IsActive = false,
                IsHidden = false,
                CreatedDate = DateTime.UtcNow,
                NumberOfFlags = 0,
            };

            this.reviewService.AddReview(newReview);
            //this.LoadRatingsForProduct(productId);
        }

        public virtual void LoadRatingsForProduct(int productId)
        {
            //IEnumerable<Rating> ratingsForProduct = this.ratingService.GetRatingsByDrink(productId);

            //IEnumerable<Rating> ratingsOrderedByNewest = ratingsForProduct.Reverse();

            //this.Ratings.Clear();
            //foreach (var rating in ratingsOrderedByNewest)
            //{
            //    this.Ratings.Add(rating);
            //}

            //double avg = this.ratingService.GetAverageRating(productId);
            //this.AverageRating = avg;
        }

        protected virtual void InitializeBottles()
        {
            this.Bottles.Clear();

            // Create exactly 5 bottles (for 1-5 rating scale)
            for (int i = 0; i < 5; i++)
            {
                this.Bottles.Add(new BottleAsset
                {
                    ImageSource = AssetConstants.EmptyBottlePath
                });
            }
        }

        private int GetUserId()
        {
            return this.Ratings.Count + RatingsCountToUserOffset;
        }
    }
}