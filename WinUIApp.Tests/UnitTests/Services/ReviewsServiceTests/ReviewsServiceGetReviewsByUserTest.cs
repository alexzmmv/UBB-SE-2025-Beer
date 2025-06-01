using DataAccess.DTOModels;
using DataAccess.IRepository;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.ReviewsServiceTests
{
    public class ReviewsServiceGetReviewsByUserTest
    {
        private readonly Mock<IReviewsRepository> mockReviewsRepository;
        private readonly ReviewsService reviewsService;

        private readonly Guid userId;
        private readonly List<ReviewDTO> expectedReviews;

        public ReviewsServiceGetReviewsByUserTest()
        {
            this.mockReviewsRepository = new Mock<IReviewsRepository>();
            this.reviewsService = new ReviewsService(this.mockReviewsRepository.Object);

            this.userId = Guid.NewGuid();

            this.expectedReviews = new List<ReviewDTO>
            {
                new ReviewDTO
                {
                    ReviewId = 1,
                    Content = "User review one",
                    RatingValue = 4,
                    CreatedDate = DateTime.UtcNow,
                    DrinkId = 10,
                    UserId = this.userId,
                    IsHidden = false
                },
                new ReviewDTO
                {
                    ReviewId = 2,
                    Content = "User review two",
                    RatingValue = 5,
                    CreatedDate = DateTime.UtcNow,
                    DrinkId = 11,
                    UserId = this.userId,
                    IsHidden = true
                }
            };

            this.mockReviewsRepository
                .Setup(repository => repository.GetReviewsByUserId(this.userId))
                .ReturnsAsync(this.expectedReviews);
        }

        [Fact]
        public async Task GetReviewsByUser_WhenRepositoryReturnsReviews_ReturnsExpectedReviews()
        {
            // Act
            List<ReviewDTO> actualReviews = await this.reviewsService.GetReviewsByUser(this.userId);

            // Assert
            Assert.Equal(this.expectedReviews, actualReviews);
        }

        [Fact]
        public async Task GetReviewsByUser_WhenRepositoryThrowsException_ReturnsEmptyList()
        {
            // Arrange
            this.mockReviewsRepository
                .Setup(repository => repository.GetReviewsByUserId(this.userId))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            List<ReviewDTO> actualReviews = await this.reviewsService.GetReviewsByUser(this.userId);

            // Assert
            Assert.Empty(actualReviews);
        }
    }
}
