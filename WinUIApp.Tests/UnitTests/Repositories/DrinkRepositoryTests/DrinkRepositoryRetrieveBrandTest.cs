using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;

namespace WinUIApp.Tests.UnitTests.Repositories.DrinkRepositoryTests
{
    public class DrinkRepositoryRetrieveBrandTest
    {
        private const string EXISTING_BRAND_NAME = "Existing Brand";
        private const string NEW_BRAND_NAME = "New Brand";

        private Mock<IAppDbContext> mockDbContext;
        private Mock<DbSet<Brand>> mockBrandDbSet;
        private List<Brand> brands;
        private DrinkRepository drinkRepository;

        public DrinkRepositoryRetrieveBrandTest()
        {
            this.brands = new List<Brand>
            {
                new Brand { BrandId = 1, BrandName = EXISTING_BRAND_NAME }
            };

            this.mockBrandDbSet = this.brands.AsQueryable().BuildMockDbSet();

            this.mockDbContext = new Mock<IAppDbContext>();
            this.mockDbContext
                .Setup(context => context.Brands)
                .Returns(this.mockBrandDbSet.Object);

            this.mockDbContext
                .Setup(context => context.SaveChanges())
                .Callback(() =>
                {
                    Brand addedBrand = this.mockBrandDbSet.Invocations
                        .Where(invocation => invocation.Method.Name == nameof(DbSet<Brand>.Add))
                        .Select(invocation => invocation.Arguments[0])
                        .Cast<Brand>()
                        .First();

                    this.brands.Add(addedBrand);
                    this.mockBrandDbSet = this.brands.AsQueryable().BuildMockDbSet();

                    this.mockDbContext
                        .Setup(context => context.Brands)
                        .Returns(this.mockBrandDbSet.Object);
                });

            this.drinkRepository = new DrinkRepository(this.mockDbContext.Object);
        }

        [Fact]
        public void RetrieveBrand_WhenBrandExists_ReturnsExistingBrand()
        {
            // Act
            Brand result = this.drinkRepository.RetrieveBrand(EXISTING_BRAND_NAME);

            // Assert
            Assert.Equal(EXISTING_BRAND_NAME, result.BrandName);
        }

        [Fact]
        public void RetrieveBrand_WhenBrandDoesNotExist_AddsBrandToDatabase()
        {
            // Act
            this.drinkRepository.RetrieveBrand(NEW_BRAND_NAME);

            // Assert
            bool newBrandExists = this.brands.Any(brand => brand.BrandName == NEW_BRAND_NAME);
            Assert.True(newBrandExists);
        }

        [Fact]
        public void RetrieveBrand_WhenBrandIsAdded_ReturnsNewBrand()
        {
            // Act
            Brand result = this.drinkRepository.RetrieveBrand(NEW_BRAND_NAME);

            // Assert
            Assert.Equal(NEW_BRAND_NAME, result.BrandName);
        }
    }
}
