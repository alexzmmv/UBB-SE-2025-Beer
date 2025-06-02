using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using MockQueryable.Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;
using WinUIApp.WebAPI.Models;
using DataAccess.Repository;

namespace WinUIApp.Tests.UnitTests.Repositories.DrinkRepositoryTests
{
    public class DrinkRepositoryGetDrinkByIdTest
    {
        private readonly Mock<IAppDbContext> mockDbContext;
        private readonly Mock<DbSet<Drink>> mockDrinkDbSet;
        private readonly DrinkRepository drinkRepository;
        private readonly List<Drink> drinkData;
        private readonly int EXISTING_DRINK_ID = 1;
        private readonly int NON_EXISTING_DRINK_ID = 999;
        private readonly string DRINK_NAME = "Sample Drink";
        private readonly string BRAND_NAME = "Sample Brand";
        private readonly string CATEGORY_NAME = "Sample Category";
        private readonly string DRINK_IMAGE_URL = "https://example.com/sample.jpg";
        private readonly decimal ALCOHOL_CONTENT = 5.5m;

        public DrinkRepositoryGetDrinkByIdTest()
        {
            this.mockDbContext = new Mock<IAppDbContext>();

            Brand sampleBrand = new Brand { BrandId = 1, BrandName = this.BRAND_NAME };
            Category sampleCategory = new Category(1, this.CATEGORY_NAME);

            this.drinkData = new List<Drink>
            {
                new Drink
                {
                    DrinkId = this.EXISTING_DRINK_ID,
                    DrinkName = this.DRINK_NAME,
                    DrinkURL = this.DRINK_IMAGE_URL,
                    AlcoholContent = this.ALCOHOL_CONTENT,
                    IsRequestingApproval = false,
                    Brand = sampleBrand,
                    DrinkCategories = new List<DrinkCategory>
                    {
                        new DrinkCategory
                        {
                            Category = sampleCategory
                        }
                    }
                }
            };

            this.mockDrinkDbSet = this.drinkData.AsQueryable().BuildMockDbSet();

            this.mockDbContext
                .Setup(context => context.Drinks)
                .Returns(this.mockDrinkDbSet.Object);

            this.drinkRepository = new DrinkRepository(this.mockDbContext.Object);
        }

        [Fact]
        public void GetDrinkById_WhenDrinkExists_ReturnsNotNull()
        {
            // Act
            DrinkDTO? result = this.drinkRepository.GetDrinkById(this.EXISTING_DRINK_ID);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetDrinkById_WhenDrinkExists_ReturnsCorrectDrinkId()
        {
            // Act
            DrinkDTO? result = this.drinkRepository.GetDrinkById(this.EXISTING_DRINK_ID);

            // Assert
            Assert.Equal(this.EXISTING_DRINK_ID, result!.DrinkId);
        }

        [Fact]
        public void GetDrinkById_WhenDrinkExists_ReturnsCorrectBrandName()
        {
            // Act
            DrinkDTO? result = this.drinkRepository.GetDrinkById(this.EXISTING_DRINK_ID);

            // Assert
            Assert.Equal(this.BRAND_NAME, result!.DrinkBrand.BrandName);
        }

        [Fact]
        public void GetDrinkById_WhenDrinkExists_ReturnsCorrectDrinkName()
        {
            // Act
            DrinkDTO? result = this.drinkRepository.GetDrinkById(this.EXISTING_DRINK_ID);

            // Assert
            Assert.Equal(this.DRINK_NAME, result!.DrinkName);
        }

        [Fact]
        public void GetDrinkById_WhenDrinkExists_ReturnsCorrectCategory()
        {
            // Act
            DrinkDTO? result = this.drinkRepository.GetDrinkById(this.EXISTING_DRINK_ID);

            // Assert
            string actualCategoryName = result!.CategoryList.First().CategoryName;
            Assert.Equal(this.CATEGORY_NAME, actualCategoryName);
        }

        [Fact]
        public void GetDrinkById_WhenDrinkDoesNotExist_ReturnsNull()
        {
            // Act
            DrinkDTO? result = this.drinkRepository.GetDrinkById(this.NON_EXISTING_DRINK_ID);

            // Assert
            Assert.Null(result);
        }
    }
}
