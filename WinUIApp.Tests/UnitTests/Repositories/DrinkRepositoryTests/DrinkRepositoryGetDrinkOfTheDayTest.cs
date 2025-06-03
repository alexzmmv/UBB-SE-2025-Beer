using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Moq.Protected;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;
using WinUIApp.WebAPI.Models;

namespace WinUIApp.Tests.UnitTests.Repositories.DrinkRepositoryTests
{
    public class DrinkRepositoryGetDrinkOfTheDayTest
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;
        private Mock<DbSet<DrinkOfTheDay>> mockDrinkOfTheDayDbSet;
        private readonly DrinkRepository drinkRepository;
        private readonly List<DrinkOfTheDay> drinkOfTheDayData;

        private const int ValidDrinkId = 123;
        private readonly DateTime TodayDate = DateTime.UtcNow.Date;

        public DrinkRepositoryGetDrinkOfTheDayTest()
        {
            this.mockAppDbContext = new Mock<IAppDbContext>();

            // Setup test data for DrinkOfTheDay
            this.drinkOfTheDayData = new List<DrinkOfTheDay>
            {
                new DrinkOfTheDay { DrinkId = ValidDrinkId, DrinkTime = TodayDate }
            };

            this.mockDrinkOfTheDayDbSet = this.drinkOfTheDayData.AsQueryable().BuildMockDbSet();

            this.mockAppDbContext.Setup(context => context.DrinkOfTheDays).Returns(this.mockDrinkOfTheDayDbSet.Object);

            // We need to mock GetDrinkById, so let's create a subclass with override
            this.drinkRepository = new TestDrinkRepository(this.mockAppDbContext.Object);
        }

        [Fact]
        public void GetDrinkOfTheDay_WhenDrinkOfTheDayExists_ReturnsDrinkDto()
        {
            // Arrange
            DrinkDTO? returnedDrinkDto = new DrinkDTO(ValidDrinkId, "Test Drink", "http://image.url", new List<Category>(), new Brand { BrandId = 1, BrandName = "TestBrand" }, 40.0f, false);
            ((TestDrinkRepository)this.drinkRepository).SetupGetDrinkById(returnedDrinkDto);

            // Act
            DrinkDTO actualDrinkDto = this.drinkRepository.GetDrinkOfTheDay();

            // Assert
            Assert.NotNull(actualDrinkDto);
            Assert.Equal(ValidDrinkId, actualDrinkDto.DrinkId);
        }

        [Fact]
        public void GetDrinkOfTheDay_WhenDrinkOfTheDayIsEmpty_ThrowsException()
        {
            // Arrange
            this.drinkOfTheDayData.Clear();
            this.mockDrinkOfTheDayDbSet = this.drinkOfTheDayData.AsQueryable().BuildMockDbSet();
            this.mockAppDbContext.Setup(context => context.DrinkOfTheDays).Returns(this.mockDrinkOfTheDayDbSet.Object);

            // Act & Assert
            Exception exception = Assert.Throws<Exception>(() => this.drinkRepository.GetDrinkOfTheDay());
            Assert.Equal("DrinkOfTheDay table is empty.", exception.Message);
        }

        [Fact]
        public void GetDrinkOfTheDay_WhenDrinkNotFound_ThrowsException()
        {
            // Arrange
            // No DrinkDTO returned (null)
            ((TestDrinkRepository)this.drinkRepository).SetupGetDrinkById(null);

            // Act & Assert
            Exception exception = Assert.Throws<Exception>(() => this.drinkRepository.GetDrinkOfTheDay());
            Assert.Equal($"Drink with ID {ValidDrinkId} not found.", exception.Message);
        }

        // Test subclass to override GetDrinkById and ResetDrinkOfTheDay
        private class TestDrinkRepository : DrinkRepository
        {
            private DrinkDTO? drinkDtoToReturn;

            public bool ResetDrinkOfTheDayCalled { get; set; }

            public TestDrinkRepository(IAppDbContext dbContext) : base(dbContext)
            {
            }

            public void SetupGetDrinkById(DrinkDTO? drinkDto)
            {
                this.drinkDtoToReturn = drinkDto;
            }

            public override DrinkDTO? GetDrinkById(int drinkId)
            {
                return this.drinkDtoToReturn;
            }

            public override void ResetDrinkOfTheDay()
            {
                this.ResetDrinkOfTheDayCalled = true;
            }
        }
    }
}
