using DataAccess.IRepository;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.ReviewsServiceTests
{
    public class ReviewsServiceUpdateNumberOfFlagsForReviewTest
    {
        private readonly Mock<IReviewsRepository> mockReviewsRepository;
        private readonly ReviewsService reviewsService;

        private const int VALID_REVIEW_ID = 1;
        private const int VALID_NUMBER_OF_FLAGS = 3;

        public ReviewsServiceUpdateNumberOfFlagsForReviewTest()
        {
            this.mockReviewsRepository = new Mock<IReviewsRepository>();
            this.reviewsService = new ReviewsService(this.mockReviewsRepository.Object);

            this.mockReviewsRepository
                .Setup(repo => repo.UpdateNumberOfFlagsForReview(VALID_REVIEW_ID, VALID_NUMBER_OF_FLAGS))
                .Returns(Task.CompletedTask);
        }

        [Fact]
        public async Task UpdateNumberOfFlagsForReview_WhenCalled_InvokesRepositoryMethod()
        {
            // Act
            await this.reviewsService.UpdateNumberOfFlagsForReview(VALID_REVIEW_ID, VALID_NUMBER_OF_FLAGS);

            // Assert
            this.mockReviewsRepository.Verify(
                repo => repo.UpdateNumberOfFlagsForReview(VALID_REVIEW_ID, VALID_NUMBER_OF_FLAGS),
                Times.Once);
        }

        [Fact]
        public async Task UpdateNumberOfFlagsForReview_WhenRepositoryThrowsException_DoesNotThrow()
        {
            // Arrange
            this.mockReviewsRepository
                .Setup(repo => repo.UpdateNumberOfFlagsForReview(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Repository exception"));

            // Act & Assert
            await this.reviewsService.UpdateNumberOfFlagsForReview(VALID_REVIEW_ID, VALID_NUMBER_OF_FLAGS);
        }
    }
}
