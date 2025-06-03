using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;

namespace WinUIApp.Tests.UnitTests.Repositories.DrinkRepositoryTests
{
    public class DrinkRepositoryAddNewVoteTest
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;
        private readonly Mock<DbSet<Vote>> mockVoteDbSet;
        private readonly DrinkRepository drinkRepository;

        public DrinkRepositoryAddNewVoteTest()
        {
            this.mockAppDbContext = new Mock<IAppDbContext>();
            this.mockVoteDbSet = new Mock<DbSet<Vote>>();

            this.mockAppDbContext.Setup(context => context.Votes).Returns(this.mockVoteDbSet.Object);
            this.mockAppDbContext.Setup(context => context.SaveChanges()).Returns(1);

            this.drinkRepository = new DrinkRepository(this.mockAppDbContext.Object);
        }

        [Fact]
        public void AddNewVote_AddsVoteToDbSetWithCorrectProperties()
        {
            Vote? addedVote = null;
            Guid expectedUserId = Guid.NewGuid();
            int expectedDrinkId = 123;
            DateTime expectedVoteTime = DateTime.UtcNow;

            this.mockVoteDbSet
                .Setup(set => set.Add(It.IsAny<Vote>()))
                .Callback<Vote>(vote => addedVote = vote);

            this.drinkRepository.AddNewVote(expectedUserId, expectedDrinkId, expectedVoteTime);

            Assert.NotNull(addedVote);
            Assert.Equal(expectedUserId, addedVote.UserId);
            Assert.Equal(expectedDrinkId, addedVote.DrinkId);
            Assert.Equal(expectedVoteTime, addedVote.VoteTime);
        }

        [Fact]
        public void AddNewVote_CallsSaveChangesOnce()
        {
            int saveChangesCallCount = 0;

            this.mockAppDbContext
                .Setup(context => context.SaveChanges())
                .Callback(() => saveChangesCallCount++)
                .Returns(1);

            this.drinkRepository.AddNewVote(Guid.NewGuid(), 1, DateTime.UtcNow);

            Assert.Equal(1, saveChangesCallCount);
        }
    }
}