using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;
using WinUIApp.WebAPI.Models;

namespace WinUIApp.Tests.UnitTests.Repositories.DrinkRepositoryTests
{
    public class DrinkRepositoryAddDrinkTest
    {
        private const string VALID_DRINK_NAME = "Gin Tonic";
        private const string VALID_DRINK_URL = "http://example.com/gin.jpg";
        private const string VALID_BRAND_NAME = "DryGin Co.";
        private const string VALID_CATEGORY_NAME = "Cocktail";
        private const float VALID_ALCOHOL_CONTENT = 40.0f;
        private const int EXPECTED_BRAND_ID = 100;
        private const int EXPECTED_CATEGORY_ID = 200;

        private List<Drink> drinks;
        private List<DrinkCategory> drinkCategories;
        private List<Brand> brands;
        private List<Category> categories;

        private Mock<DbSet<Drink>> mockDrinkDbSet;
        private Mock<DbSet<DrinkCategory>> mockDrinkCategoryDbSet;
        private Mock<IAppDbContext> mockDbContext;

        private Mock<DrinkRepository> mockDrinkRepository;

        public DrinkRepositoryAddDrinkTest()
        {
            this.drinks = new List<Drink>();
            this.drinkCategories = new List<DrinkCategory>();
            this.brands = new List<Brand> { new Brand { BrandId = EXPECTED_BRAND_ID, BrandName = VALID_BRAND_NAME } };
            this.categories = new List<Category> { new Category { CategoryId = EXPECTED_CATEGORY_ID, CategoryName = VALID_CATEGORY_NAME } };

            this.mockDrinkDbSet = this.drinks.AsQueryable().BuildMockDbSet();
            this.mockDrinkCategoryDbSet = this.drinkCategories.AsQueryable().BuildMockDbSet();

            this.mockDbContext = new Mock<IAppDbContext>();
            this.mockDbContext.Setup(context => context.Drinks).Returns(this.mockDrinkDbSet.Object);
            this.mockDbContext.Setup(context => context.DrinkCategories).Returns(this.mockDrinkCategoryDbSet.Object);
            this.mockDbContext.Setup(context => context.Brands).Returns(this.brands.AsQueryable().BuildMockDbSet().Object);
            this.mockDbContext.Setup(context => context.Categories).Returns(this.categories.AsQueryable().BuildMockDbSet().Object);

            this.mockDbContext.Setup(context => context.SaveChanges()).Callback(() =>
            {
                Drink drinkToAdd = this.mockDrinkDbSet.Invocations
                    .Where(invocation => invocation.Method.Name == nameof(DbSet<Drink>.Add))
                    .Select(invocation => invocation.Arguments[0])
                    .Cast<Drink>()
                    .FirstOrDefault();

                if (drinkToAdd != null && !this.drinks.Contains(drinkToAdd))
                {
                    drinkToAdd.DrinkId = 999;
                    drinkToAdd.Brand = this.brands.First(b => b.BrandId == EXPECTED_BRAND_ID);
                    drinkToAdd.DrinkCategories = new List<DrinkCategory>(); // Ensure it's not null
                    this.drinks.Add(drinkToAdd);
                    this.mockDrinkDbSet = this.drinks.AsQueryable().BuildMockDbSet();
                    this.mockDbContext.Setup(context => context.Drinks).Returns(this.mockDrinkDbSet.Object);
                }

                DrinkCategory linkToAdd = this.mockDrinkCategoryDbSet.Invocations
                    .Where(invocation => invocation.Method.Name == nameof(DbSet<DrinkCategory>.Add))
                    .Select(invocation => invocation.Arguments[0])
                    .Cast<DrinkCategory>()
                    .FirstOrDefault();

                if (linkToAdd != null && !this.drinkCategories.Contains(linkToAdd))
                {
                    linkToAdd.Category = this.categories.First(c => c.CategoryId == EXPECTED_CATEGORY_ID);
                    linkToAdd.Drink = this.drinks.FirstOrDefault(d => d.DrinkId == linkToAdd.DrinkId) ?? this.drinks.Last(); // Best-effort
                    this.drinks.Last().DrinkCategories.Add(linkToAdd); // Attach to the drink
                    this.drinkCategories.Add(linkToAdd);
                    this.mockDrinkCategoryDbSet = this.drinkCategories.AsQueryable().BuildMockDbSet();
                    this.mockDbContext.Setup(context => context.DrinkCategories).Returns(this.mockDrinkCategoryDbSet.Object);
                }
            });

            this.mockDrinkRepository = new Mock<DrinkRepository>(this.mockDbContext.Object)
            {
                CallBase = true
            };

            this.mockDrinkRepository
                .Setup(repo => repo.RetrieveBrand(VALID_BRAND_NAME))
                .Returns(this.brands.First());

            this.mockDrinkRepository
                .Setup(repo => repo.RetrieveCategory(It.Is<Category>(c => c.CategoryName == VALID_CATEGORY_NAME)))
                .Returns(this.categories.First());
        }

        [Fact]
        public void AddDrink_WhenDrinkIsValid_AddsDrinkToDatabase()
        {
            // Arrange
            var inputCategories = new List<Category>
            {
                new Category { CategoryName = VALID_CATEGORY_NAME }
            };

            // Act
            this.mockDrinkRepository.Object.AddDrink(
                VALID_DRINK_NAME,
                VALID_DRINK_URL,
                inputCategories,
                VALID_BRAND_NAME,
                VALID_ALCOHOL_CONTENT);

            // Assert
            Assert.Contains(this.drinks, d => d.DrinkName == VALID_DRINK_NAME);
        }

        [Fact]
        public void AddDrink_WhenDrinkIsAdded_ReturnsDrinkDtoWithExpectedName()
        {
            // Arrange
            var inputCategories = new List<Category>
            {
                new Category { CategoryName = VALID_CATEGORY_NAME }
            };

            // Act
            var result = this.mockDrinkRepository.Object.AddDrink(
                VALID_DRINK_NAME,
                VALID_DRINK_URL,
                inputCategories,
                VALID_BRAND_NAME,
                VALID_ALCOHOL_CONTENT);

            // Assert
            Assert.Equal(VALID_DRINK_NAME, result.DrinkName);
        }

        [Fact]
        public void AddDrink_WhenDrinkIsAdded_AlsoAddsDrinkCategoryLinks()
        {
            // Arrange
            var inputCategories = new List<Category>
            {
                new Category { CategoryName = VALID_CATEGORY_NAME }
            };

            // Act
            this.mockDrinkRepository.Object.AddDrink(
                VALID_DRINK_NAME,
                VALID_DRINK_URL,
                inputCategories,
                VALID_BRAND_NAME,
                VALID_ALCOHOL_CONTENT);

            // Assert
            Assert.Contains(this.drinkCategories, dc => dc.CategoryId == EXPECTED_CATEGORY_ID);
        }
    }
}
