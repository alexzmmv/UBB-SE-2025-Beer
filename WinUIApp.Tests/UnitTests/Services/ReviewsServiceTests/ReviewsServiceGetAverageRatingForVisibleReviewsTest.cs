using DataAccess.IRepository;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.ReviewsServiceTests
{
    public class ReviewsServiceGetAverageRatingForVisibleReviewsTest
    {
        private readonly Mock<IReviewsRepository> mockReviewsRepository;
        private readonly ReviewsService reviewsService;

        private const double ValidAverageRating = 4.3;
        private const double FailureAverageRating = 0.0;

        public ReviewsServiceGetAverageRatingForVisibleReviewsTest()
        {
            mockReviewsRepository = new Mock<IReviewsRepository>();
            reviewsService = new ReviewsService(mockReviewsRepository.Object);

            mockReviewsRepository
                .Setup(repository => repository.GetAverageRatingForVisibleReviews())
                .ReturnsAsync(ValidAverageRating);
        }

        [Fact]
        public async Task GetAverageRatingForVisibleReviews_WhenRepositoryReturnsValue_ReturnsAverageRating()
        {
            // Act
            double actualAverageRating = await reviewsService.GetAverageRatingForVisibleReviews();

            // Assert
            Assert.Equal(ValidAverageRating, actualAverageRating);
        }

        [Fact]
        public async Task GetAverageRatingForVisibleReviews_WhenRepositoryThrowsException_ReturnsZero()
        {
            // Arrange
            mockReviewsRepository
                .Setup(repository => repository.GetAverageRatingForVisibleReviews())
                .ThrowsAsync(new Exception("Repository failure"));

            // Act
            double actualAverageRating = await reviewsService.GetAverageRatingForVisibleReviews();

            // Assert
            Assert.Equal(FailureAverageRating, actualAverageRating);
        }
    }
}
