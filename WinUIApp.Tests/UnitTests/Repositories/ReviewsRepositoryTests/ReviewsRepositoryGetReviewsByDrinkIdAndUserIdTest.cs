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
            this.mockAppDbContext = new Mock<IAppDbContext>();

            this.existingDrinkId = 10;
            this.nonExistingDrinkId = 999;

            this.existingUserId = Guid.NewGuid();
            this.nonExistingUserId = Guid.NewGuid();

            this.reviewData = new List<Review>
            {
                new Review { ReviewId = 1, DrinkId = this.existingDrinkId, UserId = this.existingUserId, Content = "Review 1" },
                new Review { ReviewId = 2, DrinkId = this.existingDrinkId, UserId = Guid.NewGuid(), Content = "Other User Review" },
                new Review { ReviewId = 3, DrinkId = 20, UserId = this.existingUserId, Content = "Other Drink Review" },
                new Review { ReviewId = 4, DrinkId = this.existingDrinkId, UserId = this.existingUserId, Content = "Review 2" }
            };

            this.mockReviewDbSet = this.reviewData.AsQueryable().BuildMockDbSet();

            this.mockAppDbContext
                .Setup(context => context.Reviews)
                .Returns(this.mockReviewDbSet.Object);

            this.reviewsRepository = new ReviewsRepository(this.mockAppDbContext.Object);
        }

        [Fact]
        public async Task GetReviewsByDrinkIdAndUserId_WhenReviewsExist_ReturnsOnlyReviewsForGivenDrinkIdAndUserId()
        {
            // Arrange

            // Act
            List<ReviewDTO> reviews = await this.reviewsRepository.GetReviewsByDrinkIdAndUserId(this.existingDrinkId, this.existingUserId);

            // Assert
            Assert.All(reviews, reviewDto =>
            {
                Assert.Equal(this.existingDrinkId, reviewDto.DrinkId);
                Assert.Equal(this.existingUserId, reviewDto.UserId);
            });
            Assert.Equal(2, reviews.Count);
        }

        [Fact]
        public async Task GetReviewsByDrinkIdAndUserId_WhenNoReviewsExist_ReturnsEmptyList()
        {
            // Arrange

            // Act
            List<ReviewDTO> reviews = await this.reviewsRepository.GetReviewsByDrinkIdAndUserId(this.nonExistingDrinkId, this.nonExistingUserId);

            // Assert
            Assert.Empty(reviews);
        }
    }
}
