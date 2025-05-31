using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUiApp.Data;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;
using Xunit;

namespace WinUIApp.Tests.UnitTests.Repositories.ReviewsRepositoryTests
{
    public class ReviewsRepositoryRemoveReviewByIdTest
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;
        private readonly Mock<DbSet<Review>> mockReviewDbSet;
        private readonly ReviewsRepository reviewsRepository;
        private readonly List<Review> reviewData;

        public ReviewsRepositoryRemoveReviewByIdTest()
        {
            mockAppDbContext = new Mock<IAppDbContext>();

            // Test data
            reviewData = new List<Review>
            {
                new Review { ReviewId = 1, Content = "Test Review 1" },
                new Review { ReviewId = 2, Content = "Test Review 2" }
            };

            // Use MockQueryable to create a proper mock DbSet
            mockReviewDbSet = reviewData.AsQueryable().BuildMockDbSet();

            mockAppDbContext.Setup(context => context.Reviews).Returns(mockReviewDbSet.Object);

            reviewsRepository = new ReviewsRepository(mockAppDbContext.Object);
        }

        [Fact]
        public async Task RemoveReviewById_WhenReviewExists_CallsRemove()
        {
            // Arrange
            Review removedReview = null!;
            int existingReviewId = 1;

            mockReviewDbSet
                .Setup(set => set.Remove(It.IsAny<Review>()))
                .Callback<Review>(review => removedReview = review);

            mockAppDbContext
                .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            await reviewsRepository.RemoveReviewById(existingReviewId);

            // Assert
            Assert.NotNull(removedReview);
            Assert.Equal(existingReviewId, removedReview.ReviewId);
        }

        [Fact]
        public async Task RemoveReviewById_WhenReviewDoesNotExist_DoesNotCallRemove()
        {
            // Arrange
            bool removeCalled = false;
            int nonExistingReviewId = 999;

            // Create a separate mock for empty data
            var emptyData = new List<Review>();
            var emptyMockDbSet = emptyData.AsQueryable().BuildMockDbSet();

            emptyMockDbSet
                .Setup(set => set.Remove(It.IsAny<Review>()))
                .Callback(() => removeCalled = true);

            mockAppDbContext
                .Setup(context => context.Reviews)
                .Returns(emptyMockDbSet.Object);

            var repository = new ReviewsRepository(mockAppDbContext.Object);

            // Act
            await repository.RemoveReviewById(nonExistingReviewId);

            // Assert
            Assert.False(removeCalled);
        }
    }
}
