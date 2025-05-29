using Moq;
using WinUIApp.WebAPI.Models;
using WinUIApp.WebAPI.Repositories;
using WinUIApp.WebAPI.Services;
using WinUiApp.Data.Data;
using Xunit;

namespace WinUIApp.Tests.UnitTests.Services
{
    public class RatingServiceGetRatingByIdTest
    {
        private readonly RatingService service;
        private readonly Mock<IRatingRepository> repository;

        public RatingServiceGetRatingByIdTest()
        {
            repository = new Mock<IRatingRepository>();
            service = new RatingService(repository.Object);
        }

        [Fact]
        public void GetRatingById_WhenRatingExists_ReturnsRating()
        {
            // Arrange
            var ratingId = 1;
            var expectedDataModel = new Rating
            {
                RatingId = ratingId
            };

            repository.Setup(repository => repository.GetRatingById(ratingId))
                       .Returns(expectedDataModel);

            // Act
            var result = service.GetRatingById(ratingId);

            // Assert
            Assert.Equal(ratingId, result.RatingId);
        }

        [Fact]
        public void GetRatingById_WhenRatingDoesNotExist_ReturnsNull()
        {
            // Arrange
            var ratingId = 999;

            repository.Setup(repository => repository.GetRatingById(ratingId))
                       .Returns((Rating)null);

            // Act
            var result = service.GetRatingById(ratingId);

            // Assert
            Assert.Null(result);
        }
    }
}
