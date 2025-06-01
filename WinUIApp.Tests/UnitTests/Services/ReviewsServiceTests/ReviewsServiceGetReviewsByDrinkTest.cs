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

        private const int EXISTING_DRINK_ID = 42;
        private const int NON_EXISTENT_DRINK_ID = 999;

        private readonly List<ReviewDTO> reviewsForExistingDrink;

        public ReviewsServiceGetReviewsByDrinkTest()
        {
            this.mockReviewsRepository = new Mock<IReviewsRepository>();
            this.reviewsService = new ReviewsService(this.mockReviewsRepository.Object);

            this.reviewsForExistingDrink = new List<ReviewDTO>
            {
                new ReviewDTO
                {
                    ReviewId = 1,
                    Content = "Great taste!",
                    RatingValue = 5,
                    CreatedDate = DateTime.UtcNow,
                    DrinkId = EXISTING_DRINK_ID,
                    UserId = Guid.NewGuid(),
                    IsHidden = false
                },
                new ReviewDTO
                {
                    ReviewId = 2,
                    Content = "Not bad at all.",
                    RatingValue = 4,
                    CreatedDate = DateTime.UtcNow,
                    DrinkId = EXISTING_DRINK_ID,
                    UserId = Guid.NewGuid(),
                    IsHidden = false
                }
            };

            this.mockReviewsRepository
                .Setup(repository => repository.GetReviewsByDrinkId(EXISTING_DRINK_ID))
                .ReturnsAsync(this.reviewsForExistingDrink);

            this.mockReviewsRepository
                .Setup(repository => repository.GetReviewsByDrinkId(NON_EXISTENT_DRINK_ID))
                .ReturnsAsync(new List<ReviewDTO>());
        }

        [Fact]
        public async Task GetReviewsByDrink_WhenDrinkExists_ReturnsReviews()
        {
            // Act
            List<ReviewDTO> result = await this.reviewsService.GetReviewsByDrink(EXISTING_DRINK_ID);

            // Assert
            Assert.Equal(this.reviewsForExistingDrink, result);
        }

        [Fact]
        public async Task GetReviewsByDrink_WhenDrinkDoesNotExist_ReturnsEmptyList()
        {
            // Act
            List<ReviewDTO> result = await this.reviewsService.GetReviewsByDrink(NON_EXISTENT_DRINK_ID);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetReviewsByDrink_WhenRepositoryThrowsException_ReturnsEmptyList()
        {
            // Arrange
            const int drinkIdThatCausesException = 500;

            this.mockReviewsRepository
                .Setup(repository => repository.GetReviewsByDrinkId(drinkIdThatCausesException))
                .ThrowsAsync(new Exception("Simulated database failure"));

            // Act
            List<ReviewDTO> result = await this.reviewsService.GetReviewsByDrink(drinkIdThatCausesException);

            // Assert
            Assert.Empty(result);
        }
    }
}
