using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;

namespace WinUIApp.Tests.UnitTests.Repositories.DrinkRepositoryTests
{
    public class DrinkRepositoryAddBrandTest
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;
        private readonly Mock<DbSet<Brand>> mockBrandDbSet;
        private readonly DrinkRepository drinkRepository;
        private readonly List<Brand> brandData;

        public DrinkRepositoryAddBrandTest()
        {
            this.mockAppDbContext = new Mock<IAppDbContext>();

            this.brandData = new List<Brand>();

            this.mockBrandDbSet = this.brandData.AsQueryable().BuildMockDbSet();

            this.mockAppDbContext
                .Setup(context => context.Brands)
                .Returns(this.mockBrandDbSet.Object);

            this.drinkRepository = new DrinkRepository(this.mockAppDbContext.Object);
        }

        [Fact]
        public void AddBrand_WhenCalled_AddsBrandToDbSetAndSavesChanges()
        {
            // Arrange
            Brand? addedBrand = null!;
            string brandName = "NewBrand";

            this.mockBrandDbSet
                .Setup(set => set.Add(It.IsAny<Brand>()))
                .Callback<Brand>(brand => addedBrand = brand);

            this.mockAppDbContext
                .Setup(c => c.SaveChanges())
                .Returns(1);

            // Act
            this.drinkRepository.AddBrand(brandName);

            // Assert
            Assert.NotNull(addedBrand);
            Assert.Equal(brandName, addedBrand.BrandName);
        }
    }
}
