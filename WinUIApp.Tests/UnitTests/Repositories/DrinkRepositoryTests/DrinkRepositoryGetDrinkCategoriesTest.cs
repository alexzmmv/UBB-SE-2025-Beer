using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;

namespace WinUIApp.Tests.UnitTests.Repositories.DrinkRepositoryTests
{
    public class DrinkRepositoryGetDrinkCategoriesTest
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;
        private readonly Mock<DbSet<Category>> mockCategoryDbSet;
        private readonly DrinkRepository drinkRepository;
        private readonly List<Category> categoryData;

        public DrinkRepositoryGetDrinkCategoriesTest()
        {
            this.mockAppDbContext = new Mock<IAppDbContext>();

            this.categoryData = new List<Category>
            {
                new Category(2, "Category B"),
                new Category(1, "Category A"),
                new Category(3, "Category C")
            };

            this.mockCategoryDbSet = this.categoryData.AsQueryable().BuildMockDbSet();

            this.mockAppDbContext.Setup(context => context.Categories).Returns(this.mockCategoryDbSet.Object);

            this.drinkRepository = new DrinkRepository(this.mockAppDbContext.Object);
        }

        [Fact]
        public void GetDrinkCategories_WhenCalled_ReturnsCategoriesOrderedByCategoryId()
        {
            // Act
            List<Category> categories = this.drinkRepository.GetDrinkCategories();

            // Assert
            Assert.Equal(3, categories.Count);
            Assert.Equal(1, categories[0].CategoryId);
            Assert.Equal("Category A", categories[0].CategoryName);
            Assert.Equal(2, categories[1].CategoryId);
            Assert.Equal(3, categories[2].CategoryId);
        }
    }
}