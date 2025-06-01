using DataAccess.DTOModels;
using DataAccess.IRepository;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.ReviewsServiceTests
{
    public class ReviewsServiceGetFlaggedReviewsTest
    {
        private readonly Mock<IReviewsRepository> mockReviewsRepository;
        private readonly ReviewsService reviewsService;

        private const int MINIMUM_FLAGS = 2;
        private readonly List<ReviewDTO> expectedFlaggedReviews;

        public ReviewsServiceGetFlaggedReviewsTest()
        {
            this.mockReviewsRepository = new Mock<IReviewsRepository>();
            this.reviewsService = new ReviewsService(this.mockReviewsRepository.Object);

            this.expectedFlaggedReviews = new List<ReviewDTO>
            {
                new ReviewDTO { ReviewId = 1, NumberOfFlags = MINIMUM_FLAGS },
                new ReviewDTO { ReviewId = 2, NumberOfFlags = MINIMUM_FLAGS + 1 }
            };

            this.mockReviewsRepository
                .Setup(repository => repository.GetFlaggedReviews(MINIMUM_FLAGS))
                .ReturnsAsync(this.expectedFlaggedReviews);
        }

        [Fact]
        public async Task GetFlaggedReviews_WithValidMinFlags_ReturnsFlaggedReviews()
        {
            // Act
            List<ReviewDTO> actualFlaggedReviews = await this.reviewsService.GetFlaggedReviews(MINIMUM_FLAGS);

            // Assert
            Assert.Equal(this.expectedFlaggedReviews, actualFlaggedReviews);
        }

        [Fact]
        public async Task GetFlaggedReviews_WhenRepositoryThrowsException_ReturnsEmptyList()
        {
            // Arrange
            this.mockReviewsRepository
                .Setup(repository => repository.GetFlaggedReviews(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Repository failure"));

            // Act
            List<ReviewDTO> actualFlaggedReviews = await this.reviewsService.GetFlaggedReviews(MINIMUM_FLAGS);

            // Assert
            Assert.Empty(actualFlaggedReviews);
        }
    }
}
