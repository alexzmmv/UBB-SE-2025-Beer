using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;

namespace WinUIApp.Tests.UnitTests.Repositories.ReviewsRepositoryTests
{
    public class ReviewsRepositoryUpdateNumberOfFlagsForReviewTest
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;
        private readonly Mock<DbSet<Review>> mockReviewDbSet;
        private readonly ReviewsRepository reviewsRepository;
        private readonly List<Review> reviewData;

        public ReviewsRepositoryUpdateNumberOfFlagsForReviewTest()
        {
            mockAppDbContext = new Mock<IAppDbContext>();

            reviewData = new List<Review>
            {
                new Review { ReviewId = 1, NumberOfFlags = 0 },
                new Review { ReviewId = 2, NumberOfFlags = 5 }
            };

            mockReviewDbSet = reviewData.AsQueryable().BuildMockDbSet();

            mockAppDbContext
                .Setup(context => context.Reviews)
                .Returns(mockReviewDbSet.Object);

            mockReviewDbSet
                .Setup(set => set.Find(It.IsAny<object[]>()))
                .Returns<object[]>(ids => reviewData.FirstOrDefault(r => r.ReviewId == (int)ids[0]));

            reviewsRepository = new ReviewsRepository(mockAppDbContext.Object);
        }

        [Fact]
        public async Task UpdateNumberOfFlagsForReview_WhenReviewExists_UpdatesNumberOfFlagsAndCallsSaveChanges()
        {
            // Arrange
            int existingReviewId = 1;
            int newNumberOfFlags = 3;
            Review? updatedReview = null;

            mockReviewDbSet
                .Setup(set => set.Update(It.IsAny<Review>()))
                .Callback<Review>(review => updatedReview = review);

            mockAppDbContext
                .Setup(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            await reviewsRepository.UpdateNumberOfFlagsForReview(existingReviewId, newNumberOfFlags);

            // Assert
            Assert.NotNull(updatedReview);
            Assert.Equal(existingReviewId, updatedReview!.ReviewId);
            Assert.Equal(newNumberOfFlags, updatedReview.NumberOfFlags);
        }

        [Fact]
        public async Task UpdateNumberOfFlagsForReview_WhenReviewDoesNotExist_DoesNotCallUpdateOrSaveChanges()
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
            await reviewsRepository.UpdateNumberOfFlagsForReview(nonExistingReviewId, 10);

            // Assert
            Assert.False(updateCalled);
            Assert.False(saveChangesCalled);
        }
    }
}
