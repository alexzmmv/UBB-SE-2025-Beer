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
            mockReviewsRepository = new Mock<IReviewsRepository>();
            reviewsService = new ReviewsService(mockReviewsRepository.Object);

            userId = Guid.NewGuid();

            expectedReviews = new List<ReviewDTO>
            {
                new ReviewDTO
                {
                    ReviewId = 1,
                    Content = "User review one",
                    RatingValue = 4,
                    CreatedDate = DateTime.UtcNow,
                    DrinkId = 10,
                    UserId = userId,
                    IsHidden = false
                },
                new ReviewDTO
                {
                    ReviewId = 2,
                    Content = "User review two",
                    RatingValue = 5,
                    CreatedDate = DateTime.UtcNow,
                    DrinkId = 11,
                    UserId = userId,
                    IsHidden = true
                }
            };

            mockReviewsRepository
                .Setup(repository => repository.GetReviewsByUserId(userId))
                .ReturnsAsync(expectedReviews);
        }

        [Fact]
        public async Task GetReviewsByUser_WhenRepositoryReturnsReviews_ReturnsExpectedReviews()
        {
            // Act
            List<ReviewDTO> actualReviews = await reviewsService.GetReviewsByUser(userId);

            // Assert
            Assert.Equal(expectedReviews, actualReviews);
        }

        [Fact]
        public async Task GetReviewsByUser_WhenRepositoryThrowsException_ReturnsEmptyList()
        {
            // Arrange
            mockReviewsRepository
                .Setup(repository => repository.GetReviewsByUserId(userId))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            List<ReviewDTO> actualReviews = await reviewsService.GetReviewsByUser(userId);

            // Assert
            Assert.Empty(actualReviews);
        }
    }
}
