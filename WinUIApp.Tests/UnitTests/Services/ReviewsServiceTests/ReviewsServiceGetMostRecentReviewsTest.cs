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
        private const int REQUESTED_REVIEW_COUNT = 3;

        public ReviewsServiceGetMostRecentReviewsTest()
        {
            this.mockReviewsRepository = new Mock<IReviewsRepository>();
            this.reviewsService = new ReviewsService(this.mockReviewsRepository.Object);

            this.expectedRecentReviews = new List<ReviewDTO>
            {
                new ReviewDTO { ReviewId = 101, CreatedDate = DateTime.UtcNow.AddMinutes(-1) },
                new ReviewDTO { ReviewId = 102, CreatedDate = DateTime.UtcNow.AddMinutes(-2) },
                new ReviewDTO { ReviewId = 103, CreatedDate = DateTime.UtcNow.AddMinutes(-3) }
            };

            this.mockReviewsRepository
                .Setup(repository => repository.GetMostRecentReviews(REQUESTED_REVIEW_COUNT))
                .ReturnsAsync(this.expectedRecentReviews);
        }

        [Fact]
        public async Task GetMostRecentReviews_WhenRepositoryReturnsReviews_ReturnsExpectedReviews()
        {
            // Act
            List<ReviewDTO> actualReviews = await this.reviewsService.GetMostRecentReviews(REQUESTED_REVIEW_COUNT);

            // Assert
            Assert.Equal(this.expectedRecentReviews, actualReviews);
        }

        [Fact]
        public async Task GetMostRecentReviews_WhenRepositoryThrowsException_ReturnsEmptyList()
        {
            // Arrange
            this.mockReviewsRepository
                .Setup(repository => repository.GetMostRecentReviews(REQUESTED_REVIEW_COUNT))
                .ThrowsAsync(new Exception("Repository failure"));

            // Act
            List<ReviewDTO> actualReviews = await this.reviewsService.GetMostRecentReviews(REQUESTED_REVIEW_COUNT);

            // Assert
            Assert.Empty(actualReviews);
        }
    }
}
