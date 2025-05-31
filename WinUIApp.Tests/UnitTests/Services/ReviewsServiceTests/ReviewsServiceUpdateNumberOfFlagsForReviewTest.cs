using DataAccess.IRepository;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.ReviewsServiceTests
{
    public class ReviewsServiceUpdateNumberOfFlagsForReviewTest
    {
        private readonly Mock<IReviewsRepository> mockReviewsRepository;
        private readonly ReviewsService reviewsService;

        private const int ValidReviewId = 1;
        private const int ValidNumberOfFlags = 3;

        public ReviewsServiceUpdateNumberOfFlagsForReviewTest()
        {
            mockReviewsRepository = new Mock<IReviewsRepository>();
            reviewsService = new ReviewsService(mockReviewsRepository.Object);

            mockReviewsRepository
                .Setup(repo => repo.UpdateNumberOfFlagsForReview(ValidReviewId, ValidNumberOfFlags))
                .Returns(Task.CompletedTask);
        }

        [Fact]
        public async Task UpdateNumberOfFlagsForReview_WhenCalled_InvokesRepositoryMethod()
        {
            // Act
            await reviewsService.UpdateNumberOfFlagsForReview(ValidReviewId, ValidNumberOfFlags);

            // Assert
            mockReviewsRepository.Verify(
                repo => repo.UpdateNumberOfFlagsForReview(ValidReviewId, ValidNumberOfFlags),
                Times.Once);
        }

        [Fact]
        public async Task UpdateNumberOfFlagsForReview_WhenRepositoryThrowsException_DoesNotThrow()
        {
            // Arrange
            mockReviewsRepository
                .Setup(repo => repo.UpdateNumberOfFlagsForReview(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Repository exception"));

            // Act & Assert
            await reviewsService.UpdateNumberOfFlagsForReview(ValidReviewId, ValidNumberOfFlags);
        }
    }
}
