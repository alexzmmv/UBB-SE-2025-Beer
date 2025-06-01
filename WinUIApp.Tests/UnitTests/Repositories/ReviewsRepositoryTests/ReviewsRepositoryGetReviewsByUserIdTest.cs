using DataAccess.DTOModels;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;

namespace WinUIApp.Tests.UnitTests.Repositories.ReviewsRepositoryTests
{
    public class ReviewsRepositoryGetReviewsByUserIdTest
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;
        private readonly Mock<DbSet<Review>> mockReviewDbSet;
        private readonly ReviewsRepository reviewsRepository;
        private readonly List<Review> reviewData;
        private readonly Guid existingUserId;
        private readonly Guid nonExistingUserId;

        public ReviewsRepositoryGetReviewsByUserIdTest()
        {
            this.mockAppDbContext = new Mock<IAppDbContext>();

            this.existingUserId = Guid.NewGuid();
            this.nonExistingUserId = Guid.NewGuid();

            this.reviewData = new List<Review>
            {
                new Review { ReviewId = 1, UserId = this.existingUserId, Content = "User Review 1" },
                new Review { ReviewId = 2, UserId = Guid.NewGuid(), Content = "Other User Review" },
                new Review { ReviewId = 3, UserId = this.existingUserId, Content = "User Review 2" }
            };

            this.mockReviewDbSet = this.reviewData.AsQueryable().BuildMockDbSet();

            this.mockAppDbContext
                .Setup(context => context.Reviews)
                .Returns(this.mockReviewDbSet.Object);

            this.reviewsRepository = new ReviewsRepository(this.mockAppDbContext.Object);
        }

        [Fact]
        public async Task GetReviewsByUserId_WhenReviewsExist_ReturnsOnlyReviewsForGivenUserId()
        {
            // Arrange

            // Act
            List<ReviewDTO> reviewsForUser = await this.reviewsRepository.GetReviewsByUserId(this.existingUserId);

            // Assert
            Assert.All(reviewsForUser, reviewDto => Assert.Equal(this.existingUserId, reviewDto.UserId));
            Assert.Equal(2, reviewsForUser.Count);
        }

        [Fact]
        public async Task GetReviewsByUserId_WhenNoReviewsExist_ReturnsEmptyList()
        {
            // Arrange

            // Act
            List<ReviewDTO> reviewsForUser = await this.reviewsRepository.GetReviewsByUserId(this.nonExistingUserId);

            // Assert
            Assert.Empty(reviewsForUser);
        }
    }
}
