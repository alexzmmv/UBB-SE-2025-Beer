using DataAccess.IRepository;
using Moq;
using WinUiApp.Data.Data;
using WinUIApp.WebAPI.Models;
using WinUIApp.WebAPI.Services;

namespace WinUIApp.Tests.UnitTests.Services.DrinkServiceTests
{
    public class DrinkServiceAddDrinkTest
    {
        private readonly Mock<IDrinkRepository> mockDrinkRepository;
        private readonly DrinkService drinkService;

        private readonly string testDrinkName = "TestDrink";
        private readonly string testDrinkPath = "test/path.png";
        private readonly List<Category> testCategories = new List<Category> { new Category(1, "Category1") };
        private readonly string testBrandName = "TestBrand";
        private readonly float testAlcoholPercentage = 5.0f;
        private readonly Guid testUserId = Guid.NewGuid();

        private readonly DrinkDTO expectedDrinkDto = new DrinkDTO
        {
            DrinkId = 1,
            DrinkName = "TestDrink",
            // add other DrinkDTO properties if needed
        };

        public DrinkServiceAddDrinkTest()
        {
            this.mockDrinkRepository = new Mock<IDrinkRepository>();
            this.drinkService = new DrinkService(this.mockDrinkRepository.Object);

            this.mockDrinkRepository
                .Setup(repo => repo.AddDrink(
                    testDrinkName,
                    testDrinkPath,
                    testCategories,
                    testBrandName,
                    testAlcoholPercentage,
                    It.IsAny<bool>()))
                .Returns(expectedDrinkDto);
        }

        [Fact]
        public void AddDrink_WhenCalled_ReturnsExpectedDrinkDTO()
        {
            // Act
            DrinkDTO result = this.drinkService.AddDrink(
                testDrinkName,
                testDrinkPath,
                testCategories,
                testBrandName,
                testAlcoholPercentage,
                testUserId);

            // Assert
            Assert.Equal(expectedDrinkDto, result);
            this.mockDrinkRepository.Verify(repo => repo.AddDrink(
                testDrinkName,
                testDrinkPath,
                testCategories,
                testBrandName,
                testAlcoholPercentage,
                It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public void AddDrink_WhenRepositoryThrows_ThrowsWrappedException()
        {
            // Arrange
            this.mockDrinkRepository
                .Setup(repo => repo.AddDrink(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<List<Category>>(),
                    It.IsAny<string>(),
                    It.IsAny<float>(),
                    It.IsAny<bool>()))
                .Throws(new Exception("Repository add failed"));

            // Act & Assert
            Exception ex = Assert.Throws<Exception>(() => this.drinkService.AddDrink(
                testDrinkName,
                testDrinkPath,
                testCategories,
                testBrandName,
                testAlcoholPercentage,
                testUserId));

            Assert.Contains("Error happened while adding a drink", ex.Message);
            Assert.Contains("Repository add failed", ex.InnerException?.Message);
        }
    }
}
