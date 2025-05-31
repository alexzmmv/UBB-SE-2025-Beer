using DataAccess.DTOModels;
using DataAccess.IRepository;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.ReviewsServiceTests
{
    public class ReviewsServiceGetAverageRatingTest
    {
        private readonly Mock<IReviewsRepository> mockReviewsRepository;
        private readonly ReviewsService reviewsService;

        private const int DrinkIdWithRatings = 101;
        private const int DrinkIdWithoutRatings = 202;
        private const int DrinkIdWithNoReviews = 303;
        private const int DrinkIdThatThrows = 404;

        private readonly List<ReviewDTO> reviewsWithRatings;
        private readonly List<ReviewDTO> reviewsWithoutRatings;

        public ReviewsServiceGetAverageRatingTest()
        {
            mockReviewsRepository = new Mock<IReviewsRepository>();
            reviewsService = new ReviewsService(mockReviewsRepository.Object);

            reviewsWithRatings = new List<ReviewDTO>
            {
                new ReviewDTO { RatingValue = 4 },
                new ReviewDTO { RatingValue = 5 },
                new ReviewDTO { RatingValue = 3 }
            };

            reviewsWithoutRatings = new List<ReviewDTO>
            {
                new ReviewDTO { RatingValue = null },
                new ReviewDTO { RatingValue = null }
            };

            mockReviewsRepository
                .Setup(repo => repo.GetReviewsByDrinkId(DrinkIdWithRatings))
                .ReturnsAsync(reviewsWithRatings);

            mockReviewsRepository
                .Setup(repo => repo.GetReviewsByDrinkId(DrinkIdWithoutRatings))
                .ReturnsAsync(reviewsWithoutRatings);

            mockReviewsRepository
                .Setup(repo => repo.GetReviewsByDrinkId(DrinkIdWithNoReviews))
                .ReturnsAsync(new List<ReviewDTO>());

            mockReviewsRepository
                .Setup(repo => repo.GetReviewsByDrinkId(DrinkIdThatThrows))
                .ThrowsAsync(new Exception("Simulated error"));
        }

        [Fact]
        public async Task GetAverageRating_WithValidRatings_ReturnsCorrectAverage()
        {
            // Arrange
            double expectedAverage = 4.0;

            // Act
            double result = await reviewsService.GetAverageRating(DrinkIdWithRatings);

            // Assert
            Assert.Equal(expectedAverage, result);
        }

        [Fact]
        public async Task GetAverageRating_WithNoValidRatings_ReturnsZero()
        {
            // Act
            double result = await reviewsService.GetAverageRating(DrinkIdWithoutRatings);

            // Assert
            Assert.Equal(0.0, result);
        }

        [Fact]
        public async Task GetAverageRating_WithNoReviews_ReturnsZero()
        {
            // Act
            double result = await reviewsService.GetAverageRating(DrinkIdWithNoReviews);

            // Assert
            Assert.Equal(0.0, result);
        }

        [Fact]
        public async Task GetAverageRating_WhenRepositoryThrowsException_ReturnsZero()
        {
            // Act
            double result = await reviewsService.GetAverageRating(DrinkIdThatThrows);

            // Assert
            Assert.Equal(0.0, result);
        }
    }
}
