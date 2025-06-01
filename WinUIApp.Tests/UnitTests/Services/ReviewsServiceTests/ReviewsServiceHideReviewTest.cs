using DataAccess.IRepository;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.ReviewsServiceTests
{
    public class ReviewsServiceHideReviewTest
    {
        private readonly Mock<IReviewsRepository> mockReviewsRepository;
        private readonly ReviewsService reviewsService;

        private const int VALID_REVIEW_ID = 123;
        private const bool HIDE_FLAG = true;

        public ReviewsServiceHideReviewTest()
        {
            this.mockReviewsRepository = new Mock<IReviewsRepository>();
            this.reviewsService = new ReviewsService(this.mockReviewsRepository.Object);
        }

        [Fact]
        public async Task HideReview_CallsUpdateReviewVisibilityWithTrue()
        {
            // Act
            await this.reviewsService.HideReview(VALID_REVIEW_ID);

            // Assert
            this.mockReviewsRepository.Verify(
                repository => repository.UpdateReviewVisibility(VALID_REVIEW_ID, HIDE_FLAG),
                Times.Once);
        }

        [Fact]
        public async Task HideReview_WhenRepositoryThrowsException_DoesNotThrow()
        {
            // Arrange
            this.mockReviewsRepository
                .Setup(repository => repository.UpdateReviewVisibility(It.IsAny<int>(), It.IsAny<bool>()))
                .ThrowsAsync(new Exception("Repository failure"));

            // Act & Assert
            Exception? exception = await Record.ExceptionAsync(() => this.reviewsService.HideReview(VALID_REVIEW_ID));
            Assert.Null(exception);
        }
    }
}
