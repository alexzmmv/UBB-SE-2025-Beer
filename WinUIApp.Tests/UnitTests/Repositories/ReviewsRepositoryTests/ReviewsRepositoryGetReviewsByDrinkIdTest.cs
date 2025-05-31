using DataAccess.DTOModels;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;

namespace WinUIApp.Tests.UnitTests.Repositories.ReviewsRepositoryTests
{
    public class ReviewsRepositoryGetReviewsByDrinkIdTest
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;
        private readonly Mock<DbSet<Review>> mockReviewDbSet;
        private readonly ReviewsRepository reviewsRepository;
        private readonly List<Review> reviewData;

        public ReviewsRepositoryGetReviewsByDrinkIdTest()
        {
            mockAppDbContext = new Mock<IAppDbContext>();

            reviewData = new List<Review>
            {
                new Review { ReviewId = 1, DrinkId = 10, Content = "Review 1" },
                new Review { ReviewId = 2, DrinkId = 20, Content = "Review 2" },
                new Review { ReviewId = 3, DrinkId = 10, Content = "Review 3" }
            };

            mockReviewDbSet = reviewData.AsQueryable().BuildMockDbSet();

            mockAppDbContext
                .Setup(context => context.Reviews)
                .Returns(mockReviewDbSet.Object);

            reviewsRepository = new ReviewsRepository(mockAppDbContext.Object);
        }

        [Fact]
        public async Task GetReviewsByDrinkId_WhenReviewsExist_ReturnsOnlyReviewsForGivenDrinkId()
        {
            // Arrange
            int targetDrinkId = 10;

            // Act
            List<ReviewDTO> reviewsForDrink = await reviewsRepository.GetReviewsByDrinkId(targetDrinkId);

            // Assert
            Assert.All(reviewsForDrink, reviewDto => Assert.Equal(targetDrinkId, reviewDto.DrinkId));
            Assert.Equal(2, reviewsForDrink.Count);
        }

        [Fact]
        public async Task GetReviewsByDrinkId_WhenNoReviewsExist_ReturnsEmptyList()
        {
            // Arrange
            int nonExistingDrinkId = 999;

            // Act
            List<ReviewDTO> reviewsForDrink = await reviewsRepository.GetReviewsByDrinkId(nonExistingDrinkId);

            // Assert
            Assert.Empty(reviewsForDrink);
        }
    }
}
