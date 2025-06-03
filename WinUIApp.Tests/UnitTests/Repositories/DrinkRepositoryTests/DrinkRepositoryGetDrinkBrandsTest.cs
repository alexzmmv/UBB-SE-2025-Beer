using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;

namespace WinUIApp.Tests.UnitTests.Repositories.DrinkRepositoryTests
{
    public class DrinkRepositoryGetDrinkBrandsTest
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;
        private readonly Mock<DbSet<Brand>> mockBrandDbSet;
        private readonly DrinkRepository drinkRepository;
        private readonly List<Brand> brandData;

        public DrinkRepositoryGetDrinkBrandsTest()
        {
            this.mockAppDbContext = new Mock<IAppDbContext>();

            this.brandData = new List<Brand>
            {
                new Brand { BrandId = 1, BrandName = "Brand A" },
                new Brand { BrandId = 2, BrandName = "Brand B" },
                new Brand { BrandId = 3, BrandName = "Brand C" }
            };

            this.mockBrandDbSet = this.brandData.AsQueryable().BuildMockDbSet();

            this.mockAppDbContext.Setup(context => context.Brands).Returns(this.mockBrandDbSet.Object);

            this.drinkRepository = new DrinkRepository(this.mockAppDbContext.Object);
        }

        [Fact]
        public void GetDrinkBrands_WhenCalled_ReturnsAllBrands()
        {
            // Act
            List<Brand> brands = this.drinkRepository.GetDrinkBrands();

            // Assert
            Assert.Equal(3, brands.Count);
            Assert.Contains(brands, b => b.BrandName == "Brand A");
            Assert.Contains(brands, b => b.BrandName == "Brand B");
            Assert.Contains(brands, b => b.BrandName == "Brand C");
        }
    }
}