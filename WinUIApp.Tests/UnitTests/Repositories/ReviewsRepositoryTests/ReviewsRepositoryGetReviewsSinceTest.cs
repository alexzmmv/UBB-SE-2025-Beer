using DataAccess.DTOModels;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;

namespace WinUIApp.Tests.UnitTests.Repositories.ReviewsRepositoryTests
{
    public class ReviewsRepositoryGetReviewsSinceTest
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;
        private readonly Mock<DbSet<Review>> mockReviewDbSet;
        private readonly ReviewsRepository reviewsRepository;
        private readonly List<Review> reviewData;

        public ReviewsRepositoryGetReviewsSinceTest()
        {
            this.mockAppDbContext = new Mock<IAppDbContext>();

            DateTime now = DateTime.UtcNow;

            this.reviewData = new List<Review>
            {
                new Review { ReviewId = 1, Content = "Visible recent", CreatedDate = now.AddDays(-1), IsHidden = false },
                new Review { ReviewId = 2, Content = "Visible old", CreatedDate = now.AddDays(-10), IsHidden = false },
                new Review { ReviewId = 3, Content = "Hidden recent", CreatedDate = now.AddDays(-1), IsHidden = true },
                new Review { ReviewId = 4, Content = "Visible latest", CreatedDate = now, IsHidden = false }
            };

            this.mockReviewDbSet = this.reviewData.AsQueryable().BuildMockDbSet();

            this.mockAppDbContext
                .Setup(context => context.Reviews)
                .Returns(this.mockReviewDbSet.Object);

            this.reviewsRepository = new ReviewsRepository(this.mockAppDbContext.Object);
        }

        [Fact]
        public async Task GetReviewsSince_WhenCalled_FiltersOutOldReviews()
        {
            // Arrange
            DateTime filterDate = DateTime.UtcNow.AddDays(-5);

            // Act
            List<ReviewDTO> result = await this.reviewsRepository.GetReviewsSince(filterDate);

            // Assert
            int expectedCount = 2; // Only ReviewId 1 and 4 qualify
            Assert.Equal(expectedCount, result.Count);
        }

        [Fact]
        public async Task GetReviewsSince_WhenCalled_FiltersOutHiddenReviews()
        {
            // Arrange
            DateTime filterDate = DateTime.UtcNow.AddDays(-2);

            // Act
            List<ReviewDTO> result = await this.reviewsRepository.GetReviewsSince(filterDate);

            // Assert
            bool containsHiddenReview = result.Any(review => review.ReviewId == 3);
            Assert.False(containsHiddenReview);
        }

        [Fact]
        public async Task GetReviewsSince_WhenCalled_ReturnsReviewsInDescendingOrder()
        {
            // Arrange
            DateTime filterDate = DateTime.UtcNow.AddDays(-5);

            // Act
            List<ReviewDTO> result = await this.reviewsRepository.GetReviewsSince(filterDate);

            // Assert
            int expectedFirstReviewId = 4;
            int actualFirstReviewId = result.First().ReviewId;

            Assert.Equal(expectedFirstReviewId, actualFirstReviewId);
        }
    }
}
