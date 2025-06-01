using DataAccess.DTOModels;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;

namespace WinUIApp.Tests.UnitTests.Repositories.ReviewsRepositoryTests
{
    public class ReviewsRepositoryGetReviewByIdTest
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;
        private readonly Mock<DbSet<Review>> mockReviewDbSet;
        private readonly ReviewsRepository reviewsRepository;
        private readonly List<Review> reviewData;

        public ReviewsRepositoryGetReviewByIdTest()
        {
            this.mockAppDbContext = new Mock<IAppDbContext>();

            this.reviewData = new List<Review>
            {
                new Review { ReviewId = 1, Content = "Review 1", IsHidden = false },
                new Review { ReviewId = 2, Content = "Review 2", IsHidden = false }
            };

            this.mockReviewDbSet = this.reviewData.AsQueryable().BuildMockDbSet();

            this.mockAppDbContext
                .Setup(context => context.Reviews)
                .Returns(this.mockReviewDbSet.Object);

            this.reviewsRepository = new ReviewsRepository(this.mockAppDbContext.Object);
        }

        [Fact]
        public async Task GetReviewById_WhenReviewExists_ReturnsReviewDto()
        {
            // Arrange
            int existingReviewId = 1;

            // Act
            ReviewDTO? reviewDto = await this.reviewsRepository.GetReviewById(existingReviewId);

            // Assert
            Assert.NotNull(reviewDto);
            Assert.Equal(existingReviewId, reviewDto!.ReviewId);
        }

        [Fact]
        public async Task GetReviewById_WhenReviewDoesNotExist_ReturnsNull()
        {
            // Arrange
            int nonExistingReviewId = 999;

            // Act
            ReviewDTO? reviewDto = await this.reviewsRepository.GetReviewById(nonExistingReviewId);

            // Assert
            Assert.Null(reviewDto);
        }
    }
}
