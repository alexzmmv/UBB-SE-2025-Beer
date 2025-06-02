using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;

namespace WinUIApp.Tests.UnitTests.Repositories.DrinkRepositoryTests
{
    public class DrinkRepositoryGetRandomDrinkIdTest
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;
        private readonly Mock<DbSet<Drink>> mockDrinkDbSet;
        private readonly DrinkRepository drinkRepository;
        private readonly List<Drink> drinkData;

        public DrinkRepositoryGetRandomDrinkIdTest()
        {
            this.mockAppDbContext = new Mock<IAppDbContext>();

            this.drinkData = new List<Drink>
            {
                new Drink { DrinkId = 1, IsRequestingApproval = false },
                new Drink { DrinkId = 2, IsRequestingApproval = false },
                new Drink { DrinkId = 3, IsRequestingApproval = true } // Should be ignored
            };

            this.mockDrinkDbSet = this.drinkData.AsQueryable().BuildMockDbSet();

            this.mockAppDbContext.Setup(context => context.Drinks).Returns(this.mockDrinkDbSet.Object);

            this.drinkRepository = new DrinkRepository(this.mockAppDbContext.Object);
        }

        [Fact]
        public void GetRandomDrinkId_WhenApprovedDrinksExist_ReturnsDrinkId()
        {
            // Act
            int randomDrinkId = this.drinkRepository.GetRandomDrinkId();

            // Assert
            Assert.Contains(randomDrinkId, new[] { 1, 2 });
        }

        [Fact]
        public void GetRandomDrinkId_WhenNoApprovedDrink_ThrowsException()
        {
            // Arrange
            List<Drink> emptyApprovedDrinks = new List<Drink>
            {
                new Drink { DrinkId = 1, IsRequestingApproval = true }
            };
            Mock<DbSet<Drink>> emptyMockDrinkDbSet = emptyApprovedDrinks.AsQueryable().BuildMockDbSet();

            this.mockAppDbContext.Setup(context => context.Drinks).Returns(emptyMockDrinkDbSet.Object);

            DrinkRepository repositoryWithNoApprovedDrink = new DrinkRepository(this.mockAppDbContext.Object);

            // Act & Assert
            Exception exception = Assert.Throws<Exception>(() => repositoryWithNoApprovedDrink.GetRandomDrinkId());
            Assert.Equal("No approved drink found in the database.", exception.Message);
        }
    }
}