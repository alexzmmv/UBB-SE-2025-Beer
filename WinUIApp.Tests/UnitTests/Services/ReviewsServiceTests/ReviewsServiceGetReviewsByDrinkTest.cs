using DataAccess.DTOModels;
using DataAccess.IRepository;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.ReviewsServiceTests
{
    public class ReviewsServiceGetReviewsByDrinkTest
    {
        private readonly Mock<IReviewsRepository> mockReviewsRepository;
        private readonly ReviewsService reviewsService;

        private const int ExistingDrinkId = 42;
        private const int NonExistentDrinkId = 999;

        private readonly List<ReviewDTO> reviewsForExistingDrink;

        public ReviewsServiceGetReviewsByDrinkTest()
        {
            mockReviewsRepository = new Mock<IReviewsRepository>();
            reviewsService = new ReviewsService(mockReviewsRepository.Object);

            reviewsForExistingDrink = new List<ReviewDTO>
            {
                new ReviewDTO
                {
                    ReviewId = 1,
                    Content = "Great taste!",
                    RatingValue = 5,
                    CreatedDate = DateTime.UtcNow,
                    DrinkId = ExistingDrinkId,
                    UserId = Guid.NewGuid(),
                    IsHidden = false
                },
                new ReviewDTO
                {
                    ReviewId = 2,
                    Content = "Not bad at all.",
                    RatingValue = 4,
                    CreatedDate = DateTime.UtcNow,
                    DrinkId = ExistingDrinkId,
                    UserId = Guid.NewGuid(),
                    IsHidden = false
                }
            };

            mockReviewsRepository
                .Setup(repository => repository.GetReviewsByDrinkId(ExistingDrinkId))
                .ReturnsAsync(reviewsForExistingDrink);

            mockReviewsRepository
                .Setup(repository => repository.GetReviewsByDrinkId(NonExistentDrinkId))
                .ReturnsAsync(new List<ReviewDTO>());
        }

        [Fact]
        public async Task GetReviewsByDrink_WhenDrinkExists_ReturnsReviews()
        {
            // Act
            List<ReviewDTO> result = await reviewsService.GetReviewsByDrink(ExistingDrinkId);

            // Assert
            Assert.Equal(reviewsForExistingDrink, result);
        }

        [Fact]
        public async Task GetReviewsByDrink_WhenDrinkDoesNotExist_ReturnsEmptyList()
        {
            // Act
            List<ReviewDTO> result = await reviewsService.GetReviewsByDrink(NonExistentDrinkId);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetReviewsByDrink_WhenRepositoryThrowsException_ReturnsEmptyList()
        {
            // Arrange
            const int drinkIdThatCausesException = 500;

            mockReviewsRepository
                .Setup(repository => repository.GetReviewsByDrinkId(drinkIdThatCausesException))
                .ThrowsAsync(new Exception("Simulated database failure"));

            // Act
            List<ReviewDTO> result = await reviewsService.GetReviewsByDrink(drinkIdThatCausesException);

            // Assert
            Assert.Empty(result);
        }
    }
}
