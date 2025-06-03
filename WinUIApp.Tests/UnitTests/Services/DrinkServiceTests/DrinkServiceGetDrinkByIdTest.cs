using DataAccess.IRepository;
using Moq;
using WinUIApp.WebAPI.Models;
using WinUIApp.WebAPI.Services;

namespace WinUIApp.Tests.UnitTests.Services.DrinkServiceTests
{
    public class DrinkServiceGetDrinkByIdTest
    {
        private readonly Mock<IDrinkRepository> mockDrinkRepository;
        private readonly DrinkService drinkService;

        private const int EXISTING_DRINK_ID = 1;
        private const int NON_EXISTING_DRINK_ID = 999;

        public DrinkServiceGetDrinkByIdTest()
        {
            this.mockDrinkRepository = new Mock<IDrinkRepository>();
            this.drinkService = new DrinkService(this.mockDrinkRepository.Object);

            // Setup for existing drink ID
            this.mockDrinkRepository
                .Setup(repo => repo.GetDrinkById(EXISTING_DRINK_ID))
                .Returns(new DrinkDTO { DrinkId = EXISTING_DRINK_ID });

            // Setup for non-existing drink ID (throws internally)
            this.mockDrinkRepository
                .Setup(repo => repo.GetDrinkById(NON_EXISTING_DRINK_ID))
                .Throws(new Exception("Drink not found"));
        }

        [Fact]
        public void GetDrinkById_WhenCalledWithExistingId_ReturnsDrinkDto()
        {
            // Act
            DrinkDTO? result = this.drinkService.GetDrinkById(EXISTING_DRINK_ID);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(EXISTING_DRINK_ID, result!.DrinkId);
        }

        [Fact]
        public void GetDrinkById_WhenRepositoryThrows_ThrowsWrappedException()
        {
            // Act & Assert
            Exception exception = Assert.Throws<Exception>(() => this.drinkService.GetDrinkById(NON_EXISTING_DRINK_ID));
            Assert.Contains($"Error happened while getting drink with ID {NON_EXISTING_DRINK_ID}", exception.Message);
        }
    }
}