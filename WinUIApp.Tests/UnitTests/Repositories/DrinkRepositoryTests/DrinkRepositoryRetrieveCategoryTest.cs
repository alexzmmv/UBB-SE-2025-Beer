using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;

namespace WinUIApp.Tests.UnitTests.Repositories.DrinkRepositoryTests
{
    public class DrinkRepositoryRetrieveCategoryTest
    {
        private const int EXISTING_CATEGORY_ID = 1;
        private const string EXISTING_CATEGORY_NAME = "Beer";
        private const string SECONDARY_MATCH_NAME = "Cider";
        private const string NEW_CATEGORY_NAME = "Whiskey";

        private List<Category> categories;
        private Mock<DbSet<Category>> mockCategoryDbSet;
        private Mock<IAppDbContext> mockDbContext;
        private DrinkRepository drinkRepository;

        public DrinkRepositoryRetrieveCategoryTest()
        {
            this.categories = new List<Category>
            {
                new Category { CategoryId = EXISTING_CATEGORY_ID, CategoryName = EXISTING_CATEGORY_NAME },
                new Category { CategoryId = 2, CategoryName = SECONDARY_MATCH_NAME }
            };

            this.mockCategoryDbSet = this.categories.AsQueryable().BuildMockDbSet();

            this.mockDbContext = new Mock<IAppDbContext>();
            this.mockDbContext
                .Setup(context => context.Categories)
                .Returns(this.mockCategoryDbSet.Object);

            this.mockDbContext
                .Setup(context => context.SaveChanges())
                .Callback(() =>
                {
                    Category addedCategory = this.mockCategoryDbSet.Invocations
                        .Where(invocation => invocation.Method.Name == nameof(DbSet<Category>.Add))
                        .Select(invocation => invocation.Arguments[0])
                        .Cast<Category>()
                        .First();

                    this.categories.Add(addedCategory);
                    this.mockCategoryDbSet = this.categories.AsQueryable().BuildMockDbSet();

                    this.mockDbContext
                        .Setup(context => context.Categories)
                        .Returns(this.mockCategoryDbSet.Object);
                });

            this.drinkRepository = new DrinkRepository(this.mockDbContext.Object);
        }

        [Fact]
        public void RetrieveCategory_WhenCategoryExistsById_ReturnsMatchingCategory()
        {
            // Arrange
            Category inputCategory = new Category
            {
                CategoryId = EXISTING_CATEGORY_ID,
                CategoryName = "Unused Name"
            };

            // Act
            Category result = this.drinkRepository.RetrieveCategory(inputCategory);

            // Assert
            Assert.Equal(EXISTING_CATEGORY_NAME, result.CategoryName);
        }

        [Fact]
        public void RetrieveCategory_WhenCategoryExistsByName_ReturnsMatchingCategory()
        {
            // Arrange
            Category inputCategory = new Category
            {
                CategoryId = 999,
                CategoryName = SECONDARY_MATCH_NAME
            };

            // Act
            Category result = this.drinkRepository.RetrieveCategory(inputCategory);

            // Assert
            Assert.Equal(SECONDARY_MATCH_NAME, result.CategoryName);
        }

        [Fact]
        public void RetrieveCategory_WhenCategoryDoesNotExist_AddsCategoryToDatabase()
        {
            // Arrange
            Category inputCategory = new Category
            {
                CategoryId = 999,
                CategoryName = NEW_CATEGORY_NAME
            };

            // Act
            this.drinkRepository.RetrieveCategory(inputCategory);

            // Assert
            bool newCategoryExists = this.categories.Any(category => category.CategoryName == NEW_CATEGORY_NAME);
            Assert.True(newCategoryExists);
        }

        [Fact]
        public void RetrieveCategory_WhenCategoryIsCreated_ReturnsNewCategory()
        {
            // Arrange
            Category inputCategory = new Category
            {
                CategoryId = 999,
                CategoryName = NEW_CATEGORY_NAME
            };

            // Act
            Category result = this.drinkRepository.RetrieveCategory(inputCategory);

            // Assert
            Assert.Equal(NEW_CATEGORY_NAME, result.CategoryName);
        }
    }
}
