using DataAccess.IRepository;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.ReviewsServiceTests
{
    public class ReviewsServiceResetReviewFlagsTest
    {
        private readonly Mock<IReviewsRepository> mockReviewsRepository;
        private readonly ReviewsService reviewsService;

        private const int VALID_REVIEW_ID = 123;
        private const int NUMBER_OF_FLAGS_RESET_TO_ZERO = 0;

        public ReviewsServiceResetReviewFlagsTest()
        {
            this.mockReviewsRepository = new Mock<IReviewsRepository>();
            this.reviewsService = new ReviewsService(this.mockReviewsRepository.Object);
        }

        [Fact]
        public async Task ResetReviewFlags_CallsUpdateNumberOfFlagsForReviewWithZero()
        {
            // Act
            await this.reviewsService.ResetReviewFlags(VALID_REVIEW_ID);

            // Assert
            this.mockReviewsRepository.Verify(
                repository => repository.UpdateNumberOfFlagsForReview(VALID_REVIEW_ID, NUMBER_OF_FLAGS_RESET_TO_ZERO),
                Times.Once);
        }

        [Fact]
        public async Task ResetReviewFlags_WhenRepositoryThrowsException_DoesNotThrow()
        {
            // Arrange
            this.mockReviewsRepository
                .Setup(repository => repository.UpdateNumberOfFlagsForReview(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Repository failure"));

            // Act & Assert
            Exception? exception = await Record.ExceptionAsync(() => this.reviewsService.ResetReviewFlags(VALID_REVIEW_ID));
            Assert.Null(exception);
        }
    }
}
