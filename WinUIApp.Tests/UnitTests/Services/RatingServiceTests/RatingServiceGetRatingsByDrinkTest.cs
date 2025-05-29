using Moq;
using WinUiApp.Data.Data;
using WinUIApp.WebAPI.Services;
using System.Collections.Generic;
using Xunit;
using DataAccess.IRepository;

namespace WinUIApp.Tests.UnitTests.Services
{
    public class RatingServiceGetRatingsByDrinkTest
    {
        private readonly RatingService service;
        private readonly Mock<IRatingRepository> repository;

        public RatingServiceGetRatingsByDrinkTest()
        {
            repository = new Mock<IRatingRepository>();
            service = new RatingService(repository.Object);
        }

        private const int DrinkId = 1;

        [Fact]
        public void GetRatingsByDrink_WhenRatingsExist_ReturnsCorrectCount()
        {
            // Arrange
            var ratings = new List<Rating> { new Rating(), new Rating() };
            repository.Setup(repository => repository.GetRatingsByDrinkId(DrinkId)).Returns(ratings);

            // Act
            var result = service.GetRatingsByDrink(DrinkId);

            // Assert
            Assert.Equal(ratings.Count, result.Count());
        }

        [Fact]
        public void GetRatingsByDrink_WhenNoRatingsExist_ReturnsEmptyCollection()
        {
            // Arrange
            repository.Setup(repository => repository.GetRatingsByDrinkId(DrinkId)).Returns(new List<Rating>());

            // Act
            var result = service.GetRatingsByDrink(DrinkId);

            // Assert
            Assert.Empty(result);
        }
    }
}
