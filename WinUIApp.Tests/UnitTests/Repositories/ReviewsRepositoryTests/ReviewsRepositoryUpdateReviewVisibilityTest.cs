using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;

namespace WinUIApp.Tests.UnitTests.Repositories.ReviewsRepositoryTests
{
    public class ReviewsRepositoryUpdateReviewVisibilityTest
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;
        private readonly Mock<DbSet<Review>> mockReviewDbSet;
        private readonly ReviewsRepository reviewsRepository;
        private readonly List<Review> reviewData;

        public ReviewsRepositoryUpdateReviewVisibilityTest()
        {
            mockAppDbContext = new Mock<IAppDbContext>();

            reviewData = new List<Review>
            {
                new Review { ReviewId = 1, IsHidden = false },
                new Review { ReviewId = 2, IsHidden = true }
            };

            mockReviewDbSet = reviewData.AsQueryable().BuildMockDbSet();

            mockAppDbContext
                .Setup(context => context.Reviews)
                .Returns(mockReviewDbSet.Object);

            // Setup Find for synchronous call
            mockReviewDbSet
                .Setup(set => set.Find(It.IsAny<object[]>()))
                .Returns<object[]>(ids => reviewData.FirstOrDefault(r => r.ReviewId == (int)ids[0]));

            reviewsRepository = new ReviewsRepository(mockAppDbContext.Object);
        }

        [Fact]
        public async Task UpdateReviewVisibility_WhenReviewExists_UpdatesIsHiddenAndCallsSaveChanges()
        {
            // Arrange
            int existingReviewId = 1;
            bool newIsHiddenValue = true;
            Review? updatedReview = null;

            mockReviewDbSet
                .Setup(set => set.Update(It.IsAny<Review>()))
                .Callback<Review>(review => updatedReview = review);

            mockAppDbContext
                .Setup(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            await reviewsRepository.UpdateReviewVisibility(existingReviewId, newIsHiddenValue);

            // Assert
            Assert.NotNull(updatedReview);
            Assert.Equal(existingReviewId, updatedReview!.ReviewId);
            Assert.Equal(newIsHiddenValue, updatedReview.IsHidden);
        }

        [Fact]
        public async Task UpdateReviewVisibility_WhenReviewDoesNotExist_DoesNotCallUpdateOrSaveChanges()
        {
            // Arrange
            int nonExistingReviewId = 999;
            bool updateCalled = false;
            bool saveChangesCalled = false;

            mockReviewDbSet
                .Setup(set => set.Update(It.IsAny<Review>()))
                .Callback(() => updateCalled = true);

            mockAppDbContext
                .Setup(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Callback(() => saveChangesCalled = true)
                .ReturnsAsync(0);

            // Act
            await reviewsRepository.UpdateReviewVisibility(nonExistingReviewId, true);

            // Assert
            Assert.False(updateCalled);
            Assert.False(saveChangesCalled);
        }
    }
}
