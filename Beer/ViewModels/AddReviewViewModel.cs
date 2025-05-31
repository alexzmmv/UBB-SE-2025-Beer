using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DTOModels;
using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
using WinUIApp.ViewHelpers;

namespace WinUIApp.ViewModels
{
    public class AddReviewViewModel : ViewModelBase
    {
        private const int BottleRatingToIndexOffset = 1;

        private ObservableCollection<BottleAsset> bottles;
        private int ratingScore;
        private string content;
        private IReviewService reviewService;
        private const int MINIMUM_RATING = 1;
        private const int MAXIMUM_RATING = 5;
        private const int ERROR_ON_VALIDATION = -2;

        public AddReviewViewModel(IReviewService reviewService)
        {
            this.reviewService = reviewService ?? throw new ArgumentNullException(nameof(reviewService));
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
            foreach (int currentRatingBottle in Enumerable.Range(AddReviewViewModel.MINIMUM_RATING, AddReviewViewModel.MAXIMUM_RATING))
            {
                int bottleIndex = currentRatingBottle - BottleRatingToIndexOffset;
                this.Bottles[bottleIndex].ImageSource = currentRatingBottle <= clickedBottleNumber
                    ? AssetConstants.FilledBottlePath
                    : AssetConstants.EmptyBottlePath;
            }

            this.RatingScore = clickedBottleNumber;
        }

        public virtual async Task<int> AddReview(int drinkId)
        {
            if (this.RatingScore < AddReviewViewModel.MINIMUM_RATING)
            {
                return ERROR_ON_VALIDATION;
            }

            ReviewDTO newReview = new ReviewDTO
            {
                ReviewId = 0,
                DrinkId = drinkId,
                Content = Content,
                RatingValue = this.RatingScore,
                UserId = App.CurrentUserId,
                IsHidden = true,
                CreatedDate = DateTime.UtcNow,
                NumberOfFlags = 0,
            };

            return await this.reviewService.AddReview(newReview);
        }

        protected virtual void InitializeBottles()
        {
            this.Bottles.Clear();

            // Create exactly 5 bottles (for 1-5 rating scale)
            for (int i = MINIMUM_RATING; i <= MAXIMUM_RATING; i++)
            {
                this.Bottles.Add(new BottleAsset
                {
                    ImageSource = AssetConstants.EmptyBottlePath
                });
            }
        }
    }
}