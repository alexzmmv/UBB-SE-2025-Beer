using DataAccess.IRepository;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.ReviewsServiceTests
{
    public class ReviewsServiceHideReviewTest
    {
        private readonly Mock<IReviewsRepository> mockReviewsRepository;
        private readonly ReviewsService reviewsService;

        private const int ValidReviewId = 123;
        private const bool HideFlag = true;

        public ReviewsServiceHideReviewTest()
        {
            mockReviewsRepository = new Mock<IReviewsRepository>();
            reviewsService = new ReviewsService(mockReviewsRepository.Object);
        }

        [Fact]
        public async Task HideReview_CallsUpdateReviewVisibilityWithTrue()
        {
            // Act
            await reviewsService.HideReview(ValidReviewId);

            // Assert
            mockReviewsRepository.Verify(
                repository => repository.UpdateReviewVisibility(ValidReviewId, HideFlag),
                Times.Once);
        }

        [Fact]
        public async Task HideReview_WhenRepositoryThrowsException_DoesNotThrow()
        {
            // Arrange
            mockReviewsRepository
                .Setup(repository => repository.UpdateReviewVisibility(It.IsAny<int>(), It.IsAny<bool>()))
                .ThrowsAsync(new Exception("Repository failure"));

            // Act & Assert
            var exception = await Record.ExceptionAsync(() => reviewsService.HideReview(ValidReviewId));
            Assert.Null(exception);
        }
    }
}
