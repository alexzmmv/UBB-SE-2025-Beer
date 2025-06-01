using DataAccess.DTOModels;
using DataAccess.IRepository;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.ReviewsServiceTests
{
    public class ReviewsServiceGetReviewsSinceTest
    {
        private readonly Mock<IReviewsRepository> mockReviewsRepository;
        private readonly ReviewsService reviewsService;

        private readonly DateTime validDate;
        private readonly List<ReviewDTO> reviewsSinceDate;

        public ReviewsServiceGetReviewsSinceTest()
        {
            this.mockReviewsRepository = new Mock<IReviewsRepository>();
            this.reviewsService = new ReviewsService(this.mockReviewsRepository.Object);

            this.validDate = new DateTime(2023, 1, 1);

            this.reviewsSinceDate = new List<ReviewDTO>
            {
                new ReviewDTO { ReviewId = 10, Content = "Recent Review 1", CreatedDate = new DateTime(2023, 3, 15) },
                new ReviewDTO { ReviewId = 11, Content = "Recent Review 2", CreatedDate = new DateTime(2023, 2, 20) }
            };

            this.mockReviewsRepository
                .Setup(repository => repository.GetReviewsSince(It.Is<DateTime>(date => date == this.validDate)))
                .ReturnsAsync(this.reviewsSinceDate);
        }

        [Fact]
        public async Task GetReviewsSince_WithValidDate_ReturnsReviews()
        {
            // Act
            List<ReviewDTO> actualReviews = await this.reviewsService.GetReviewsSince(this.validDate);

            // Assert
            Assert.Equal(this.reviewsSinceDate, actualReviews);
        }

        [Fact]
        public async Task GetReviewsSince_WhenRepositoryThrowsException_ReturnsEmptyList()
        {
            // Arrange
            this.mockReviewsRepository
                .Setup(repository => repository.GetReviewsSince(It.IsAny<DateTime>()))
                .ThrowsAsync(new Exception("Repository failure"));

            // Act
            List<ReviewDTO> actualReviews = await this.reviewsService.GetReviewsSince(this.validDate);

            // Assert
            Assert.Empty(actualReviews);
        }
    }
}
