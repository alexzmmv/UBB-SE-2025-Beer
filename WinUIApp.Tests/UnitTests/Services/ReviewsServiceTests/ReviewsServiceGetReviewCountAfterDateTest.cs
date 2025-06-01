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
        private const int EXPECTED_REVIEW_COUNT = 4;

        public ReviewsServiceGetReviewCountAfterDateTest()
        {
            this.mockReviewsRepository = new Mock<IReviewsRepository>();
            this.reviewsService = new ReviewsService(this.mockReviewsRepository.Object);

            this.targetDate = new DateTime(2023, 1, 1);

            this.mockReviewsRepository
                .Setup(repository => repository.GetReviewCountAfterDate(this.targetDate))
                .ReturnsAsync(EXPECTED_REVIEW_COUNT);
        }

        [Fact]
        public async Task GetReviewCountAfterDate_WhenRepositoryReturnsCount_ReturnsExpectedCount()
        {
            // Act
            int actualCount = await this.reviewsService.GetReviewCountAfterDate(this.targetDate);

            // Assert
            Assert.Equal(EXPECTED_REVIEW_COUNT, actualCount);
        }

        [Fact]
        public async Task GetReviewCountAfterDate_WhenRepositoryThrowsException_ReturnsZero()
        {
            // Arrange
            this.mockReviewsRepository
                .Setup(repository => repository.GetReviewCountAfterDate(this.targetDate))
                .ThrowsAsync(new Exception("Simulated repository failure"));

            // Act
            int actualCount = await this.reviewsService.GetReviewCountAfterDate(this.targetDate);

            // Assert
            Assert.Equal(0, actualCount);
        }
    }
}
