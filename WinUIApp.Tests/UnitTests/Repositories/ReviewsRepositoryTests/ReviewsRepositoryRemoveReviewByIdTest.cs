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
            this.mockAppDbContext = new Mock<IAppDbContext>();

            // Test data
            this.reviewData = new List<Review>
            {
                new Review { ReviewId = 1, Content = "Test Review 1" },
                new Review { ReviewId = 2, Content = "Test Review 2" }
            };

            // Use MockQueryable to create a proper mock DbSet
            this.mockReviewDbSet = this.reviewData.AsQueryable().BuildMockDbSet();

            this.mockAppDbContext.Setup(context => context.Reviews).Returns(this.mockReviewDbSet.Object);

            this.reviewsRepository = new ReviewsRepository(this.mockAppDbContext.Object);
        }

        [Fact]
        public async Task RemoveReviewById_WhenReviewExists_CallsRemove()
        {
            // Arrange
            Review removedReview = null!;
            int existingReviewId = 1;

            this.mockReviewDbSet
                .Setup(set => set.Remove(It.IsAny<Review>()))
                .Callback<Review>(review => removedReview = review);

            this.mockAppDbContext
                .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            await this.reviewsRepository.RemoveReviewById(existingReviewId);

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

            List<Review> emptyData = [];
            Mock<DbSet<Review>> emptyMockDbSet = emptyData.AsQueryable().BuildMockDbSet();

            emptyMockDbSet
                .Setup(set => set.Remove(It.IsAny<Review>()))
                .Callback(() => removeCalled = true);

            this.mockAppDbContext
                .Setup(context => context.Reviews)
                .Returns(emptyMockDbSet.Object);

            ReviewsRepository repository = new(this.mockAppDbContext.Object);

            // Act
            await repository.RemoveReviewById(nonExistingReviewId);

            // Assert
            Assert.False(removeCalled);
        }
    }
}
