using DataAccess.DTOModels;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;

namespace WinUIApp.Tests.UnitTests.Repositories.ReviewsRepositoryTests
{
    public class ReviewsRepositoryGetHiddenReviewsTest
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;
        private readonly Mock<DbSet<Review>> mockReviewDbSet;
        private readonly ReviewsRepository reviewsRepository;
        private readonly List<Review> reviewData;

        public ReviewsRepositoryGetHiddenReviewsTest()
        {
            mockAppDbContext = new Mock<IAppDbContext>();

            reviewData = new List<Review>
            {
                new Review { ReviewId = 1, IsHidden = true },
                new Review { ReviewId = 2, IsHidden = false },
                new Review { ReviewId = 3, IsHidden = true }
            };

            mockReviewDbSet = reviewData.AsQueryable().BuildMockDbSet();

            mockAppDbContext
                .Setup(context => context.Reviews)
                .Returns(mockReviewDbSet.Object);

            reviewsRepository = new ReviewsRepository(mockAppDbContext.Object);
        }

        [Fact]
        public async Task GetHiddenReviews_WhenCalled_ReturnsOnlyHiddenReviews()
        {
            // Act
            List<ReviewDTO> hiddenReviews = await reviewsRepository.GetHiddenReviews();

            // Assert
            Assert.All(hiddenReviews, reviewDto => Assert.True(reviewDto.IsHidden));
            Assert.Equal(2, hiddenReviews.Count);
        }
    }
}
