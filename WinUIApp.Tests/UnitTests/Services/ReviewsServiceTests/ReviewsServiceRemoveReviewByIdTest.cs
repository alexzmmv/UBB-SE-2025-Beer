using DataAccess.IRepository;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.ReviewsServiceTests
{
    public class ReviewsServiceRemoveReviewByIdTest
    {
        private readonly Mock<IReviewsRepository> mockReviewsRepository;
        private readonly ReviewsService reviewsService;

        private const int EXISTING_REVIEW_ID = 1;
        private const int NON_EXISTING_REVIEW_ID = 999;

        public ReviewsServiceRemoveReviewByIdTest()
        {
            this.mockReviewsRepository = new Mock<IReviewsRepository>();
            this.reviewsService = new ReviewsService(this.mockReviewsRepository.Object);

            // Setup mock for RemoveReviewById to just complete successfully
            this.mockReviewsRepository
                .Setup(repo => repo.RemoveReviewById(EXISTING_REVIEW_ID))
                .Returns(Task.CompletedTask);

            // Setup mock for RemoveReviewById to throw exception for non-existing review id
            this.mockReviewsRepository
                .Setup(repo => repo.RemoveReviewById(NON_EXISTING_REVIEW_ID))
                .ThrowsAsync(new Exception("Review not found"));
        }

        [Fact]
        public async Task RemoveReviewById_WhenCalledWithExistingReviewId_CallsRepositoryRemove()
        {
            // Act
            await this.reviewsService.RemoveReviewById(EXISTING_REVIEW_ID);

            // Assert
            this.mockReviewsRepository.Verify(repo => repo.RemoveReviewById(EXISTING_REVIEW_ID), Times.Once);
        }

        [Fact]
        public async Task RemoveReviewById_WhenRepositoryThrowsException_DoesNotThrow()
        {
            // Act & Assert
            Exception? exception = await Record.ExceptionAsync(() => this.reviewsService.RemoveReviewById(NON_EXISTING_REVIEW_ID));
            Assert.Null(exception);
        }
    }
}
