using DataAccess.IRepository;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.ReviewsServiceTests
{
    public class ReviewsServiceResetReviewFlagsTest
    {
        private readonly Mock<IReviewsRepository> mockReviewsRepository;
        private readonly ReviewsService reviewsService;

        private const int ValidReviewId = 123;
        private const int NumberOfFlagsResetToZero = 0;

        public ReviewsServiceResetReviewFlagsTest()
        {
            mockReviewsRepository = new Mock<IReviewsRepository>();
            reviewsService = new ReviewsService(mockReviewsRepository.Object);
        }

        [Fact]
        public async Task ResetReviewFlags_CallsUpdateNumberOfFlagsForReviewWithZero()
        {
            // Act
            await reviewsService.ResetReviewFlags(ValidReviewId);

            // Assert
            mockReviewsRepository.Verify(
                repository => repository.UpdateNumberOfFlagsForReview(ValidReviewId, NumberOfFlagsResetToZero),
                Times.Once);
        }

        [Fact]
        public async Task ResetReviewFlags_WhenRepositoryThrowsException_DoesNotThrow()
        {
            // Arrange
            mockReviewsRepository
                .Setup(repository => repository.UpdateNumberOfFlagsForReview(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Repository failure"));

            // Act & Assert
            var exception = await Record.ExceptionAsync(() => reviewsService.ResetReviewFlags(ValidReviewId));
            Assert.Null(exception);
        }
    }
}
