using DataAccess.IRepository;
using Moq;
using WinUiApp.Data.Data;
using WinUIApp.WebAPI.Services;

namespace WinUIApp.Tests.UnitTests.Services.DrinkServiceTests
{
    public class DrinkServiceGetDrinkCategoriesTest
    {
        private readonly Mock<IDrinkRepository> mockDrinkRepository;
        private readonly DrinkService drinkService;

        public DrinkServiceGetDrinkCategoriesTest()
        {
            this.mockDrinkRepository = new Mock<IDrinkRepository>();
            this.drinkService = new DrinkService(this.mockDrinkRepository.Object);

            List<Category> mockCategories = new List<Category>
            {
                new Category(1, "Soda"),
                new Category(2, "Juice")
            };

            this.mockDrinkRepository
                .Setup(repo => repo.GetDrinkCategories())
                .Returns(mockCategories);
        }

        [Fact]
        public void GetDrinkCategories_WhenCalled_ReturnsCategories()
        {
            // Act
            List<Category> categories = this.drinkService.GetDrinkCategories();

            // Assert
            Assert.NotEmpty(categories);
            Assert.Equal(2, categories.Count);
            this.mockDrinkRepository.Verify(repo => repo.GetDrinkCategories(), Times.Once);
        }

        [Fact]
        public void GetDrinkCategories_WhenRepositoryThrows_ThrowsWrappedException()
        {
            // Arrange
            this.mockDrinkRepository
                .Setup(repo => repo.GetDrinkCategories())
                .Throws(new Exception("Repository failure"));

            // Act & Assert
            Exception exception = Assert.Throws<Exception>(() => this.drinkService.GetDrinkCategories());
            Assert.Contains("Error happened while getting drink categories", exception.Message);
            Assert.Contains("Repository failure", exception.InnerException?.Message);
        }
    }
}
