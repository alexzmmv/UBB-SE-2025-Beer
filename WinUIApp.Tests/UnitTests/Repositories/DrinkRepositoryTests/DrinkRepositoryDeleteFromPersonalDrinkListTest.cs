using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;

namespace WinUIApp.Tests.UnitTests.Repositories.DrinkRepositoryTests
{
    public class DrinkRepositoryDeleteFromPersonalDrinkListTest
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;
        private readonly Mock<DbSet<UserDrink>> mockUserDrinkDbSet;
        private readonly DrinkRepository drinkRepository;
        private readonly Guid existingUserId;
        private readonly Guid nonExistingUserId;
        private readonly int existingDrinkId;
        private readonly int nonExistingDrinkId;
        private readonly List<UserDrink> userDrinkData;

        public DrinkRepositoryDeleteFromPersonalDrinkListTest()
        {
            this.mockAppDbContext = new Mock<IAppDbContext>();

            this.existingUserId = Guid.NewGuid();
            this.nonExistingUserId = Guid.NewGuid();

            this.existingDrinkId = 1;
            this.nonExistingDrinkId = 999;

            this.userDrinkData = new List<UserDrink>
            {
                new UserDrink { UserId = this.existingUserId, DrinkId = this.existingDrinkId }
            };

            this.mockUserDrinkDbSet = this.userDrinkData.AsQueryable().BuildMockDbSet();

            this.mockAppDbContext.Setup(context => context.UserDrinks).Returns(this.mockUserDrinkDbSet.Object);

            this.mockUserDrinkDbSet
                .Setup(set => set.Remove(It.IsAny<UserDrink>()))
                .Callback<UserDrink>(userDrink => this.userDrinkData.Remove(userDrink));

            this.mockAppDbContext.Setup(context => context.SaveChanges()).Returns(1);

            this.drinkRepository = new DrinkRepository(this.mockAppDbContext.Object);
        }

        [Fact]
        public void DeleteFromPersonalDrinkList_WhenEntryExists_RemovesEntryAndReturnsTrue()
        {
            // Act
            bool result = this.drinkRepository.DeleteFromPersonalDrinkList(this.existingUserId, this.existingDrinkId);

            // Assert
            Assert.True(result);
            Assert.DoesNotContain(this.userDrinkData, ud => ud.UserId == this.existingUserId && ud.DrinkId == this.existingDrinkId);
            this.mockUserDrinkDbSet.Verify(set => set.Remove(It.IsAny<UserDrink>()), Times.Once);
            this.mockAppDbContext.Verify(context => context.SaveChanges(), Times.Once);
        }

        [Fact]
        public void DeleteFromPersonalDrinkList_WhenEntryDoesNotExist_ReturnsTrueAndDoesNotRemove()
        {
            // Act
            bool result = this.drinkRepository.DeleteFromPersonalDrinkList(this.nonExistingUserId, this.nonExistingDrinkId);

            // Assert
            Assert.True(result);
            this.mockUserDrinkDbSet.Verify(set => set.Remove(It.IsAny<UserDrink>()), Times.Never);
            this.mockAppDbContext.Verify(context => context.SaveChanges(), Times.Never);
        }
    }
}