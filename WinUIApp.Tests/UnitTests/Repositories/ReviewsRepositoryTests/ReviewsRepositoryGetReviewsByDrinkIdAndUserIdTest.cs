using DataAccess.DTOModels;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;

namespace WinUIApp.Tests.UnitTests.Repositories.ReviewsRepositoryTests
{
    public class ReviewsRepositoryGetReviewsByDrinkIdAndUserIdTest
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;
        private readonly Mock<DbSet<Review>> mockReviewDbSet;
        private readonly ReviewsRepository reviewsRepository;
        private readonly List<Review> reviewData;

        private readonly int existingDrinkId;
        private readonly Guid existingUserId;
        private readonly int nonExistingDrinkId;
        private readonly Guid nonExistingUserId;

        public ReviewsRepositoryGetReviewsByDrinkIdAndUserIdTest()
        {
            mockAppDbContext = new Mock<IAppDbContext>();

            existingDrinkId = 10;
            nonExistingDrinkId = 999;

            existingUserId = Guid.NewGuid();
            nonExistingUserId = Guid.NewGuid();

            reviewData = new List<Review>
            {
                new Review { ReviewId = 1, DrinkId = existingDrinkId, UserId = existingUserId, Content = "Review 1" },
                new Review { ReviewId = 2, DrinkId = existingDrinkId, UserId = Guid.NewGuid(), Content = "Other User Review" },
                new Review { ReviewId = 3, DrinkId = 20, UserId = existingUserId, Content = "Other Drink Review" },
                new Review { ReviewId = 4, DrinkId = existingDrinkId, UserId = existingUserId, Content = "Review 2" }
            };

            mockReviewDbSet = reviewData.AsQueryable().BuildMockDbSet();

            mockAppDbContext
                .Setup(context => context.Reviews)
                .Returns(mockReviewDbSet.Object);

            reviewsRepository = new ReviewsRepository(mockAppDbContext.Object);
        }

        [Fact]
        public async Task GetReviewsByDrinkIdAndUserId_WhenReviewsExist_ReturnsOnlyReviewsForGivenDrinkIdAndUserId()
        {
            // Arrange

            // Act
            List<ReviewDTO> reviews = await reviewsRepository.GetReviewsByDrinkIdAndUserId(existingDrinkId, existingUserId);

            // Assert
            Assert.All(reviews, reviewDto =>
            {
                Assert.Equal(existingDrinkId, reviewDto.DrinkId);
                Assert.Equal(existingUserId, reviewDto.UserId);
            });
            Assert.Equal(2, reviews.Count);
        }

        [Fact]
        public async Task GetReviewsByDrinkIdAndUserId_WhenNoReviewsExist_ReturnsEmptyList()
        {
            // Arrange

            // Act
            List<ReviewDTO> reviews = await reviewsRepository.GetReviewsByDrinkIdAndUserId(nonExistingDrinkId, nonExistingUserId);

            // Assert
            Assert.Empty(reviews);
        }
    }
}
