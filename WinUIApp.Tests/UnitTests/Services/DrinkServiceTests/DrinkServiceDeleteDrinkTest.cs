using DataAccess.IRepository;
using Moq;
using WinUIApp.WebAPI.Services;

namespace WinUIApp.Tests.UnitTests.Services.DrinkServiceTests
{
    public class DrinkServiceDeleteDrinkTest
    {
        private readonly Mock<IDrinkRepository> mockDrinkRepository;
        private readonly DrinkService drinkService;

        private const int TestDrinkId = 1;
        private readonly Guid TestUserId = Guid.NewGuid();

        public DrinkServiceDeleteDrinkTest()
        {
            this.mockDrinkRepository = new Mock<IDrinkRepository>();
            this.drinkService = new DrinkService(this.mockDrinkRepository.Object);

            this.mockDrinkRepository
                .Setup(repo => repo.DeleteDrink(TestDrinkId))
                .Verifiable();
        }

        [Fact]
        public void DeleteDrink_WhenCalled_CallsRepositoryDelete()
        {
            // Act
            this.drinkService.DeleteDrink(TestDrinkId, TestUserId);

            // Assert
            this.mockDrinkRepository.Verify(repo => repo.DeleteDrink(TestDrinkId), Times.Once);
        }

        [Fact]
        public void DeleteDrink_WhenRepositoryThrows_ThrowsWrappedException()
        {
            // Arrange
            this.mockDrinkRepository
                .Setup(repo => repo.DeleteDrink(It.IsAny<int>()))
                .Throws(new Exception("Repository delete failed"));

            // Act & Assert
            Exception ex = Assert.Throws<Exception>(() => this.drinkService.DeleteDrink(TestDrinkId, TestUserId));
            Assert.Contains("Error happened while deleting a drink:", ex.Message);
            Assert.Contains("Repository delete failed", ex.InnerException?.Message);
        }
    }
}