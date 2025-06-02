using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;
using WinUIApp.WebAPI.Models;

namespace WinUIApp.Tests.UnitTests.Repositories.DrinkRepositoryTests
{
    public class DrinkRepositoryGetPersonalDrinkListTest
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;
        private readonly Mock<DbSet<UserDrink>> mockUserDrinkDbSet;
        private readonly Mock<DbSet<Drink>> mockDrinkDbSet;
        private readonly DrinkRepository drinkRepository;
        private readonly Guid existingUserId;
        private readonly Guid nonExistingUserId;
        private readonly List<UserDrink> userDrinkData;
        private readonly List<Drink> drinkData;

        public DrinkRepositoryGetPersonalDrinkListTest()
        {
            this.mockAppDbContext = new Mock<IAppDbContext>();

            this.existingUserId = Guid.NewGuid();
            this.nonExistingUserId = Guid.NewGuid();

            // Setup UserDrinks test data
            this.userDrinkData = new List<UserDrink>
            {
                new UserDrink { UserId = this.existingUserId, DrinkId = 1 },
                new UserDrink { UserId = this.existingUserId, DrinkId = 2 }
            };

            // Setup Drink test data (only approved drinks)
            this.drinkData = new List<Drink>
            {
                new Drink
                {
                    DrinkId = 1,
                    IsRequestingApproval = false,
                    Brand = new Brand { /* initialize as needed */ },
                    DrinkCategories = new List<DrinkCategory>
                    {
                        new DrinkCategory
                        {
                            Category = new Category { /* initialize as needed */ }
                        }
                    }
                },
                new Drink
                {
                    DrinkId = 2,
                    IsRequestingApproval = false,
                    Brand = new Brand { /* initialize as needed */ },
                    DrinkCategories = new List<DrinkCategory>
                    {
                        new DrinkCategory
                        {
                            Category = new Category { /* initialize as needed */ }
                        }
                    }
                },
                new Drink
                {
                    DrinkId = 3,
                    IsRequestingApproval = true, // should be excluded
                    Brand = new Brand { /* initialize as needed */ },
                    DrinkCategories = new List<DrinkCategory>()
                }
            };

            this.mockUserDrinkDbSet = this.userDrinkData.AsQueryable().BuildMockDbSet();
            this.mockDrinkDbSet = this.drinkData.AsQueryable().BuildMockDbSet();

            this.mockAppDbContext.Setup(context => context.UserDrinks).Returns(this.mockUserDrinkDbSet.Object);
            this.mockAppDbContext.Setup(context => context.Drinks).Returns(this.mockDrinkDbSet.Object);

            this.drinkRepository = new DrinkRepository(this.mockAppDbContext.Object);
        }

        [Fact]
        public void GetPersonalDrinkList_WhenUserHasDrinks_ReturnsApprovedDrinksOnly()
        {
            // Act
            List<DrinkDTO> personalDrinkList = this.drinkRepository.GetPersonalDrinkList(this.existingUserId);

            // Assert
            Assert.All(personalDrinkList, dto => Assert.False(dto.IsRequestingApproval));
            Assert.Equal(2, personalDrinkList.Count);
        }

        [Fact]
        public void GetPersonalDrinkList_WhenUserHasNoDrinks_ReturnsEmptyList()
        {
            // Act
            List<DrinkDTO> personalDrinkList = this.drinkRepository.GetPersonalDrinkList(this.nonExistingUserId);

            // Assert
            Assert.Empty(personalDrinkList);
        }
    }
}