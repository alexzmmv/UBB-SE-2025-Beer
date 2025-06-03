using DataAccess.IRepository;
using Moq;
using WinUiApp.Data.Data;
using WinUIApp.WebAPI.Models;
using WinUIApp.WebAPI.Services;

namespace WinUIApp.Tests.UnitTests.Services.DrinkServiceTests
{
    public class DrinkServiceGetDrinksTest
    {
        private readonly Mock<IDrinkRepository> mockDrinkRepository;
        private readonly DrinkService drinkService;

        private readonly List<DrinkDTO> allDrinks;

        public DrinkServiceGetDrinksTest()
        {
            this.mockDrinkRepository = new Mock<IDrinkRepository>();
            this.drinkService = new DrinkService(this.mockDrinkRepository.Object);

            this.allDrinks = new List<DrinkDTO>
            {
                new DrinkDTO
                {
                    DrinkId = 1,
                    DrinkName = "Alpha",
                    AlcoholContent = 5.0f,
                    DrinkBrand = new Brand { BrandName = "BrandA" },
                    CategoryList = new List<Category> { new Category(1, "Cat1"), new Category(2, "Cat2") }
                },
                new DrinkDTO
                {
                    DrinkId = 2,
                    DrinkName = "Beta",
                    AlcoholContent = 7.5f,
                    DrinkBrand = new Brand { BrandName = "BrandB" },
                    CategoryList = new List<Category> { new Category(2, "Cat2") }
                },
                new DrinkDTO
                {
                    DrinkId = 3,
                    DrinkName = "Gamma",
                    AlcoholContent = 4.0f,
                    DrinkBrand = new Brand { BrandName = "BrandA" },
                    CategoryList = new List<Category> { new Category(1, "Cat1") }
                }
            };

            this.mockDrinkRepository
                .Setup(repo => repo.GetDrinks())
                .Returns(allDrinks);
        }

        [Fact]
        public void GetDrinks_WhenCalledWithoutFilters_ReturnsAllDrinks()
        {
            // Act
            var result = this.drinkService.GetDrinks(null, null, null, null, null, null);

            // Assert
            Assert.Equal(allDrinks.Count, result.Count);
        }

        [Fact]
        public void GetDrinks_WithMinimumAlcoholFilter_ReturnsFilteredDrinks()
        {
            // Act
            var result = this.drinkService.GetDrinks(null, null, null, 5.0f, null, null);

            // Assert
            Assert.All(result, d => Assert.True(d.AlcoholContent >= 5.0f));
        }

        [Fact]
        public void GetDrinks_WithMaximumAlcoholFilter_ReturnsFilteredDrinks()
        {
            // Act
            var result = this.drinkService.GetDrinks(null, null, null, null, 5.0f, null);

            // Assert
            Assert.All(result, d => Assert.True(d.AlcoholContent <= 5.0f));
        }

        [Fact]
        public void GetDrinks_WithSearchKeyword_ReturnsMatchingDrinks()
        {
            // Act
            var result = this.drinkService.GetDrinks("alpha", null, null, null, null, null);

            // Assert
            Assert.All(result, d => Assert.Contains("alpha", d.DrinkName, StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void GetDrinks_WithBrandFilter_ReturnsFilteredDrinks()
        {
            // Act
            var result = this.drinkService.GetDrinks(null, new List<string> { "BrandA" }, null, null, null, null);

            // Assert
            Assert.All(result, d => Assert.Equal("BrandA", d.DrinkBrand.BrandName));
        }

        [Fact]
        public void GetDrinks_WithCategoryFilter_ReturnsDrinksMatchingAllCategories()
        {
            // Act
            var categoryFilter = new List<string> { "Cat1", "Cat2" };
            var result = this.drinkService.GetDrinks(null, null, categoryFilter, null, null, null);

            // Assert
            Assert.All(result, d =>
                Assert.True(categoryFilter.All(cat => d.CategoryList.Any(c => c.CategoryName == cat))));
        }

        [Fact]
        public void GetDrinks_WithOrderingByDrinkNameAscending_ReturnsOrderedDrinks()
        {
            // Arrange
            var orderingCriteria = new Dictionary<string, bool> { { "DrinkName", true } };

            // Act
            var result = this.drinkService.GetDrinks(null, null, null, null, null, orderingCriteria);

            // Assert
            var expectedOrder = allDrinks.OrderBy(d => d.DrinkName).Select(d => d.DrinkId);
            Assert.Equal(expectedOrder, result.Select(d => d.DrinkId));
        }

        [Fact]
        public void GetDrinks_WithOrderingByDrinkNameDescending_ReturnsOrderedDrinks()
        {
            // Arrange
            var orderingCriteria = new Dictionary<string, bool> { { "DrinkName", false } };

            // Act
            var result = this.drinkService.GetDrinks(null, null, null, null, null, orderingCriteria);

            // Assert
            var expectedOrder = allDrinks.OrderByDescending(d => d.DrinkName).Select(d => d.DrinkId);
            Assert.Equal(expectedOrder, result.Select(d => d.DrinkId));
        }

        [Fact]
        public void GetDrinks_WithOrderingByAlcoholContentDescending_ReturnsOrderedDrinks()
        {
            // Arrange
            var orderingCriteria = new Dictionary<string, bool> { { "AlcoholContent", false } };

            // Act
            var result = this.drinkService.GetDrinks(null, null, null, null, null, orderingCriteria);

            // Assert
            var expectedOrder = allDrinks.OrderByDescending(d => d.AlcoholContent).Select(d => d.DrinkId);
            Assert.Equal(expectedOrder, result.Select(d => d.DrinkId));
        }

        [Fact]
        public void GetDrinks_WithOrderingByAlcoholContentAscending_ReturnsOrderedDrinks()
        {
            // Arrange
            var orderingCriteria = new Dictionary<string, bool> { { "AlcoholContent", true } };

            // Act
            var result = this.drinkService.GetDrinks(null, null, null, null, null, orderingCriteria);

            // Assert
            var expectedOrder = allDrinks.OrderBy(d => d.AlcoholContent).Select(d => d.DrinkId);
            Assert.Equal(expectedOrder, result.Select(d => d.DrinkId));
        }

        [Fact]
        public void GetDrinks_WhenRepositoryThrows_ThrowsWrappedException()
        {
            // Arrange
            this.mockDrinkRepository
                .Setup(repo => repo.GetDrinks())
                .Throws(new Exception("Repository failure"));

            // Act & Assert
            Exception ex = Assert.Throws<Exception>(() => this.drinkService.GetDrinks(null, null, null, null, null, null));
            Assert.Contains("Error happened while getting drinks:", ex.Message);
            Assert.Contains("Repository failure", ex.InnerException?.Message);
        }
    }
}