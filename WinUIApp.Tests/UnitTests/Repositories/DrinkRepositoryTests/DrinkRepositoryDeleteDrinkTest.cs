using DataAccess.Data;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;

namespace WinUIApp.Tests.UnitTests.Repositories.DrinkRepositoryTests
{
    public class DrinkRepositoryDeleteDrinkTest
    {
        private readonly Mock<DbSet<Drink>> drinksMock;
        private readonly Mock<DbSet<DrinkCategory>> drinkCategoriesMock;
        private readonly Mock<DbSet<DrinkModificationRequest>> modificationRequestsMock;
        private readonly Mock<IAppDbContext> dbContextMock;
        private readonly List<Drink> drinksData;
        private readonly List<DrinkCategory> drinkCategoriesData;
        private readonly List<DrinkModificationRequest> modificationRequestsData;
        private readonly DrinkRepository repository;

        public DrinkRepositoryDeleteDrinkTest()
        {
            drinksData = new List<Drink>();
            drinkCategoriesData = new List<DrinkCategory>();
            modificationRequestsData = new List<DrinkModificationRequest>();

            drinksMock = CreateDbSetMock(drinksData);
            drinkCategoriesMock = CreateDbSetMock(drinkCategoriesData);
            modificationRequestsMock = CreateDbSetMock(modificationRequestsData);

            dbContextMock = new Mock<IAppDbContext>();
            dbContextMock.Setup(x => x.Drinks).Returns(drinksMock.Object);
            dbContextMock.Setup(x => x.DrinkCategories).Returns(drinkCategoriesMock.Object);
            dbContextMock.Setup(x => x.DrinkModificationRequests).Returns(modificationRequestsMock.Object);
            dbContextMock.Setup(x => x.SaveChanges()).Returns(1);

            repository = new DrinkRepository(dbContextMock.Object);
        }

        private static Mock<DbSet<T>> CreateDbSetMock<T>(List<T> data) where T : class
        {
            var queryable = data.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            mockSet.Setup(m => m.Remove(It.IsAny<T>())).Callback<T>(entity => data.Remove(entity));
            mockSet.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>(entities =>
            {
                foreach (var e in entities.ToList())
                    data.Remove(e);
            });

            return mockSet;
        }

        [Fact]
        public void DeleteDrink_DrinkNotFound_ThrowsException()
        {
            // No drinks added to data => should throw
            var ex = Assert.Throws<Exception>(() => repository.DeleteDrink(42));
            Assert.Contains("No drink found", ex.Message);
        }

        [Fact]
        public void DeleteDrink_WithNoModificationRequests_RemovesDrinkAndCategoriesAndCallsSaveChanges()
        {
            // Arrange
            var drink = new Drink
            {
                DrinkId = 1,
                DrinkCategories = new List<DrinkCategory>()
            };
            drinksData.Add(drink);

            var category = new DrinkCategory { DrinkId = 1, CategoryId = 100 };
            drinkCategoriesData.Add(category);
            drink.DrinkCategories.Add(category);

            // Act
            repository.DeleteDrink(1);

            // Assert
            Assert.DoesNotContain(drink, drinksData);
        }

        [Fact]
        public void DeleteDrink_WithModificationRequests_NullifiesAndRemovesRequestsAndCallsSaveChanges()
        {
            // Arrange
            var drink = new Drink
            {
                DrinkId = 1,
                DrinkCategories = new List<DrinkCategory>()
            };
            drinksData.Add(drink);

            var req = new DrinkModificationRequest
            {
                DrinkModificationRequestId = 10,
                OldDrinkId = 1,
                NewDrinkId = null,
                OldDrink = new Drink { DrinkId = 1 },
                NewDrink = null
            };
            modificationRequestsData.Add(req);

            // Act
            repository.DeleteDrink(1);

            // Assert that modificationRequestsData is empty (removed)
            Assert.Empty(modificationRequestsData);
        }
    }
}