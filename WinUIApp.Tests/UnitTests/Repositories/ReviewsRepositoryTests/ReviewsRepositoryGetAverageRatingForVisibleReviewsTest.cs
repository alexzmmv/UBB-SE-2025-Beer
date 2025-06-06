﻿using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;

namespace WinUIApp.Tests.UnitTests.Repositories.ReviewsRepositoryTests
{
    public class ReviewsRepositoryGetAverageRatingForVisibleReviewsTest
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;
        private readonly Mock<DbSet<Review>> mockReviewDbSet;
        private readonly ReviewsRepository reviewsRepository;
        private readonly List<Review> reviewData;

        public ReviewsRepositoryGetAverageRatingForVisibleReviewsTest()
        {
            this.mockAppDbContext = new Mock<IAppDbContext>();

            this.reviewData = new List<Review>
            {
                new Review { ReviewId = 1, RatingValue = 4, IsHidden = false },
                new Review { ReviewId = 2, RatingValue = 5, IsHidden = false },
                new Review { ReviewId = 3, RatingValue = null, IsHidden = false },
                new Review { ReviewId = 4, RatingValue = 2, IsHidden = true }
            };

            this.mockReviewDbSet = this.reviewData.AsQueryable().BuildMockDbSet();

            this.mockAppDbContext
                .Setup(context => context.Reviews)
                .Returns(this.mockReviewDbSet.Object);

            this.reviewsRepository = new ReviewsRepository(this.mockAppDbContext.Object);
        }

        [Fact]
        public async Task GetAverageRatingForVisibleReviews_WhenThereAreVisibleReviews_ReturnsRoundedAverage()
        {
            // Arrange
            double expectedAverage = 3.0; // (4 + 5 + 0) / 3 = 3.0

            // Act
            double actualAverage = await this.reviewsRepository.GetAverageRatingForVisibleReviews();

            // Assert
            Assert.Equal(expectedAverage, actualAverage);
        }

        [Fact]
        public async Task GetAverageRatingForVisibleReviews_WhenThereAreNoVisibleReviews_ReturnsZero()
        {
            // Arrange
            List<Review> onlyHiddenData = new()
            {
                new Review { ReviewId = 1, RatingValue = 5, IsHidden = true },
                new Review { ReviewId = 2, RatingValue = 3, IsHidden = true }
            };

            Mock<DbSet<Review>> hiddenMockDbSet = onlyHiddenData.AsQueryable().BuildMockDbSet();

            this.mockAppDbContext
                .Setup(context => context.Reviews)
                .Returns(hiddenMockDbSet.Object);

            ReviewsRepository repositoryWithOnlyHiddenReviews = new(this.mockAppDbContext.Object);

            double expectedAverage = 0.0;

            // Act
            double actualAverage = await repositoryWithOnlyHiddenReviews.GetAverageRatingForVisibleReviews();

            // Assert
            Assert.Equal(expectedAverage, actualAverage);
        }
    }
}
