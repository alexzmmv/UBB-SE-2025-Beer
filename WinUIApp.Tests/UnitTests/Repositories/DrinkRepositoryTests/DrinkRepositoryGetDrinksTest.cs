using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;
using WinUIApp.WebAPI.Models;

namespace WinUIApp.Tests.UnitTests.Repositories.DrinkRepositoryTests
{
    public class DrinkRepositoryGetDrinksTest
    {
        private readonly Mock<IAppDbContext> mockDbContext;
        private Mock<DbSet<Drink>> mockDrinkDbSet;
        private readonly DrinkRepository drinkRepository;
        private readonly List<Drink> drinkData;

        public DrinkRepositoryGetDrinksTest()
        {
            this.mockDbContext = new Mock<IAppDbContext>();

            Brand sampleBrand = new Brand { BrandId = 1, BrandName = "Sample Brand" };
            Category sampleCategory = new Category(1, "Test Category");

            this.drinkData = new List<Drink>
            {
                new Drink
                {
                    DrinkId = 1,
                    DrinkName = "Approved Drink",
                    DrinkURL = "http://example.com/image.jpg",
                    Brand = sampleBrand,
                    IsRequestingApproval = false,
                    AlcoholContent = 5.5m,
                    DrinkCategories = new List<DrinkCategory>
                    {
                        new DrinkCategory { Category = sampleCategory }
                    }
                },
                new Drink
                {
                    DrinkId = 2,
                    DrinkName = "Pending Drink",
                    DrinkURL = "http://example.com/pending.jpg",
                    Brand = sampleBrand,
                    IsRequestingApproval = true,
                    AlcoholContent = 4.2m,
                    DrinkCategories = new List<DrinkCategory>()
                }
            };

            this.mockDrinkDbSet = this.drinkData.AsQueryable().BuildMockDbSet();

            this.mockDbContext
                .Setup(context => context.Drinks)
                .Returns(this.mockDrinkDbSet.Object);

            this.drinkRepository = new DrinkRepository(this.mockDbContext.Object);
        }

        [Fact]
        public void GetDrinks_WhenCalled_ReturnsOnlyApprovedDrinks()
        {
            // Act
            List<DrinkDTO> result = this.drinkRepository.GetDrinks();

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public void GetDrinks_WhenCalled_MapsDrinkIdCorrectly()
        {
            // Act
            List<DrinkDTO> result = this.drinkRepository.GetDrinks();

            // Assert
            int expectedDrinkId = 1;
            Assert.Equal(expectedDrinkId, result[0].DrinkId);
        }

        [Fact]
        public void GetDrinks_WhenCalled_MapsDrinkNameCorrectly()
        {
            // Act
            List<DrinkDTO> result = this.drinkRepository.GetDrinks();

            // Assert
            string expectedDrinkName = "Approved Drink";
            Assert.Equal(expectedDrinkName, result[0].DrinkName);
        }

        [Fact]
        public void GetDrinks_WhenCalled_MapsBrandCorrectly()
        {
            // Act
            List<DrinkDTO> result = this.drinkRepository.GetDrinks();

            // Assert
            string expectedBrandName = "Sample Brand";
            Assert.Equal(expectedBrandName, result[0].DrinkBrand.BrandName);
        }

        [Fact]
        public void GetDrinks_WhenCalled_MapsCategoriesCorrectly()
        {
            // Act
            List<DrinkDTO> result = this.drinkRepository.GetDrinks();

            // Assert
            int expectedCategoryCount = 1;
            Assert.Equal(expectedCategoryCount, result[0].CategoryList.Count);
        }

        [Fact]
        public void GetDrinks_WhenNoApprovedDrinksExist_ReturnsEmptyList()
        {
            // Arrange
            foreach (Drink drink in this.drinkData)
            {
                drink.IsRequestingApproval = true;
            }

            this.mockDrinkDbSet = this.drinkData.AsQueryable().BuildMockDbSet();

            this.mockDbContext
                .Setup(context => context.Drinks)
                .Returns(this.mockDrinkDbSet.Object);

            DrinkRepository repositoryWithNoApprovedDrinks = new DrinkRepository(this.mockDbContext.Object);

            // Act
            List<DrinkDTO> result = repositoryWithNoApprovedDrinks.GetDrinks();

            // Assert
            Assert.Empty(result);
        }
    }
}