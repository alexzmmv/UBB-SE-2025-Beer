using DataAccess.IRepository;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.ReviewsServiceTests
{
    public class ReviewsServiceGetAverageRatingForVisibleReviewsTest
    {
        private readonly Mock<IReviewsRepository> mockReviewsRepository;
        private readonly ReviewsService reviewsService;

        private const double VALID_AVERAGE_RATING = 4.3;
        private const double FAILURE_AVERAGE_RATING = 0.0;

        public ReviewsServiceGetAverageRatingForVisibleReviewsTest()
        {
            this.mockReviewsRepository = new Mock<IReviewsRepository>();
            this.reviewsService = new ReviewsService(this.mockReviewsRepository.Object);

            this.mockReviewsRepository
                .Setup(repository => repository.GetAverageRatingForVisibleReviews())
                .ReturnsAsync(VALID_AVERAGE_RATING);
        }

        [Fact]
        public async Task GetAverageRatingForVisibleReviews_WhenRepositoryReturnsValue_ReturnsAverageRating()
        {
            // Act
            double actualAverageRating = await this.reviewsService.GetAverageRatingForVisibleReviews();

            // Assert
            Assert.Equal(VALID_AVERAGE_RATING, actualAverageRating);
        }

        [Fact]
        public async Task GetAverageRatingForVisibleReviews_WhenRepositoryThrowsException_ReturnsZero()
        {
            // Arrange
            this.mockReviewsRepository
                .Setup(repository => repository.GetAverageRatingForVisibleReviews())
                .ThrowsAsync(new Exception("Repository failure"));

            // Act
            double actualAverageRating = await this.reviewsService.GetAverageRatingForVisibleReviews();

            // Assert
            Assert.Equal(FAILURE_AVERAGE_RATING, actualAverageRating);
        }
    }
}
