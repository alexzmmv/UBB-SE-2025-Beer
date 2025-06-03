using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;

namespace WinUIApp.Tests.UnitTests.Repositories.DrinkRepositoryTests
{
    public class DrinkRepositoryAddToPersonalDrinkListTest
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;
        private readonly Mock<DbSet<UserDrink>> mockUserDrinkDbSet;
        private readonly DrinkRepository drinkRepository;
        private readonly Guid existingUserId;
        private readonly Guid nonExistingUserId;
        private readonly int existingDrinkId;
        private readonly int newDrinkId;
        private readonly List<UserDrink> userDrinkData;

        public DrinkRepositoryAddToPersonalDrinkListTest()
        {
            this.mockAppDbContext = new Mock<IAppDbContext>();

            this.existingUserId = Guid.NewGuid();
            this.nonExistingUserId = Guid.NewGuid();

            this.existingDrinkId = 1;
            this.newDrinkId = 999;

            this.userDrinkData = new List<UserDrink>
            {
                new UserDrink { UserId = this.existingUserId, DrinkId = this.existingDrinkId }
            };

            this.mockUserDrinkDbSet = this.userDrinkData.AsQueryable().BuildMockDbSet();

            this.mockAppDbContext.Setup(context => context.UserDrinks).Returns(this.mockUserDrinkDbSet.Object);

            this.mockAppDbContext.Setup(context => context.SaveChanges()).Returns(1);

            this.mockUserDrinkDbSet.Setup(set => set.Add(It.IsAny<UserDrink>())).Callback<UserDrink>(userDrink => this.userDrinkData.Add(userDrink));

            this.drinkRepository = new DrinkRepository(this.mockAppDbContext.Object);
        }

        [Fact]
        public void AddToPersonalDrinkList_WhenEntryAlreadyExists_ReturnsTrueAndDoesNotAdd()
        {
            // Act
            bool result = this.drinkRepository.AddToPersonalDrinkList(this.existingUserId, this.existingDrinkId);

            // Assert
            Assert.True(result);
            // Verify that Add was never called because entry exists
            this.mockUserDrinkDbSet.Verify(set => set.Add(It.IsAny<UserDrink>()), Times.Never);
            this.mockAppDbContext.Verify(context => context.SaveChanges(), Times.Never);
        }

        [Fact]
        public void AddToPersonalDrinkList_WhenEntryDoesNotExist_AddsAndReturnsTrue()
        {
            // Act
            bool result = this.drinkRepository.AddToPersonalDrinkList(this.existingUserId, this.newDrinkId);

            // Assert
            Assert.True(result);
            Assert.Contains(this.userDrinkData, ud => ud.UserId == this.existingUserId && ud.DrinkId == this.newDrinkId);
            this.mockUserDrinkDbSet.Verify(set => set.Add(It.IsAny<UserDrink>()), Times.Once);
            this.mockAppDbContext.Verify(context => context.SaveChanges(), Times.Once);
        }
    }
}