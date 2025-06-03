using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;
using WinUIApp.WebAPI.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace WinUIApp.Tests.UnitTests.Repositories.DrinkRepositoryTests
{
    public class DrinkRepositoryUpdateDrinkTest
    {
        private readonly Mock<IAppDbContext> dbContext;
        private readonly List<Drink> drinks;
        private readonly List<Brand> brands;
        private readonly List<Category> categories;
        private readonly List<DrinkCategory> drinkCategories;
        private readonly DrinkRepository repository;

        public DrinkRepositoryUpdateDrinkTest()
        {
            drinks = new List<Drink>
            {
                new Drink
                {
                    DrinkId = 1,
                    DrinkName = "Original",
                    DrinkURL = "url",
                    AlcoholContent = 10,
                    BrandId = 1,
                    IsRequestingApproval = false,
                    DrinkCategories = new List<DrinkCategory>()
                }
            };

            brands = new List<Brand>
            {
                new Brand { BrandId = 1, BrandName = "KnownBrand" }
            };

            categories = new List<Category>
            {
                new Category { CategoryId = 1, CategoryName = "Vodka" }
            };

            drinkCategories = new List<DrinkCategory>
            {
                new DrinkCategory { DrinkId = 1, CategoryId = 1 }
            };

            dbContext = new Mock<IAppDbContext>();
            dbContext.Setup(x => x.Drinks).Returns(CreateMockDbSet(drinks).Object);
            dbContext.Setup(x => x.Brands).Returns(CreateMockDbSet(brands).Object);
            dbContext.Setup(x => x.Categories).Returns(CreateMockDbSet(categories).Object);
            dbContext.Setup(x => x.DrinkCategories).Returns(CreateMockDbSet(drinkCategories).Object);
            dbContext.Setup(x => x.SaveChanges()).Returns(1);

            repository = new DrinkRepository(dbContext.Object);
        }

        private Mock<DbSet<T>> CreateMockDbSet<T>(List<T> data) where T : class
        {
            var queryable = data.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            mockSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(data.Add);
            mockSet.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<T>>()))
                   .Callback<IEnumerable<T>>(entities => data.RemoveAll(entities.Contains));
            return mockSet;
        }

        [Fact]
        public void UpdateDrink_BrandDoesNotExist_AddsNewBrand()
        {
            var dto = CreateDtoWithBrand("NewBrand");
            repository.UpdateDrink(dto);
            Assert.Contains(brands, b => b.BrandName == "NewBrand");
        }

        [Fact]
        public void UpdateDrink_BrandExists_UsesExistingBrand()
        {
            var dto = CreateDtoWithBrand("KnownBrand");
            repository.UpdateDrink(dto);
            Assert.Equal(1, drinks.First().BrandId);
        }

        [Fact]
        public void UpdateDrink_DrinkNotFound_ThrowsException()
        {
            var dto = CreateDtoWithBrand("KnownBrand");
            dto.DrinkId = 999;
            var ex = Assert.Throws<Exception>(() => repository.UpdateDrink(dto));
            Assert.Contains("No drink found", ex.Message);
        }

        [Fact]
        public void UpdateDrink_UpdatesDrinkName()
        {
            var dto = CreateDtoWithBrand("KnownBrand");
            dto.DrinkName = "UpdatedName";
            repository.UpdateDrink(dto);
            Assert.Equal("UpdatedName", drinks.First().DrinkName);
        }

        [Fact]
        public void UpdateDrink_RemovesOldCategories()
        {
            var dto = CreateDtoWithBrand("KnownBrand");
            dto.CategoryList = new List<Category> { new Category { CategoryId = 1 } };
            repository.UpdateDrink(dto);
            Assert.DoesNotContain(drinkCategories, dc => dc.CategoryId != 1);
        }

        [Fact]
        public void UpdateDrink_AddsNewDrinkCategories()
        {
            var dto = CreateDtoWithBrand("KnownBrand");
            dto.CategoryList = new List<Category> { new Category { CategoryId = 1 } };
            repository.UpdateDrink(dto);
            Assert.Contains(drinkCategories, dc => dc.CategoryId == 1 && dc.DrinkId == 1);
        }

        [Fact]
        public void UpdateDrink_SaveChangesThrows_ExceptionIsWrapped()
        {
            dbContext.Setup(x => x.SaveChanges()).Throws(new InvalidOperationException("fail"));

            var dto = CreateDtoWithBrand("KnownBrand");

            var ex = Assert.Throws<Exception>(() => repository.UpdateDrink(dto));
            Assert.Contains("fail", ex.InnerException?.Message);
        }

        private DrinkDTO CreateDtoWithBrand(string brandName)
        {
            return new DrinkDTO
            {
                DrinkId = 1,
                DrinkName = "NewName",
                DrinkImageUrl = "img",
                AlcoholContent = 20,
                IsRequestingApproval = true,
                DrinkBrand = new Brand { BrandName = brandName },
                CategoryList = new List<Category> { new Category { CategoryId = 1 } }
            };
        }
    }
}
