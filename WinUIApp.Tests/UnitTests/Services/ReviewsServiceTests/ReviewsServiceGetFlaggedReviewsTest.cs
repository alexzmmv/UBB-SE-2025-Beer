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

        private const int MinimumFlags = 2;
        private readonly List<ReviewDTO> expectedFlaggedReviews;

        public ReviewsServiceGetFlaggedReviewsTest()
        {
            mockReviewsRepository = new Mock<IReviewsRepository>();
            reviewsService = new ReviewsService(mockReviewsRepository.Object);

            expectedFlaggedReviews = new List<ReviewDTO>
            {
                new ReviewDTO { ReviewId = 1, NumberOfFlags = MinimumFlags },
                new ReviewDTO { ReviewId = 2, NumberOfFlags = MinimumFlags + 1 }
            };

            mockReviewsRepository
                .Setup(repository => repository.GetFlaggedReviews(MinimumFlags))
                .ReturnsAsync(expectedFlaggedReviews);
        }

        [Fact]
        public async Task GetFlaggedReviews_WithValidMinFlags_ReturnsFlaggedReviews()
        {
            // Act
            List<ReviewDTO> actualFlaggedReviews = await reviewsService.GetFlaggedReviews(MinimumFlags);

            // Assert
            Assert.Equal(expectedFlaggedReviews, actualFlaggedReviews);
        }

        [Fact]
        public async Task GetFlaggedReviews_WhenRepositoryThrowsException_ReturnsEmptyList()
        {
            // Arrange
            mockReviewsRepository
                .Setup(repository => repository.GetFlaggedReviews(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Repository failure"));

            // Act
            List<ReviewDTO> actualFlaggedReviews = await reviewsService.GetFlaggedReviews(MinimumFlags);

            // Assert
            Assert.Empty(actualFlaggedReviews);
        }
    }
}
