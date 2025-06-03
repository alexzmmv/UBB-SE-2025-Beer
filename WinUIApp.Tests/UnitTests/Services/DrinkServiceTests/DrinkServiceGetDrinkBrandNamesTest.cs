using DataAccess.IRepository;
using Moq;
using WinUiApp.Data.Data;
using WinUIApp.WebAPI.Services;

namespace WinUIApp.Tests.UnitTests.Services.DrinkServiceTests
{
    public class DrinkServiceGetDrinkBrandNamesTest
    {
        private readonly Mock<IDrinkRepository> mockDrinkRepository;
        private readonly DrinkService drinkService;

        public DrinkServiceGetDrinkBrandNamesTest()
        {
            this.mockDrinkRepository = new Mock<IDrinkRepository>();
            this.drinkService = new DrinkService(this.mockDrinkRepository.Object);

            List<Brand> mockBrands = new List<Brand>
            {
                new Brand { BrandId = 1, BrandName = "Coca-Cola" },
                new Brand { BrandId = 2, BrandName = "Pepsi" }
            };

            this.mockDrinkRepository
                .Setup(repo => repo.GetDrinkBrands())
                .Returns(mockBrands);
        }

        [Fact]
        public void GetDrinkBrandNames_WhenCalled_ReturnsBrands()
        {
            // Act
            List<Brand> brands = this.drinkService.GetDrinkBrandNames();

            // Assert
            Assert.NotEmpty(brands);
            Assert.Equal(2, brands.Count);
            this.mockDrinkRepository.Verify(repo => repo.GetDrinkBrands(), Times.Once);
        }

        [Fact]
        public void GetDrinkBrandNames_WhenRepositoryThrows_ThrowsWrappedException()
        {
            // Arrange
            this.mockDrinkRepository
                .Setup(repo => repo.GetDrinkBrands())
                .Throws(new Exception("Repository failure"));

            // Act & Assert
            Exception exception = Assert.Throws<Exception>(() => this.drinkService.GetDrinkBrandNames());
            Assert.Contains("Error happened while getting drink brands", exception.Message);
            Assert.Contains("Repository failure", exception.InnerException?.Message);
        }
    }
}
