using DataAccess.IRepository;
using Moq;
using WinUIApp.WebAPI.Models;
using WinUIApp.WebAPI.Services;

namespace WinUIApp.Tests.UnitTests.Services.DrinkServiceTests
{
    public class DrinkServiceUpdateDrinkTest
    {
        private readonly Mock<IDrinkRepository> mockDrinkRepository;
        private readonly DrinkService drinkService;

        private readonly Guid testUserId = Guid.NewGuid();
        private readonly DrinkDTO testDrinkDto = new DrinkDTO
        {
            DrinkId = 1,
            DrinkName = "UpdatedDrink",
            // Add other properties as needed
        };

        public DrinkServiceUpdateDrinkTest()
        {
            this.mockDrinkRepository = new Mock<IDrinkRepository>();
            this.drinkService = new DrinkService(this.mockDrinkRepository.Object);

            this.mockDrinkRepository
                .Setup(repo => repo.UpdateDrink(testDrinkDto))
                .Verifiable();
        }

        [Fact]
        public void UpdateDrink_WhenCalled_CallsRepositoryUpdate()
        {
            // Act
            this.drinkService.UpdateDrink(testDrinkDto, testUserId);

            // Assert
            this.mockDrinkRepository.Verify(repo => repo.UpdateDrink(testDrinkDto), Times.Once);
        }

        [Fact]
        public void UpdateDrink_WhenRepositoryThrows_ThrowsWrappedException()
        {
            // Arrange
            this.mockDrinkRepository
                .Setup(repo => repo.UpdateDrink(It.IsAny<DrinkDTO>()))
                .Throws(new Exception("Repository update failed"));

            // Act & Assert
            Exception ex = Assert.Throws<Exception>(() => this.drinkService.UpdateDrink(testDrinkDto, testUserId));
            Assert.Contains("Error happened while updating a drink:", ex.Message);
            Assert.Contains("Repository update failed", ex.InnerException?.Message);
        }
    }
}