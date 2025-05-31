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
            mockAppDbContext = new Mock<IAppDbContext>();

            reviewData = new List<Review>
            {
                new Review { ReviewId = 1, Content = "Review 1", IsHidden = false },
                new Review { ReviewId = 2, Content = "Review 2", IsHidden = false }
            };

            mockReviewDbSet = reviewData.AsQueryable().BuildMockDbSet();

            mockAppDbContext
                .Setup(context => context.Reviews)
                .Returns(mockReviewDbSet.Object);

            reviewsRepository = new ReviewsRepository(mockAppDbContext.Object);
        }

        [Fact]
        public async Task GetReviewById_WhenReviewExists_ReturnsReviewDto()
        {
            // Arrange
            int existingReviewId = 1;

            // Act
            ReviewDTO? reviewDto = await reviewsRepository.GetReviewById(existingReviewId);

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
            ReviewDTO? reviewDto = await reviewsRepository.GetReviewById(nonExistingReviewId);

            // Assert
            Assert.Null(reviewDto);
        }
    }
}
