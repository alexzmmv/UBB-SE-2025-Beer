using DataAccess.DTOModels;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;

namespace WinUIApp.Tests.UnitTests.Repositories.ReviewsRepositoryTests
{
    public class ReviewsRepositoryAddReviewTest
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;
        private readonly Mock<DbSet<Review>> mockReviewDbSet;
        private readonly ReviewsRepository reviewsRepository;
        private readonly ReviewDTO testReviewDto;

        public ReviewsRepositoryAddReviewTest()
        {
            this.mockAppDbContext = new Mock<IAppDbContext>();
            this.mockReviewDbSet = new Mock<DbSet<Review>>();

            this.mockAppDbContext
                .Setup(context => context.Reviews)
                .Returns(this.mockReviewDbSet.Object);

            this.reviewsRepository = new ReviewsRepository(this.mockAppDbContext.Object);

            this.testReviewDto = new ReviewDTO
            {
                ReviewId = 1,
                UserId = Guid.NewGuid(),
                DrinkId = 2,
                Content = "Test review",
                CreatedDate = DateTime.UtcNow,
                RatingValue = 4.5f,
                NumberOfFlags = 0,
                IsHidden = false
            };
        }

        [Fact]
        public async Task AddReview_WhenCalled_AddsReviewToDatabase()
        {
            // Arrange
            bool wasAddCalled = false;
            this.mockReviewDbSet
                .Setup(set => set.AddAsync(It.IsAny<Review>(), It.IsAny<CancellationToken>()))
                .Callback(() => wasAddCalled = true)
                .Returns((Review review, CancellationToken _) => ValueTask.FromResult((EntityEntry<Review>)null));

            this.mockAppDbContext
                .Setup(context => context.SaveChangesAsync())
                .ReturnsAsync(1);

            // Act
            await this.reviewsRepository.AddReview(this.testReviewDto);

            // Assert
            Assert.True(wasAddCalled);
        }

        [Fact]
        public async Task AddReview_WhenCalled_CallsSaveChanges()
        {
            // Arrange
            bool wasSaveChangesCalled = false;
            this.mockReviewDbSet
                .Setup(set => set.AddAsync(It.IsAny<Review>(), It.IsAny<CancellationToken>()))
                .Returns((Review review, CancellationToken _) => ValueTask.FromResult((EntityEntry<Review>)null));

            this.mockAppDbContext
                .Setup(context => context.SaveChangesAsync())
                .Callback(() => wasSaveChangesCalled = true)
                .ReturnsAsync(1);

            // Act
            await this.reviewsRepository.AddReview(this.testReviewDto);

            // Assert
            Assert.True(wasSaveChangesCalled);
        }

        [Fact]
        public async Task AddReview_WhenCalled_ReturnsCorrectReviewId()
        {
            // Arrange
            int expectedReviewId = 123;
            this.mockReviewDbSet
                .Setup(set => set.AddAsync(It.IsAny<Review>(), It.IsAny<CancellationToken>()))
                .Callback<Review, CancellationToken>((review, _) => review.ReviewId = expectedReviewId)
                .Returns((Review review, CancellationToken _) => ValueTask.FromResult((EntityEntry<Review>)null));

            this.mockAppDbContext
                .Setup(context => context.SaveChangesAsync())
                .ReturnsAsync(1);

            // Act
            int actualReviewId = await this.reviewsRepository.AddReview(this.testReviewDto);

            // Assert
            Assert.Equal(expectedReviewId, actualReviewId);
        }
    }
}
