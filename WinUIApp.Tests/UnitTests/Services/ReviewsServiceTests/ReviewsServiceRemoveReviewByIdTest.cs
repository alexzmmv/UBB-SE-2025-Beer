using DataAccess.IRepository;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.ReviewsServiceTests
{
    public class ReviewsServiceRemoveReviewByIdTest
    {
        private readonly Mock<IReviewsRepository> mockReviewsRepository;
        private readonly ReviewsService reviewsService;

        private const int ExistingReviewId = 1;
        private const int NonExistingReviewId = 999;

        public ReviewsServiceRemoveReviewByIdTest()
        {
            mockReviewsRepository = new Mock<IReviewsRepository>();
            reviewsService = new ReviewsService(mockReviewsRepository.Object);

            // Setup mock for RemoveReviewById to just complete successfully
            mockReviewsRepository
                .Setup(repo => repo.RemoveReviewById(ExistingReviewId))
                .Returns(Task.CompletedTask);

            // Setup mock for RemoveReviewById to throw exception for non-existing review id
            mockReviewsRepository
                .Setup(repo => repo.RemoveReviewById(NonExistingReviewId))
                .ThrowsAsync(new Exception("Review not found"));
        }

        [Fact]
        public async Task RemoveReviewById_WhenCalledWithExistingReviewId_CallsRepositoryRemove()
        {
            // Act
            await reviewsService.RemoveReviewById(ExistingReviewId);

            // Assert
            mockReviewsRepository.Verify(repo => repo.RemoveReviewById(ExistingReviewId), Times.Once);
        }

        [Fact]
        public async Task RemoveReviewById_WhenRepositoryThrowsException_DoesNotThrow()
        {
            // Act & Assert
            var exception = await Record.ExceptionAsync(() => reviewsService.RemoveReviewById(NonExistingReviewId));
            Assert.Null(exception);
        }
    }
}
