using DataAccess.IRepository;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.ReviewsServiceTests
{
    public class ReviewsServiceGetReviewCountAfterDateTest
    {
        private readonly Mock<IReviewsRepository> mockReviewsRepository;
        private readonly ReviewsService reviewsService;

        private readonly DateTime targetDate;
        private const int ExpectedReviewCount = 4;

        public ReviewsServiceGetReviewCountAfterDateTest()
        {
            mockReviewsRepository = new Mock<IReviewsRepository>();
            reviewsService = new ReviewsService(mockReviewsRepository.Object);

            targetDate = new DateTime(2023, 1, 1);

            mockReviewsRepository
                .Setup(repository => repository.GetReviewCountAfterDate(targetDate))
                .ReturnsAsync(ExpectedReviewCount);
        }

        [Fact]
        public async Task GetReviewCountAfterDate_WhenRepositoryReturnsCount_ReturnsExpectedCount()
        {
            // Act
            int actualCount = await reviewsService.GetReviewCountAfterDate(targetDate);

            // Assert
            Assert.Equal(ExpectedReviewCount, actualCount);
        }

        [Fact]
        public async Task GetReviewCountAfterDate_WhenRepositoryThrowsException_ReturnsZero()
        {
            // Arrange
            mockReviewsRepository
                .Setup(repository => repository.GetReviewCountAfterDate(targetDate))
                .ThrowsAsync(new Exception("Simulated repository failure"));

            // Act
            int actualCount = await reviewsService.GetReviewCountAfterDate(targetDate);

            // Assert
            Assert.Equal(0, actualCount);
        }
    }
}
