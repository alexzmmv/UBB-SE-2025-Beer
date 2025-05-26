using Moq;
using WinUiApp.Data.Data;
using WinUIApp.WebAPI.Repositories;
using WinUIApp.WebAPI.Services;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace WinUIApp.Tests.UnitTests.Services
{
    public class ReviewServiceGetReviewsByRatingIdTest
    {
        private readonly ReviewService service;
        private readonly Mock<IReviewRepository> repository;

        public ReviewServiceGetReviewsByRatingIdTest()
        {
            repository = new Mock<IReviewRepository>();
            service = new ReviewService(repository.Object);
        }

        [Fact]
        public void GetReviewsByRating_WhenCalled_ReturnsCorrectReviewCount()
        {
            // Arrange
            var ratingId = 1;
            var expectedReviews = new List<Review>
            {
                new Review { RatingId = ratingId, Content = "Nice." },
                new Review { RatingId = ratingId, Content = "Loved it!" }
            };

            repository.Setup(repository => repository.GetReviewsByRatingId(ratingId)).Returns(expectedReviews);

            // Act
            var result = service.GetReviewsByRating(ratingId);

            // Assert
            Assert.Equal(expectedReviews.Count, result.Count());
        }
    }
}
