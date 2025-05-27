using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WinUiApp.Data.Data;
using WinUIApp.ProxyServices;
using WinUIApp.ProxyServices.Constants;
using WinUIApp.ProxyServices.Models;
using WinUIApp.ViewHelpers;

namespace WinUIApp.ViewModels
{
    public class RatingViewModel : ViewModelBase
    {
        private const int BottleRatingToIndexOffset = 1;
        private const int RatingsCountToUserOffset = 1;

        private readonly IRatingService ratingService;
        private ObservableCollection<Rating> ratings;
        private Rating? selectedRating;
        private double averageRating;
        private ObservableCollection<BottleAsset> bottles;
        private int ratingScore;

        public RatingViewModel(IRatingService ratingService)
        {
            this.ratingService = ratingService ?? throw new ArgumentNullException(nameof(ratingService));
            this.ratings = new ObservableCollection<Rating>();
            this.bottles = new ObservableCollection<BottleAsset>();
            this.InitializeBottles();
        }

        public virtual ObservableCollection<Rating> Ratings
        {
            get => this.ratings;
            set => this.SetProperty(ref this.ratings, value);
        }

        public virtual Rating? SelectedRating
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
            foreach (int currentRatingBottle in Enumerable.Range(RatingDomainConstants.MinRatingValue, RatingDomainConstants.MaxRatingValue))
            {
                int bottleIndex = currentRatingBottle - BottleRatingToIndexOffset;
                this.Bottles[bottleIndex].ImageSource = currentRatingBottle <= clickedBottleNumber
                    ? AssetConstants.FilledBottlePath
                    : AssetConstants.EmptyBottlePath;
            }

            this.RatingScore = clickedBottleNumber;
        }

        public virtual void AddRating(int productId)
        {
            if (this.RatingScore < RatingDomainConstants.MinRatingValue)
            {
                return;
            }

            Rating rating = new Rating
            {
                DrinkId = productId,
                RatingValue = this.RatingScore,
                RatingDate = DateTime.Now,
                UserId = this.GetUserId(),
            };

            this.ratingService.CreateRating(rating);
            this.LoadRatingsForProduct(productId);
        }

        public virtual void LoadRatingsForProduct(int productId)
        {
            IEnumerable<Rating> ratingsForProduct = this.ratingService.GetRatingsByDrink(productId);

            IEnumerable<Rating> ratingsOrderedByNewest = ratingsForProduct.Reverse();

            this.Ratings.Clear();
            foreach (var rating in ratingsOrderedByNewest)
            {
                this.Ratings.Add(rating);
            }

            double avg = this.ratingService.GetAverageRating(productId);
            this.AverageRating = avg;
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