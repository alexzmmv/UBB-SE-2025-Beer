// <copyright file="RatingViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using WinUIApp.ViewHelpers;
    using WinUIApp.ViewModels;
    using System.Diagnostics;
    using WinUIApp.ProxyServices;
    using WinUIApp.ProxyServices.Constants;
    using WinUIApp.ProxyServices.Models;

    /// <summary>
    /// ViewModel for managing ratings and associated bottle assets.
    /// </summary>
    public class RatingViewModel : ViewModelBase
    {

        private const int BottleRatingToIndexOffset = 1;
        private const int RatingsCountToUserOffset = 1;
        private const int PlaceholderItemId = 100;

        private readonly IRatingService ratingService;
        private ObservableCollection<Rating> ratings;
        private Rating? selectedRating;
        private double averageRating;
        private ObservableCollection<BottleAsset> bottles;
        private int ratingScore;

        /// <summary>
        /// Initializes a new instance of the <see cref="RatingViewModel"/> class.
        /// </summary>
        /// <param name="ratingService">The service used to manage ratings.</param>
        public RatingViewModel(IRatingService ratingService)
        {
            this.ratingService = ratingService ?? throw new ArgumentNullException(nameof(ratingService));
            this.ratings = new ObservableCollection<Rating>();
            this.bottles = new ObservableCollection<BottleAsset>();
            this.InitializeBottles();

        }

        /// <summary>
        /// Gets or sets the collection of ratings.
        /// </summary>
        public virtual ObservableCollection<Rating> Ratings
        {
            get => this.ratings;
            set => this.SetProperty(ref this.ratings, value);
        }

        /// <summary>
        /// Gets or sets the selected rating.
        /// </summary>
        public virtual Rating? SelectedRating
        {
            get => this.selectedRating;
            set => this.SetProperty(ref this.selectedRating, value);
        }

        /// <summary>
        /// Gets or sets the average rating value, rounded to two decimal places.
        /// </summary>
        public virtual double AverageRating
        {
            get => this.averageRating;
            set => this.SetProperty(ref this.averageRating, Math.Round(value, 2));
        }

        /// <summary>
        /// Gets or sets the collection of bottle assets.
        /// </summary>
        public virtual ObservableCollection<BottleAsset> Bottles
        {
            get => this.bottles;
            set => this.SetProperty(ref this.bottles, value);
        }

        /// <summary>
        /// Gets or sets the rating score.
        /// </summary>
        public virtual int RatingScore
        {
            get => this.ratingScore;
            set => this.SetProperty(ref this.ratingScore, value);
        }

        /// <summary>
        /// Updates the bottle ratings based on the clicked bottle number.
        /// </summary>
        /// <param name="clickedBottleNumber">The number of the clicked bottle.</param>
        public virtual void UpdateBottleRating(int clickedBottleNumber)
        {
            foreach (var currentRatingBottle in Enumerable.Range(RatingDomainConstants.MinRatingValue, RatingDomainConstants.MaxRatingValue))
            {
                var bottleIndex = currentRatingBottle - BottleRatingToIndexOffset;
                this.Bottles[bottleIndex].ImageSource = currentRatingBottle <= clickedBottleNumber
                    ? AssetConstants.FilledBottlePath
                    : AssetConstants.EmptyBottlePath;
            }

            this.RatingScore = clickedBottleNumber;
        }

        /// <summary>
        /// Adds a new rating based on the current rating score.
        /// </summary>
        public virtual void AddRating(int productId)
        {
            if (this.RatingScore < RatingDomainConstants.MinRatingValue)
            {
                return;
            }

            var rating = new Rating
            {
                DrinkId = productId,  // Use the passed parameter instead of this.ProductID
                RatingValue = this.RatingScore,
                RatingDate = DateTime.Now,
                UserId = this.GetUserId(),
            };

            this.ratingService.CreateRating(rating);
            this.LoadRatingsForProduct(productId);  // Use the same productId here
        }

        /// <summary>
        /// Loads ratings for a specific product identified by its ID.
        /// </summary>
        /// <param name="productId">The ID of the product whose ratings are to be loaded.</param>
        public virtual void LoadRatingsForProduct(int productId)
        {
            Debug.WriteLine($"Loading ratings for product {productId}");

            var ratingsForProduct = this.ratingService.GetRatingsByProduct(productId);
            Debug.WriteLine($"Service returned {ratingsForProduct.Count()} ratings");

            var ratingsOrderedByNewest = ratingsForProduct.Reverse();

            this.Ratings.Clear();
            foreach (var rating in ratingsOrderedByNewest)
            {
                Debug.WriteLine($"Adding rating: {rating.RatingValue} by User {rating.UserId}");
                this.Ratings.Add(rating);
            }

            double avg = this.ratingService.GetAverageRating(productId);
            Debug.WriteLine($"Calculated average: {avg}");
            this.AverageRating = avg;
        }

        /// <summary>
        /// Initializes the bottles collection with empty bottle assets.
        /// </summary>
        protected virtual void InitializeBottles()
        {
            // Clear existing bottles first
            this.Bottles.Clear();

            // Create exactly 5 bottles (for 1-5 rating scale)
            for (int i = 0; i < 5; i++)
            {
                this.Bottles.Add(new BottleAsset
                {
                    ImageSource = AssetConstants.EmptyBottlePath
                });
            }

            Debug.WriteLine($"Initialized {this.Bottles.Count} bottles"); // Should output 5
        }

        private int GetUserId()
        {
            return this.Ratings.Count + RatingsCountToUserOffset;
        }
    }
}