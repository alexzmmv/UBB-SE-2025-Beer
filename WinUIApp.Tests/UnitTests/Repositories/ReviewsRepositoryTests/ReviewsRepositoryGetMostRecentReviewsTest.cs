using DataAccess.DTOModels;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;

namespace WinUIApp.Tests.UnitTests.Repositories.ReviewsRepositoryTests
{
    public class ReviewsRepositoryGetMostRecentReviewsTest
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;
        private readonly Mock<DbSet<Review>> mockReviewDbSet;
        private readonly ReviewsRepository reviewsRepository;
        private readonly List<Review> reviewData;

        public ReviewsRepositoryGetMostRecentReviewsTest()
        {
            mockAppDbContext = new Mock<IAppDbContext>();

            DateTime dateTime2023 = new DateTime(2023, 1, 1);
            DateTime dateTime2024 = new DateTime(2024, 1, 1);
            DateTime dateTime2025 = new DateTime(2025, 1, 1);

            reviewData = new List<Review>
            {
                new Review { ReviewId = 1, CreatedDate = dateTime2023, IsHidden = false, Content = "Review A" },
                new Review { ReviewId = 2, CreatedDate = dateTime2025, IsHidden = false, Content = "Review B" },
                new Review { ReviewId = 3, CreatedDate = dateTime2024, IsHidden = false, Content = "Review C" },
                new Review { ReviewId = 4, CreatedDate = dateTime2025, IsHidden = true, Content = "Hidden Review" }
            };

            mockReviewDbSet = reviewData.AsQueryable().BuildMockDbSet();

            mockAppDbContext
                .Setup(context => context.Reviews)
                .Returns(mockReviewDbSet.Object);

            reviewsRepository = new ReviewsRepository(mockAppDbContext.Object);
        }

        [Fact]
        public async Task GetMostRecentReviews_WhenCalledWithCountTwo_ReturnsTwoMostRecentVisibleReviews()
        {
            // Arrange
            int numberOfReviewsToReturn = 2;
            List<int> expectedReviewIds = new List<int> { 2, 3 }; // ReviewId 2 (2025), ReviewId 3 (2024)

            // Act
            List<ReviewDTO> actualReviewDtos = await reviewsRepository.GetMostRecentReviews(numberOfReviewsToReturn);
            List<int> actualReviewIds = actualReviewDtos.Select(dto => dto.ReviewId).ToList();

            // Assert
            Assert.Equal(expectedReviewIds, actualReviewIds);
        }

        [Fact]
        public async Task GetMostRecentReviews_WhenThereAreNoVisibleReviews_ReturnsEmptyList()
        {
            // Arrange
            var onlyHiddenReviews = new List<Review>
            {
                new Review { ReviewId = 10, CreatedDate = new DateTime(2025, 1, 1), IsHidden = true },
                new Review { ReviewId = 11, CreatedDate = new DateTime(2024, 1, 1), IsHidden = true }
            };

            var hiddenMockDbSet = onlyHiddenReviews.AsQueryable().BuildMockDbSet();

            mockAppDbContext
                .Setup(context => context.Reviews)
                .Returns(hiddenMockDbSet.Object);

            var repositoryWithOnlyHiddenReviews = new ReviewsRepository(mockAppDbContext.Object);

            int numberOfReviewsToReturn = 3;

            // Act
            List<ReviewDTO> actualReviewDtos = await repositoryWithOnlyHiddenReviews.GetMostRecentReviews(numberOfReviewsToReturn);

            // Assert
            Assert.Empty(actualReviewDtos);
        }
    }
}
