using DataAccess.DTOModels;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;

namespace WinUIApp.Tests.UnitTests.Repositories.ReviewsRepositoryTests
{
    public class ReviewsRepositoryUpdateReviewTest
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;
        private readonly Mock<DbSet<Review>> mockReviewDbSet;
        private readonly ReviewsRepository reviewsRepository;
        private readonly List<Review> reviewData;

        public ReviewsRepositoryUpdateReviewTest()
        {
            this.mockAppDbContext = new Mock<IAppDbContext>();

            this.reviewData = new List<Review>
            {
                new Review
                {
                    ReviewId = 1,
                    DrinkId = 10,
                    UserId = new Guid(),
                    RatingValue = 4,
                    Content = "Original Content",
                    CreatedDate = new DateTime(2023, 1, 1),
                    IsActive = true
                }
            };

            this.mockReviewDbSet = this.reviewData.AsQueryable().BuildMockDbSet();

            this.mockAppDbContext
                .Setup(context => context.Reviews)
                .Returns(this.mockReviewDbSet.Object);

            // Setup Find method if needed (not used here since FirstOrDefault is used)
            // Setup SaveChangesAsync
            this.mockAppDbContext
                .Setup(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            this.reviewsRepository = new ReviewsRepository(this.mockAppDbContext.Object);
        }

        [Fact]
        public async Task UpdateReview_WhenReviewExists_ReturnsTrueAndUpdatesFields()
        {
            // Arrange
            int existingReviewId = 1;
            ReviewDTO updatedReviewDto = new()
            {
                ReviewId = existingReviewId,
                DrinkId = 15,
                UserId = new Guid(),
                RatingValue = 5,
                Content = "Updated Content",
                CreatedDate = new DateTime(2024, 6, 1),
                IsHidden = true
            };

            // Act
            bool updateResult = await this.reviewsRepository.UpdateReview(updatedReviewDto);

            // Assert
            Assert.True(updateResult);
            Review updatedReview = this.reviewData.Single(r => r.ReviewId == existingReviewId);
            Assert.Equal(updatedReviewDto.DrinkId, updatedReview.DrinkId);
            Assert.Equal(updatedReviewDto.UserId, updatedReview.UserId);
            Assert.Equal(updatedReviewDto.RatingValue, updatedReview.RatingValue);
            Assert.Equal(updatedReviewDto.Content, updatedReview.Content);
            Assert.Equal(updatedReviewDto.CreatedDate, updatedReview.CreatedDate);
            Assert.Equal(updatedReviewDto.IsHidden, updatedReview.IsActive);
        }

        [Fact]
        public async Task UpdateReview_WhenReviewDoesNotExist_ReturnsFalse()
        {
            // Arrange
            int nonExistingReviewId = 999;
            ReviewDTO nonExistingReviewDto = new()
            {
                ReviewId = nonExistingReviewId,
                DrinkId = 0,
                UserId = new Guid(),
                RatingValue = 0,
                Content = string.Empty,
                CreatedDate = DateTime.MinValue,
                IsHidden = false
            };

            // Act
            bool updateResult = await this.reviewsRepository.UpdateReview(nonExistingReviewDto);

            // Assert
            Assert.False(updateResult);
        }
    }
}
