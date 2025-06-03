using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;

namespace WinUIApp.Tests.UnitTests.Repositories.DrinkRepositoryTests
{
    public class DrinkRepositoryIsBrandInDatabaseTest
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;
        private readonly Mock<DbSet<Brand>> mockBrandDbSet;
        private readonly DrinkRepository drinkRepository;
        private readonly List<Brand> brandData;

        public DrinkRepositoryIsBrandInDatabaseTest()
        {
            this.mockAppDbContext = new Mock<IAppDbContext>();

            this.brandData = new List<Brand>
            {
                new Brand { BrandId = 1, BrandName = "BrandX" },
                new Brand { BrandId = 2, BrandName = "BrandY" }
            };

            this.mockBrandDbSet = this.brandData.AsQueryable().BuildMockDbSet();

            this.mockAppDbContext
                .Setup(context => context.Brands)
                .Returns(this.mockBrandDbSet.Object);

            this.drinkRepository = new DrinkRepository(this.mockAppDbContext.Object);
        }

        [Fact]
        public void IsBrandInDatabase_WhenBrandExists_ReturnsTrue()
        {
            // Act
            bool exists = this.drinkRepository.IsBrandInDatabase("BrandX");

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public void IsBrandInDatabase_WhenBrandDoesNotExist_ReturnsFalse()
        {
            // Act
            bool exists = this.drinkRepository.IsBrandInDatabase("NonExistentBrand");

            // Assert
            Assert.False(exists);
        }
    }
}