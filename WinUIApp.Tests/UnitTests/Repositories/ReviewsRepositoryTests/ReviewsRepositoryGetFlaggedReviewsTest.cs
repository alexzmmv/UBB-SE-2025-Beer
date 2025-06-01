using DataAccess.DTOModels;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;

namespace WinUIApp.Tests.UnitTests.Repositories.ReviewsRepositoryTests
{
    public class ReviewsRepositoryGetFlaggedReviewsTest
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;
        private readonly Mock<DbSet<Review>> mockReviewDbSet;
        private readonly ReviewsRepository reviewsRepository;
        private readonly List<Review> reviewData;

        public ReviewsRepositoryGetFlaggedReviewsTest()
        {
            this.mockAppDbContext = new Mock<IAppDbContext>();

            int zeroFlags = 0;
            int oneFlag = 1;
            int twoFlags = 2;

            this.reviewData = new List<Review>
            {
                new Review { ReviewId = 1, NumberOfFlags = zeroFlags, IsHidden = false },
                new Review { ReviewId = 2, NumberOfFlags = oneFlag, IsHidden = false },
                new Review { ReviewId = 3, NumberOfFlags = twoFlags, IsHidden = false },
                new Review { ReviewId = 4, NumberOfFlags = twoFlags, IsHidden = true }
            };

            this.mockReviewDbSet = this.reviewData.AsQueryable().BuildMockDbSet();

            this.mockAppDbContext
                .Setup(context => context.Reviews)
                .Returns(this.mockReviewDbSet.Object);

            this.reviewsRepository = new ReviewsRepository(this.mockAppDbContext.Object);
        }

        [Fact]
        public async Task GetFlaggedReviews_WhenCalledWithMinimumOneFlag_ReturnsTwoVisibleReviews()
        {
            // Arrange
            int minimumRequiredFlags = 1;
            int expectedReviewCount = 2; // ReviewId 2 and 3 (visible and flagged)

            // Act
            List<ReviewDTO> returnedReviews = await this.reviewsRepository.GetFlaggedReviews(minimumRequiredFlags);

            // Assert
            Assert.Equal(expectedReviewCount, returnedReviews.Count);
        }

        [Fact]
        public async Task GetFlaggedReviews_WhenAllMatchingReviewsAreHidden_ReturnsEmptyList()
        {
            // Arrange
            int minimumRequiredFlags = 2;
            this.reviewData.ForEach(review => review.IsHidden = true);

            // Rebuild mock to reflect new hidden states
            Mock<DbSet<Review>> updatedMockDbSet = this.reviewData.AsQueryable().BuildMockDbSet();
            this.mockAppDbContext.Setup(context => context.Reviews).Returns(updatedMockDbSet.Object);

            int expectedReviewCount = 0;

            // Act
            List<ReviewDTO> returnedReviews = await this.reviewsRepository.GetFlaggedReviews(minimumRequiredFlags);

            // Assert
            Assert.Equal(expectedReviewCount, returnedReviews.Count);
        }
    }
}
