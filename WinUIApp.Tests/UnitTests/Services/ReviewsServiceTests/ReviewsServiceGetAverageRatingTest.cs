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

        private const int DRINK_ID_WITH_RATINGS = 101;
        private const int DRINK_ID_WITHOUT_RATINGS = 202;
        private const int DRINK_ID_WITH_NO_REVIEWS = 303;
        private const int DRINK_ID_THAT_THROWS = 404;

        private readonly List<ReviewDTO> reviewsWithRatings;
        private readonly List<ReviewDTO> reviewsWithoutRatings;

        public ReviewsServiceGetAverageRatingTest()
        {
            this.mockReviewsRepository = new Mock<IReviewsRepository>();
            this.reviewsService = new ReviewsService(this.mockReviewsRepository.Object);

            this.reviewsWithRatings = new List<ReviewDTO>
            {
                new ReviewDTO { RatingValue = 4 },
                new ReviewDTO { RatingValue = 5 },
                new ReviewDTO { RatingValue = 3 }
            };

            this.reviewsWithoutRatings = new List<ReviewDTO>
            {
                new ReviewDTO { RatingValue = null },
                new ReviewDTO { RatingValue = null }
            };

            this.mockReviewsRepository
                .Setup(repo => repo.GetReviewsByDrinkId(DRINK_ID_WITH_RATINGS))
                .ReturnsAsync(this.reviewsWithRatings);

            this.mockReviewsRepository
                .Setup(repo => repo.GetReviewsByDrinkId(DRINK_ID_WITHOUT_RATINGS))
                .ReturnsAsync(this.reviewsWithoutRatings);

            this.mockReviewsRepository
                .Setup(repo => repo.GetReviewsByDrinkId(DRINK_ID_WITH_NO_REVIEWS))
                .ReturnsAsync(new List<ReviewDTO>());

            this.mockReviewsRepository
                .Setup(repo => repo.GetReviewsByDrinkId(DRINK_ID_THAT_THROWS))
                .ThrowsAsync(new Exception("Simulated error"));
        }

        [Fact]
        public async Task GetAverageRating_WithValidRatings_ReturnsCorrectAverage()
        {
            // Arrange
            double expectedAverage = 4.0;

            // Act
            double result = await this.reviewsService.GetAverageRating(DRINK_ID_WITH_RATINGS);

            // Assert
            Assert.Equal(expectedAverage, result);
        }

        [Fact]
        public async Task GetAverageRating_WithNoValidRatings_ReturnsZero()
        {
            // Act
            double result = await this.reviewsService.GetAverageRating(DRINK_ID_WITHOUT_RATINGS);

            // Assert
            Assert.Equal(0.0, result);
        }

        [Fact]
        public async Task GetAverageRating_WithNoReviews_ReturnsZero()
        {
            // Act
            double result = await this.reviewsService.GetAverageRating(DRINK_ID_WITH_NO_REVIEWS);

            // Assert
            Assert.Equal(0.0, result);
        }

        [Fact]
        public async Task GetAverageRating_WhenRepositoryThrowsException_ReturnsZero()
        {
            // Act
            double result = await this.reviewsService.GetAverageRating(DRINK_ID_THAT_THROWS);

            // Assert
            Assert.Equal(0.0, result);
        }
    }
}
