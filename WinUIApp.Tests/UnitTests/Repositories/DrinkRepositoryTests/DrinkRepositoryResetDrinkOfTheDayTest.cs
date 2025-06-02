using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;

namespace WinUIApp.Tests.UnitTests.Repositories.DrinkRepositoryTests
{
    public class DrinkRepositoryResetDrinkOfTheDayTest
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;
        private readonly Mock<DbSet<DrinkOfTheDay>> mockDrinkOfTheDayDbSet;
        private readonly DrinkRepository drinkRepository;
        private readonly List<DrinkOfTheDay> drinkOfTheDayData;

        private const int TOP_VOTED_DRINK_ID = 99;

        public DrinkRepositoryResetDrinkOfTheDayTest()
        {
            this.mockAppDbContext = new Mock<IAppDbContext>();

            this.drinkOfTheDayData = new List<DrinkOfTheDay>
            {
                new DrinkOfTheDay { DrinkId = 1, DrinkTime = DateTime.UtcNow.AddDays(-1) },
                new DrinkOfTheDay { DrinkId = 2, DrinkTime = DateTime.UtcNow.AddDays(-2) }
            };

            this.mockDrinkOfTheDayDbSet = this.drinkOfTheDayData.AsQueryable().BuildMockDbSet();

            this.mockAppDbContext.Setup(context => context.DrinkOfTheDays).Returns(this.mockDrinkOfTheDayDbSet.Object);

            this.mockAppDbContext.Setup(context => context.SaveChanges()).Returns(1);

            this.drinkRepository = new TestDrinkRepository(this.mockAppDbContext.Object);
        }

        [Fact]
        public void ResetDrinkOfTheDay_RemovesAllEntriesFromDrinkOfTheDays()
        {
            int removeRangeCallCount = 0;

            this.mockDrinkOfTheDayDbSet
                .Setup(set => set.RemoveRange(It.IsAny<IEnumerable<DrinkOfTheDay>>()))
                .Callback<IEnumerable<DrinkOfTheDay>>(entries =>
                {
                    removeRangeCallCount++;
                    Assert.Equal(this.drinkOfTheDayData, entries.ToList());
                });

            ((TestDrinkRepository)this.drinkRepository).SetupGetCurrentTopVotedDrink(TOP_VOTED_DRINK_ID);

            this.drinkRepository.ResetDrinkOfTheDay();

            Assert.Equal(1, removeRangeCallCount);
        }

        [Fact]
        public void ResetDrinkOfTheDay_AddsNewDrinkOfTheDayWithTopVotedDrinkId()
        {
            DrinkOfTheDay addedEntry = null!;

            this.mockDrinkOfTheDayDbSet
                .Setup(set => set.Add(It.IsAny<DrinkOfTheDay>()))
                .Callback<DrinkOfTheDay>(entry => addedEntry = entry);

            ((TestDrinkRepository)this.drinkRepository).SetupGetCurrentTopVotedDrink(TOP_VOTED_DRINK_ID);

            this.drinkRepository.ResetDrinkOfTheDay();

            Assert.NotNull(addedEntry);
            Assert.Equal(TOP_VOTED_DRINK_ID, addedEntry.DrinkId);
            Assert.True((DateTime.UtcNow - addedEntry.DrinkTime).TotalSeconds < 5);
        }

        [Fact]
        public void ResetDrinkOfTheDay_CallsSaveChanges()
        {
            int saveChangesCallCount = 0;

            this.mockAppDbContext
                .Setup(context => context.SaveChanges())
                .Callback(() => saveChangesCallCount++)
                .Returns(1);

            ((TestDrinkRepository)this.drinkRepository).SetupGetCurrentTopVotedDrink(TOP_VOTED_DRINK_ID);

            this.drinkRepository.ResetDrinkOfTheDay();

            Assert.Equal(1, saveChangesCallCount);
        }

        private class TestDrinkRepository : DrinkRepository
        {
            private int currentTopVotedDrinkId;

            public TestDrinkRepository(IAppDbContext dbContext) : base(dbContext)
            {
            }

            public void SetupGetCurrentTopVotedDrink(int drinkId)
            {
                this.currentTopVotedDrinkId = drinkId;
            }

            public override int GetCurrentTopVotedDrink()
            {
                return this.currentTopVotedDrinkId;
            }
        }
    }
}