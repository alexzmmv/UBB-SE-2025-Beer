using DataAccess.IRepository;
using Moq;
using WinUIApp.WebAPI.Services;

namespace WinUIApp.Tests.UnitTests.Services.DrinkServiceTests
{
    public class DrinkServiceDeleteFromUserPersonalDrinkListTest
    {
        private readonly Mock<IDrinkRepository> mockDrinkRepository;
        private readonly DrinkService drinkService;
        private readonly Guid userId;
        private const int ExistingDrinkId = 1;
        private const int NonExistingDrinkId = 999;

        public DrinkServiceDeleteFromUserPersonalDrinkListTest()
        {
            this.mockDrinkRepository = new Mock<IDrinkRepository>();
            this.drinkService = new DrinkService(this.mockDrinkRepository.Object);

            this.userId = Guid.NewGuid();

            this.mockDrinkRepository
                .Setup(repo => repo.DeleteFromPersonalDrinkList(userId, ExistingDrinkId))
                .Returns(true);

            this.mockDrinkRepository
                .Setup(repo => repo.DeleteFromPersonalDrinkList(userId, NonExistingDrinkId))
                .Returns(false);
        }

        [Fact]
        public void DeleteFromUserPersonalDrinkList_WhenDeleteSucceeds_ReturnsTrue()
        {
            // Act
            bool result = this.drinkService.DeleteFromUserPersonalDrinkList(userId, ExistingDrinkId);

            // Assert
            Assert.True(result);
            this.mockDrinkRepository.Verify(repo => repo.DeleteFromPersonalDrinkList(userId, ExistingDrinkId), Times.Once);
        }

        [Fact]
        public void DeleteFromUserPersonalDrinkList_WhenDeleteFails_ReturnsFalse()
        {
            // Act
            bool result = this.drinkService.DeleteFromUserPersonalDrinkList(userId, NonExistingDrinkId);

            // Assert
            Assert.False(result);
            this.mockDrinkRepository.Verify(repo => repo.DeleteFromPersonalDrinkList(userId, NonExistingDrinkId), Times.Once);
        }

        [Fact]
        public void DeleteFromUserPersonalDrinkList_WhenRepositoryThrows_ThrowsWrappedException()
        {
            // Arrange
            this.mockDrinkRepository
                .Setup(repo => repo.DeleteFromPersonalDrinkList(It.IsAny<Guid>(), It.IsAny<int>()))
                .Throws(new Exception("Repository failure"));

            // Act & Assert
            Exception ex = Assert.Throws<Exception>(() => this.drinkService.DeleteFromUserPersonalDrinkList(userId, ExistingDrinkId));
            Assert.Contains("Error deleting drink from personal list", ex.Message);
            Assert.Contains("Repository failure", ex.InnerException?.Message);
        }
    }
}
