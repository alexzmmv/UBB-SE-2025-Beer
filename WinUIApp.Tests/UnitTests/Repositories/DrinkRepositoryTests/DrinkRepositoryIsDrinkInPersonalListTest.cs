using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;

namespace WinUIApp.Tests.UnitTests.Repositories.DrinkRepositoryTests
{
    public class DrinkRepositoryIsDrinkInPersonalListTest
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;
        private readonly Mock<DbSet<UserDrink>> mockUserDrinkDbSet;
        private readonly DrinkRepository drinkRepository;
        private readonly Guid existingUserId;
        private readonly Guid nonExistingUserId;
        private readonly int existingDrinkId;
        private readonly int nonExistingDrinkId;
        private readonly List<UserDrink> userDrinkData;

        public DrinkRepositoryIsDrinkInPersonalListTest()
        {
            this.mockAppDbContext = new Mock<IAppDbContext>();

            this.existingUserId = Guid.NewGuid();
            this.nonExistingUserId = Guid.NewGuid();

            this.existingDrinkId = 1;
            this.nonExistingDrinkId = 999;

            this.userDrinkData = new List<UserDrink>
            {
                new UserDrink { UserId = this.existingUserId, DrinkId = this.existingDrinkId },
                new UserDrink { UserId = this.existingUserId, DrinkId = 2 }
            };

            this.mockUserDrinkDbSet = this.userDrinkData.AsQueryable().BuildMockDbSet();

            this.mockAppDbContext.Setup(context => context.UserDrinks).Returns(this.mockUserDrinkDbSet.Object);

            this.drinkRepository = new DrinkRepository(this.mockAppDbContext.Object);
        }

        [Fact]
        public void IsDrinkInPersonalList_WhenDrinkExists_ReturnsTrue()
        {
            // Act
            bool isInList = this.drinkRepository.IsDrinkInPersonalList(this.existingUserId, this.existingDrinkId);

            // Assert
            Assert.True(isInList);
        }

        [Fact]
        public void IsDrinkInPersonalList_WhenDrinkDoesNotExist_ReturnsFalse()
        {
            // Act
            bool isInList = this.drinkRepository.IsDrinkInPersonalList(this.existingUserId, this.nonExistingDrinkId);

            // Assert
            Assert.False(isInList);
        }

        [Fact]
        public void IsDrinkInPersonalList_WhenUserDoesNotExist_ReturnsFalse()
        {
            // Act
            bool isInList = this.drinkRepository.IsDrinkInPersonalList(this.nonExistingUserId, this.existingDrinkId);

            // Assert
            Assert.False(isInList);
        }
    }
}