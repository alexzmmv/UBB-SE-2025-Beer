using DataAccess.DTOModels;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;

namespace WinUIApp.Tests.UnitTests.Repositories.ReviewsRepositoryTests
{
    public class ReviewsRepositoryGetAllReviewsTest
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;
        private readonly Mock<DbSet<Review>> mockReviewDbSet;
        private readonly ReviewsRepository reviewsRepository;
        private readonly List<Review> reviewData;

        public ReviewsRepositoryGetAllReviewsTest()
        {
            mockAppDbContext = new Mock<IAppDbContext>();

            reviewData = new List<Review>
            {
                new Review { ReviewId = 1, Content = "Review One" },
                new Review { ReviewId = 2, Content = "Review Two" }
            };

            mockReviewDbSet = reviewData.AsQueryable().BuildMockDbSet();

            mockAppDbContext
                .Setup(context => context.Reviews)
                .Returns(mockReviewDbSet.Object);

            reviewsRepository = new ReviewsRepository(mockAppDbContext.Object);
        }

        [Fact]
        public async Task GetAllReviews_WhenCalled_ReturnsMappedReviewDtos()
        {
            // Act
            List<ReviewDTO> returnedDtos = await reviewsRepository.GetAllReviews();

            // Assert
            int expectedCount = reviewData.Count;
            Assert.Equal(expectedCount, returnedDtos.Count);
        }

        [Fact]
        public async Task GetAllReviews_WhenCalled_ReturnsReviewDtosWithMatchingIds()
        {
            // Act
            List<ReviewDTO> returnedDtos = await reviewsRepository.GetAllReviews();

            // Assert
            int firstExpectedId = reviewData[0].ReviewId;
            int actualFirstId = returnedDtos[0].ReviewId;

            Assert.Equal(firstExpectedId, actualFirstId);
        }
    }
}
