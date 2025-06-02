using DataAccess.Repository;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;

namespace WinUIApp.Tests.UnitTests.Repositories.DrinkRepositoryTests
{
    public class DrinkRepositoryUpdateExistingVoteTest
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;
        private readonly Mock<DbSet<Vote>> mockVoteDbSet;
        private readonly DrinkRepository drinkRepository;

        public DrinkRepositoryUpdateExistingVoteTest()
        {
            this.mockAppDbContext = new Mock<IAppDbContext>();
            this.mockVoteDbSet = new Mock<DbSet<Vote>>();

            this.mockAppDbContext.Setup(context => context.Votes).Returns(this.mockVoteDbSet.Object);
            this.mockAppDbContext.Setup(context => context.SaveChanges()).Returns(1);

            this.drinkRepository = new DrinkRepository(this.mockAppDbContext.Object);
        }

        [Fact]
        public void UpdateExistingVote_UpdatesDrinkIdOnVote()
        {
            Vote existingVote = new Vote { DrinkId = 1, UserId = Guid.NewGuid(), VoteTime = DateTime.UtcNow };
            int newDrinkId = 42;

            this.drinkRepository.UpdateExistingVote(existingVote, newDrinkId);

            Assert.Equal(newDrinkId, existingVote.DrinkId);
        }

        [Fact]
        public void UpdateExistingVote_CallsUpdateOnDbSetWithCorrectVote()
        {
            Vote? updatedVote = null;
            Vote existingVote = new Vote { DrinkId = 1, UserId = Guid.NewGuid(), VoteTime = DateTime.UtcNow };
            int newDrinkId = 42;

            this.mockVoteDbSet
                .Setup(set => set.Update(It.IsAny<Vote>()))
                .Callback<Vote>(vote => updatedVote = vote)
                .Returns((EntityEntry<Vote>)null!); // Return null or mock EntityEntry if needed

            this.drinkRepository.UpdateExistingVote(existingVote, newDrinkId);

            Assert.Same(existingVote, updatedVote);
        }

        [Fact]
        public void UpdateExistingVote_CallsSaveChangesOnce()
        {
            int saveChangesCallCount = 0;

            this.mockAppDbContext
                .Setup(context => context.SaveChanges())
                .Callback(() => saveChangesCallCount++)
                .Returns(1);

            Vote existingVote = new Vote { DrinkId = 1, UserId = Guid.NewGuid(), VoteTime = DateTime.UtcNow };

            this.drinkRepository.UpdateExistingVote(existingVote, 42);

            Assert.Equal(1, saveChangesCallCount);
        }
    }
}