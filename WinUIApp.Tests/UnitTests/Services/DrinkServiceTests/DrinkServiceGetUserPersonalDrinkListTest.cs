using DataAccess.IRepository;
using Moq;
using WinUIApp.WebAPI.Models;
using WinUIApp.WebAPI.Services;

namespace WinUIApp.Tests.UnitTests.Services.DrinkServiceTests
{
    public class DrinkServiceGetUserPersonalDrinkListTest
    {
        private readonly Mock<IDrinkRepository> mockDrinkRepository;
        private readonly DrinkService drinkService;
        private readonly Guid existingUserId;
        private readonly Guid nonExistingUserId;
        private readonly List<DrinkDTO> personalDrinkList;

        public DrinkServiceGetUserPersonalDrinkListTest()
        {
            this.mockDrinkRepository = new Mock<IDrinkRepository>();
            this.drinkService = new DrinkService(this.mockDrinkRepository.Object);

            this.existingUserId = Guid.NewGuid();
            this.nonExistingUserId = Guid.NewGuid();

            this.personalDrinkList = new List<DrinkDTO>
            {
                new DrinkDTO { DrinkId = 1, DrinkName = "Drink A" },
                new DrinkDTO { DrinkId = 2, DrinkName = "Drink B" }
            };

            this.mockDrinkRepository
                .Setup(repo => repo.GetPersonalDrinkList(existingUserId))
                .Returns(this.personalDrinkList);

            this.mockDrinkRepository
                .Setup(repo => repo.GetPersonalDrinkList(nonExistingUserId))
                .Returns(new List<DrinkDTO>());
        }

        [Fact]
        public void GetUserPersonalDrinkList_WhenPersonalDrinksExist_ReturnsDrinkList()
        {
            // Act
            List<DrinkDTO> result = this.drinkService.GetUserPersonalDrinkList(existingUserId);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
            this.mockDrinkRepository.Verify(repo => repo.GetPersonalDrinkList(existingUserId), Times.Once);
        }

        [Fact]
        public void GetUserPersonalDrinkList_WhenNoPersonalDrinks_ReturnsEmptyList()
        {
            // Act
            List<DrinkDTO> result = this.drinkService.GetUserPersonalDrinkList(nonExistingUserId);

            // Assert
            Assert.Empty(result);
            this.mockDrinkRepository.Verify(repo => repo.GetPersonalDrinkList(nonExistingUserId), Times.Once);
        }

        [Fact]
        public void GetUserPersonalDrinkList_WhenRepositoryThrows_ThrowsWrappedException()
        {
            // Arrange
            this.mockDrinkRepository
                .Setup(repo => repo.GetPersonalDrinkList(It.IsAny<Guid>()))
                .Throws(new Exception("Repository error"));

            // Act & Assert
            Exception ex = Assert.Throws<Exception>(() => this.drinkService.GetUserPersonalDrinkList(Guid.NewGuid()));
            Assert.Contains("Error getting personal drink list", ex.Message);
            Assert.Contains("Repository error", ex.InnerException?.Message);
        }
    }
}
