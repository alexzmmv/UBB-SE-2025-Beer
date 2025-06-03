using DataAccess.IRepository;
using Moq;
using WinUIApp.WebAPI.Services;

namespace WinUIApp.Tests.UnitTests.Services.DrinkServiceTests
{
    public class DrinkServiceAddToUserPersonalDrinkListTest
    {
        private readonly Mock<IDrinkRepository> mockDrinkRepository;
        private readonly DrinkService drinkService;
        private readonly Guid userId;
        private const int ExistingDrinkId = 1;
        private const int NonExistingDrinkId = 999;

        public DrinkServiceAddToUserPersonalDrinkListTest()
        {
            this.mockDrinkRepository = new Mock<IDrinkRepository>();
            this.drinkService = new DrinkService(this.mockDrinkRepository.Object);

            this.userId = Guid.NewGuid();

            this.mockDrinkRepository
                .Setup(repo => repo.AddToPersonalDrinkList(userId, ExistingDrinkId))
                .Returns(true);

            this.mockDrinkRepository
                .Setup(repo => repo.AddToPersonalDrinkList(userId, NonExistingDrinkId))
                .Returns(false);
        }

        [Fact]
        public void AddToUserPersonalDrinkList_WhenAddSucceeds_ReturnsTrue()
        {
            // Act
            bool result = this.drinkService.AddToUserPersonalDrinkList(userId, ExistingDrinkId);

            // Assert
            Assert.True(result);
            this.mockDrinkRepository.Verify(repo => repo.AddToPersonalDrinkList(userId, ExistingDrinkId), Times.Once);
        }

        [Fact]
        public void AddToUserPersonalDrinkList_WhenAddFails_ReturnsFalse()
        {
            // Act
            bool result = this.drinkService.AddToUserPersonalDrinkList(userId, NonExistingDrinkId);

            // Assert
            Assert.False(result);
            this.mockDrinkRepository.Verify(repo => repo.AddToPersonalDrinkList(userId, NonExistingDrinkId), Times.Once);
        }

        [Fact]
        public void AddToUserPersonalDrinkList_WhenRepositoryThrows_ThrowsWrappedException()
        {
            // Arrange
            this.mockDrinkRepository
                .Setup(repo => repo.AddToPersonalDrinkList(It.IsAny<Guid>(), It.IsAny<int>()))
                .Throws(new Exception("Repository failure"));

            // Act & Assert
            Exception ex = Assert.Throws<Exception>(() => this.drinkService.AddToUserPersonalDrinkList(userId, ExistingDrinkId));
            Assert.Contains("Error adding drink to personal list", ex.Message);
            Assert.Contains("Repository failure", ex.InnerException?.Message);
        }
    }
}
