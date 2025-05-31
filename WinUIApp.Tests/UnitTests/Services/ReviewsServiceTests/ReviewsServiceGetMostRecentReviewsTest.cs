using DataAccess.DTOModels;
using DataAccess.IRepository;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.ReviewsServiceTests
{
    public class ReviewsServiceGetMostRecentReviewsTest
    {
        private readonly Mock<IReviewsRepository> mockReviewsRepository;
        private readonly ReviewsService reviewsService;

        private readonly List<ReviewDTO> expectedRecentReviews;
        private const int RequestedReviewCount = 3;

        public ReviewsServiceGetMostRecentReviewsTest()
        {
            mockReviewsRepository = new Mock<IReviewsRepository>();
            reviewsService = new ReviewsService(mockReviewsRepository.Object);

            expectedRecentReviews = new List<ReviewDTO>
            {
                new ReviewDTO { ReviewId = 101, CreatedDate = DateTime.UtcNow.AddMinutes(-1) },
                new ReviewDTO { ReviewId = 102, CreatedDate = DateTime.UtcNow.AddMinutes(-2) },
                new ReviewDTO { ReviewId = 103, CreatedDate = DateTime.UtcNow.AddMinutes(-3) }
            };

            mockReviewsRepository
                .Setup(repository => repository.GetMostRecentReviews(RequestedReviewCount))
                .ReturnsAsync(expectedRecentReviews);
        }

        [Fact]
        public async Task GetMostRecentReviews_WhenRepositoryReturnsReviews_ReturnsExpectedReviews()
        {
            // Act
            List<ReviewDTO> actualReviews = await reviewsService.GetMostRecentReviews(RequestedReviewCount);

            // Assert
            Assert.Equal(expectedRecentReviews, actualReviews);
        }

        [Fact]
        public async Task GetMostRecentReviews_WhenRepositoryThrowsException_ReturnsEmptyList()
        {
            // Arrange
            mockReviewsRepository
                .Setup(repository => repository.GetMostRecentReviews(RequestedReviewCount))
                .ThrowsAsync(new Exception("Repository failure"));

            // Act
            List<ReviewDTO> actualReviews = await reviewsService.GetMostRecentReviews(RequestedReviewCount);

            // Assert
            Assert.Empty(actualReviews);
        }
    }
}
