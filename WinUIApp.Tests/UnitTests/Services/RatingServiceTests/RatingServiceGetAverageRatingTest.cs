using Moq;
using WinUIApp.WebAPI.Services;
using WinUiApp.Data.Data;
using System.Collections.Generic;
using Xunit;
using DataAccess.IRepository;

namespace WinUIApp.Tests.UnitTests.Services
{
    public class RatingServiceGetAverageRatingTest
    {
        private readonly RatingService service;
        private readonly Mock<IRatingRepository> repository;

        public RatingServiceGetAverageRatingTest()
        {
            repository = new Mock<IRatingRepository>();
            service = new RatingService(repository.Object);
        }

        private const int DrinkId = 1;

        [Fact]
        public void GetAverageRating_WhenActiveRatingsExist_ReturnsCorrectAverage()
        {
            // Arrange
            var firstRatingValue = 3.0f;
            var secondRatingValue = 4.0f;
            var thirdRatingValue = 5.0f;
            var numberOfRatings = 3;
            var ratings = new List<Rating>
            {
                new Rating { RatingValue = firstRatingValue},
                new Rating { RatingValue = secondRatingValue},
                new Rating { RatingValue = thirdRatingValue}
            };
            var expectedAverage = (firstRatingValue + secondRatingValue + thirdRatingValue) / numberOfRatings;
            repository.Setup(repository => repository.GetRatingsByDrinkId(DrinkId)).Returns(ratings);

            // Act
            var result = service.GetAverageRating(DrinkId);

            // Assert
            Assert.Equal(expectedAverage, result);
        }

        [Fact]
        public void GetAverageRating_WhenNoActiveRatingsExist_ReturnsZero()
        {
            // Arrange
            var ratings = new List<Rating>();
            repository.Setup(repository => repository.GetRatingsByDrinkId(DrinkId)).Returns(ratings);

            // Act
            var result = service.GetAverageRating(DrinkId);

            // Assert
            Assert.Equal(0, result);
        }
    }
}
