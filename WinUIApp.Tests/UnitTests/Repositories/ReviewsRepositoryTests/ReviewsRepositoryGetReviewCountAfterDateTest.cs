using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;

namespace WinUIApp.Tests.UnitTests.Repositories.ReviewsRepositoryTests
{
    public class ReviewsRepositoryGetReviewCountAfterDateTest
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;
        private readonly Mock<DbSet<Review>> mockReviewDbSet;
        private readonly ReviewsRepository reviewsRepository;
        private readonly List<Review> reviewData;

        public ReviewsRepositoryGetReviewCountAfterDateTest()
        {
            this.mockAppDbContext = new Mock<IAppDbContext>();

            DateTime dateTime2023 = new DateTime(2023, 1, 1);
            DateTime dateTime2024 = new DateTime(2024, 1, 1);
            DateTime dateTime2025 = new DateTime(2025, 1, 1);

            this.reviewData = new List<Review>
            {
                new Review { ReviewId = 1, CreatedDate = dateTime2023, IsHidden = false },
                new Review { ReviewId = 2, CreatedDate = dateTime2024, IsHidden = false },
                new Review { ReviewId = 3, CreatedDate = dateTime2025, IsHidden = false },
                new Review { ReviewId = 4, CreatedDate = dateTime2025, IsHidden = true }
            };

            this.mockReviewDbSet = this.reviewData.AsQueryable().BuildMockDbSet();

            this.mockAppDbContext
                .Setup(context => context.Reviews)
                .Returns(this.mockReviewDbSet.Object);

            this.reviewsRepository = new ReviewsRepository(this.mockAppDbContext.Object);
        }

        [Fact]
        public async Task GetReviewCountAfterDate_WhenCalledWith2024Date_ReturnsTwoVisibleReviews()
        {
            // Arrange
            DateTime filterDate = new DateTime(2024, 1, 1);
            int expectedReviewCount = 2; // ReviewId 2 and 3 (not hidden)

            // Act
            int actualReviewCount = await this.reviewsRepository.GetReviewCountAfterDate(filterDate);

            // Assert
            Assert.Equal(expectedReviewCount, actualReviewCount);
        }

        [Fact]
        public async Task GetReviewCountAfterDate_WhenNoReviewsMatch_ReturnsZero()
        {
            // Arrange
            DateTime filterDate = new DateTime(2026, 1, 1);
            int expectedReviewCount = 0;

            // Act
            int actualReviewCount = await this.reviewsRepository.GetReviewCountAfterDate(filterDate);

            // Assert
            Assert.Equal(expectedReviewCount, actualReviewCount);
        }
    }
}
