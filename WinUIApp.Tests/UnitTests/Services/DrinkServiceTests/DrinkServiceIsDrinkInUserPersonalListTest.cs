using DataAccess.IRepository;
using Moq;
using WinUIApp.WebAPI.Services;

namespace WinUIApp.Tests.UnitTests.Services.DrinkServiceTests
{
    public class DrinkServiceIsDrinkInUserPersonalListTest
    {
        private readonly Mock<IDrinkRepository> mockDrinkRepository;
        private readonly DrinkService drinkService;
        private readonly Guid existingUserId;
        private readonly Guid nonExistingUserId;
        private const int ExistingDrinkId = 1;
        private const int NonExistingDrinkId = 999;

        public DrinkServiceIsDrinkInUserPersonalListTest()
        {
            this.mockDrinkRepository = new Mock<IDrinkRepository>();
            this.drinkService = new DrinkService(this.mockDrinkRepository.Object);

            this.existingUserId = Guid.NewGuid();
            this.nonExistingUserId = Guid.NewGuid();

            this.mockDrinkRepository
                .Setup(repo => repo.IsDrinkInPersonalList(existingUserId, ExistingDrinkId))
                .Returns(true);

            this.mockDrinkRepository
                .Setup(repo => repo.IsDrinkInPersonalList(existingUserId, NonExistingDrinkId))
                .Returns(false);

            this.mockDrinkRepository
                .Setup(repo => repo.IsDrinkInPersonalList(nonExistingUserId, It.IsAny<int>()))
                .Returns(false);
        }

        [Fact]
        public void IsDrinkInUserPersonalList_WhenDrinkExistsInList_ReturnsTrue()
        {
            // Act
            bool result = this.drinkService.IsDrinkInUserPersonalList(existingUserId, ExistingDrinkId);

            // Assert
            Assert.True(result);
            this.mockDrinkRepository.Verify(repo => repo.IsDrinkInPersonalList(existingUserId, ExistingDrinkId), Times.Once);
        }

        [Fact]
        public void IsDrinkInUserPersonalList_WhenDrinkDoesNotExistInList_ReturnsFalse()
        {
            // Act
            bool result = this.drinkService.IsDrinkInUserPersonalList(existingUserId, NonExistingDrinkId);

            // Assert
            Assert.False(result);
            this.mockDrinkRepository.Verify(repo => repo.IsDrinkInPersonalList(existingUserId, NonExistingDrinkId), Times.Once);
        }

        [Fact]
        public void IsDrinkInUserPersonalList_WhenRepositoryThrows_ThrowsWrappedException()
        {
            // Arrange
            this.mockDrinkRepository
                .Setup(repo => repo.IsDrinkInPersonalList(It.IsAny<Guid>(), It.IsAny<int>()))
                .Throws(new Exception("Repository failure"));

            // Act & Assert
            Exception ex = Assert.Throws<Exception>(() => this.drinkService.IsDrinkInUserPersonalList(Guid.NewGuid(), ExistingDrinkId));
            Assert.Contains("Error checking if the drink is in the user's personal list", ex.Message);
            Assert.Contains("Repository failure", ex.InnerException?.Message);
        }
    }
}
