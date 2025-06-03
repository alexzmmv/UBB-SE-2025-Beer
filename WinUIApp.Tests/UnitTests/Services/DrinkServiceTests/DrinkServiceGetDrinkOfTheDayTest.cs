using DataAccess.IRepository;
using Moq;
using WinUIApp.WebAPI.Models;
using WinUIApp.WebAPI.Services;

namespace WinUIApp.Tests.UnitTests.Services.DrinkServiceTests
{
    public class DrinkServiceGetDrinkOfTheDayTest
    {
        private readonly Mock<IDrinkRepository> mockDrinkRepository;
        private readonly DrinkService drinkService;

        public DrinkServiceGetDrinkOfTheDayTest()
        {
            this.mockDrinkRepository = new Mock<IDrinkRepository>();
            this.drinkService = new DrinkService(this.mockDrinkRepository.Object);

            this.mockDrinkRepository
                .Setup(repo => repo.GetDrinkOfTheDay())
                .Returns(new DrinkDTO { DrinkId = 42 });

            this.mockDrinkRepository
                .Setup(repo => repo.GetDrinkOfTheDay())
                .Callback(() => throw new Exception("Repository failure"));
        }

        [Fact]
        public void GetDrinkOfTheDay_WhenCalled_ReturnsDrinkDto()
        {
            // Arrange
            this.mockDrinkRepository
                .Setup(repo => repo.GetDrinkOfTheDay())
                .Returns(new DrinkDTO { DrinkId = 42 });

            // Act
            DrinkDTO result = this.drinkService.GetDrinkOfTheDay();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(42, result.DrinkId);
        }

        [Fact]
        public void GetDrinkOfTheDay_WhenRepositoryThrows_ThrowsWrappedException()
        {
            // Arrange
            this.mockDrinkRepository
                .Setup(repo => repo.GetDrinkOfTheDay())
                .Throws(new Exception("Repository failure"));

            // Act & Assert
            Exception exception = Assert.Throws<Exception>(() => this.drinkService.GetDrinkOfTheDay());
            Assert.Contains("Error getting drink of the day", exception.Message);
            Assert.Contains("Repository failure", exception.InnerException?.Message);
        }
    }
}
