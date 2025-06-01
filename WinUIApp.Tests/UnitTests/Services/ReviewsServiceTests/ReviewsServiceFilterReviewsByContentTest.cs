using DataAccess.DTOModels;
using DataAccess.IRepository;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.ReviewsServiceTests
{
    public class ReviewsServiceFilterReviewsByContentTest
    {
        private readonly Mock<IReviewsRepository> mockReviewsRepository;
        private readonly ReviewsService reviewsService;

        private readonly List<ReviewDTO> flaggedReviews;

        public ReviewsServiceFilterReviewsByContentTest()
        {
            this.mockReviewsRepository = new Mock<IReviewsRepository>();
            this.reviewsService = new ReviewsService(this.mockReviewsRepository.Object);

            this.flaggedReviews = new List<ReviewDTO>
            {
                new ReviewDTO
                {
                    ReviewId = 1,
                    Content = "This drink is fantastic!",
                    RatingValue = 5,
                    CreatedDate = DateTime.UtcNow,
                    DrinkId = 10,
                    UserId = Guid.NewGuid(),
                    IsHidden = false
                },
                new ReviewDTO
                {
                    ReviewId = 2,
                    Content = "Awful experience, never again.",
                    RatingValue = 1,
                    CreatedDate = DateTime.UtcNow,
                    DrinkId = 11,
                    UserId = Guid.NewGuid(),
                    IsHidden = false
                },
                new ReviewDTO
                {
                    ReviewId = 3,
                    Content = "Average flavor, decent service.",
                    RatingValue = 3,
                    CreatedDate = DateTime.UtcNow,
                    DrinkId = 12,
                    UserId = Guid.NewGuid(),
                    IsHidden = false
                }
            };

            this.mockReviewsRepository
                .Setup(repository => repository.GetFlaggedReviews(It.IsAny<int>()))
                .ReturnsAsync(this.flaggedReviews);
        }

        [Fact]
        public async Task FilterReviewsByContent_WhenContentIsEmpty_ReturnsAllFlaggedReviews()
        {
            // Arrange
            string emptyContentFilter = string.Empty;

            // Act
            List<ReviewDTO> result = await this.reviewsService.FilterReviewsByContent(emptyContentFilter);

            // Assert
            Assert.Equal(this.flaggedReviews, result);
        }

        [Fact]
        public async Task FilterReviewsByContent_WhenContentMatchesSomeReviews_ReturnsMatchingReviews()
        {
            // Arrange
            string contentFilter = "fantastic";

            // Act
            List<ReviewDTO> result = await this.reviewsService.FilterReviewsByContent(contentFilter);

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task FilterReviewsByContent_WhenContentMatchesNothing_ReturnsEmptyList()
        {
            // Arrange
            string contentFilter = "unrelatedtext";

            // Act
            List<ReviewDTO> result = await this.reviewsService.FilterReviewsByContent(contentFilter);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task FilterReviewsByContent_WhenGetFlaggedReviewsThrowsException_ReturnsEmptyList()
        {
            // Arrange  
            Mock<ReviewsService> partialMockService = new(this.mockReviewsRepository.Object);
            string anyContent = "anything";

            partialMockService
                .Setup(service => service.GetFlaggedReviews(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Simulated failure inside service method"));

            // Act  
            List<ReviewDTO> result = await partialMockService.Object.FilterReviewsByContent(anyContent);

            // Assert  
            Assert.Empty(result);
        }
    }
}
