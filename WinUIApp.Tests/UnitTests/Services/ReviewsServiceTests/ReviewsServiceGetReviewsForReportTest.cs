using DataAccess.DTOModels;
using DataAccess.IRepository;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.ReviewsServiceTests
{
    public class ReviewsServiceGetReviewsForReportTest
    {
        private readonly Mock<IReviewsRepository> mockReviewsRepository;
        private readonly ReviewsService reviewsService;

        private readonly List<ReviewDTO> expectedRecentReviews;
        private readonly int reviewCountAfterDate;

        public ReviewsServiceGetReviewsForReportTest()
        {
            this.mockReviewsRepository = new Mock<IReviewsRepository>();
            this.reviewsService = new ReviewsService(this.mockReviewsRepository.Object);

            this.reviewCountAfterDate = 3;

            this.expectedRecentReviews = new List<ReviewDTO>
            {
                new ReviewDTO
                {
                    ReviewId = 101,
                    Content = "Most recent review one",
                    RatingValue = 4,
                    CreatedDate = DateTime.UtcNow,
                    DrinkId = 20,
                    UserId = Guid.NewGuid(),
                    IsHidden = false
                },
                new ReviewDTO
                {
                    ReviewId = 102,
                    Content = "Most recent review two",
                    RatingValue = 5,
                    CreatedDate = DateTime.UtcNow,
                    DrinkId = 21,
                    UserId = Guid.NewGuid(),
                    IsHidden = true
                },
                new ReviewDTO
                {
                    ReviewId = 103,
                    Content = "Most recent review three",
                    RatingValue = 3,
                    CreatedDate = DateTime.UtcNow,
                    DrinkId = 22,
                    UserId = Guid.NewGuid(),
                    IsHidden = false
                }
            };

            this.mockReviewsRepository
                .Setup(repository => repository.GetReviewCountAfterDate(It.IsAny<DateTime>()))
                .ReturnsAsync(this.reviewCountAfterDate);

            this.mockReviewsRepository
                .Setup(repository => repository.GetMostRecentReviews(this.reviewCountAfterDate))
                .ReturnsAsync(this.expectedRecentReviews);
        }

        [Fact]
        public async Task GetReviewsForReport_WhenRepositoryReturnsReviews_ReturnsExpectedReviews()
        {
            // Act
            List<ReviewDTO> actualReviews = await this.reviewsService.GetReviewsForReport();

            // Assert
            Assert.Equal(this.expectedRecentReviews, actualReviews);
        }

        [Fact]
        public async Task GetReviewsForReport_WhenRepositoryThrowsException_ReturnsEmptyList()
        {
            // Arrange
            this.mockReviewsRepository
                .Setup(repository => repository.GetReviewCountAfterDate(It.IsAny<DateTime>()))
                .ThrowsAsync(new Exception("Simulated failure"));

            // Act
            List<ReviewDTO> actualReviews = await this.reviewsService.GetReviewsForReport();

            // Assert
            Assert.Empty(actualReviews);
        }

        [Fact]
        public async Task GetReviewsForReport_WhenMostRecentReviewsReturnsNull_ReturnsEmptyList()
        {
            // Arrange
            this.mockReviewsRepository
                .Setup(repository => repository.GetMostRecentReviews(It.IsAny<int>()))
                .ReturnsAsync((List<ReviewDTO>?)null!);

            // Act
            List<ReviewDTO> actualReviews = await this.reviewsService.GetReviewsForReport();

            // Assert
            Assert.Empty(actualReviews);
        }
    }
}
