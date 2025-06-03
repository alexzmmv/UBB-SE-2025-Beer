using DataAccess.DTOModels;
using DataAccess.IRepository;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.ReviewsServiceTests
{
    public class ReviewsServiceGetReviewsByDrinkAndUserTest
    {
        private readonly Mock<IReviewsRepository> mockReviewsRepository;
        private readonly ReviewsService reviewsService;

        private readonly int drinkId;
        private readonly Guid userId;
        private readonly List<ReviewDTO> expectedReviews;

        public ReviewsServiceGetReviewsByDrinkAndUserTest()
        {
            this.mockReviewsRepository = new Mock<IReviewsRepository>();
            this.reviewsService = new ReviewsService(this.mockReviewsRepository.Object);

            this.drinkId = 10;
            this.userId = Guid.NewGuid();

            this.expectedReviews = new List<ReviewDTO>
            {
                new ReviewDTO
                {
                    ReviewId = 1,
                    DrinkId = this.drinkId,
                    UserId = this.userId,
                    Content = "Review by user",
                    RatingValue = 5,
                    CreatedDate = DateTime.UtcNow,
                    IsHidden = false
                }
            };

            this.mockReviewsRepository
                .Setup(repository => repository.GetReviewsByDrinkIdAndUserId(this.drinkId, this.userId))
                .ReturnsAsync(this.expectedReviews);
        }

        [Fact]
        public async Task GetReviewsByDrinkAndUser_WhenRepositoryReturnsReviews_ReturnsExpectedReviews()
        {
            // Act
            List<ReviewDTO> actualReviews = await this.reviewsService.GetReviewsByDrinkAndUser(this.drinkId, this.userId);

            // Assert
            Assert.Equal(this.expectedReviews, actualReviews);
        }

        [Fact]
        public async Task GetReviewsByDrinkAndUser_WhenRepositoryThrowsException_ReturnsEmptyList()
        {
            // Arrange
            this.mockReviewsRepository
                .Setup(repository => repository.GetReviewsByDrinkIdAndUserId(this.drinkId, this.userId))
                .ThrowsAsync(new Exception("Simulated repository exception"));

            // Act
            List<ReviewDTO> actualReviews = await this.reviewsService.GetReviewsByDrinkAndUser(this.drinkId, this.userId);

            // Assert
            Assert.Empty(actualReviews);
        }
    }
}
