using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;
using WinUIApp.Tests.TestHelpers;

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
            mockReviewDbSet = new Mock<DbSet<Review>>();

            // Test data
            reviewData = new List<Review>
            {
                new Review { ReviewId = 1, Content = "Test Review 1" },
                new Review { ReviewId = 2, Content = "Test Review 2" }
            };

            var asyncData = new TestAsyncEnumerable<Review>(reviewData);

            mockReviewDbSet.As<IAsyncEnumerable<Review>>()
                .Setup(d => d.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<Review>(reviewData.GetEnumerator()));

            mockReviewDbSet.As<IQueryable<Review>>().Setup(m => m.Provider).Returns(asyncData.Provider);
            mockReviewDbSet.As<IQueryable<Review>>().Setup(m => m.Expression).Returns(asyncData.Expression);
            mockReviewDbSet.As<IQueryable<Review>>().Setup(m => m.ElementType).Returns(asyncData.ElementType);
            mockReviewDbSet.As<IQueryable<Review>>().Setup(m => m.GetEnumerator()).Returns(() => asyncData.GetEnumerator());


            mockAppDbContext.Setup(context => context.Reviews).Returns(mockReviewDbSet.Object);

            reviewsRepository = new ReviewsRepository(mockAppDbContext.Object);
        }

        [Fact]
        public async Task RemoveReviewById_WhenReviewExists_CallsRemove()
        {
            // Arrange
            Review removedReview = null!;
            mockReviewDbSet
                .Setup(set => set.Remove(It.IsAny<Review>()))
                .Callback<Review>(r => removedReview = r);

            mockAppDbContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            await reviewsRepository.RemoveReviewById(1);

            // Assert
            Assert.NotNull(removedReview);
            Assert.Equal(1, removedReview.ReviewId);
        }

        [Fact]
        public async Task RemoveReviewById_WhenReviewExists_CallsSaveChanges()
        {
            // Arrange
            bool saveChangesCalled = false;
            mockReviewDbSet.Setup(set => set.Remove(It.IsAny<Review>()));
            mockAppDbContext
                .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Callback(() => saveChangesCalled = true)
                .ReturnsAsync(1);

            // Act
            await reviewsRepository.RemoveReviewById(1);

            // Assert
            Assert.True(saveChangesCalled);
        }

        [Fact]
        public async Task RemoveReviewById_WhenReviewDoesNotExist_DoesNotCallRemoveOrSaveChanges()
        {
            // Arrange
            var asyncData = new TestAsyncEnumerable<Review>(reviewData);

            mockReviewDbSet.As<IAsyncEnumerable<Review>>()
                .Setup(d => d.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<Review>(reviewData.GetEnumerator()));

            mockReviewDbSet.As<IQueryable<Review>>().Setup(m => m.Provider).Returns(asyncData.Provider);
            mockReviewDbSet.As<IQueryable<Review>>().Setup(m => m.Expression).Returns(asyncData.Expression);
            mockReviewDbSet.As<IQueryable<Review>>().Setup(m => m.ElementType).Returns(asyncData.ElementType);
            mockReviewDbSet.As<IQueryable<Review>>().Setup(m => m.GetEnumerator()).Returns(() => asyncData.GetEnumerator());

            bool removeCalled = false;
            bool saveCalled = false;

            mockReviewDbSet.Setup(set => set.Remove(It.IsAny<Review>())).Callback(() => removeCalled = true);
            mockAppDbContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).Callback(() => saveCalled = true).ReturnsAsync(0);

            // Act
            await reviewsRepository.RemoveReviewById(999); // Non-existing review

            // Assert
            Assert.False(removeCalled);
            Assert.False(saveCalled);
        }
    }
}
