using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;

namespace WinUIApp.Tests.UnitTests.Repositories.DrinkRepositoryTests
{
    public class DrinkRepositoryVoteDrinkOfTheDayTest
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;
        private Mock<DbSet<Vote>> mockVoteDbSet;
        private readonly DrinkRepository drinkRepository;

        private readonly Guid userId;
        private const int DRINK_ID = 123;

        public DrinkRepositoryVoteDrinkOfTheDayTest()
        {
            this.mockAppDbContext = new Mock<IAppDbContext>();

            this.userId = Guid.NewGuid();

            // Initialize with empty Votes list for each test, can be overridden in tests
            SetupVoteDbSet(new List<Vote>());

            this.mockAppDbContext.Setup(context => context.SaveChanges()).Returns(1);

            this.drinkRepository = new DrinkRepository(this.mockAppDbContext.Object);
        }

        private void SetupVoteDbSet(List<Vote> votes)
        {
            IQueryable<Vote> queryableVotes = votes.AsQueryable();

            this.mockVoteDbSet = new Mock<DbSet<Vote>>();
            this.mockVoteDbSet.As<IQueryable<Vote>>().Setup(m => m.Provider).Returns(queryableVotes.Provider);
            this.mockVoteDbSet.As<IQueryable<Vote>>().Setup(m => m.Expression).Returns(queryableVotes.Expression);
            this.mockVoteDbSet.As<IQueryable<Vote>>().Setup(m => m.ElementType).Returns(queryableVotes.ElementType);
            this.mockVoteDbSet.As<IQueryable<Vote>>().Setup(m => m.GetEnumerator()).Returns(queryableVotes.GetEnumerator());

            this.mockAppDbContext.Setup(context => context.Votes).Returns(this.mockVoteDbSet.Object);
        }

        [Fact]
        public void VoteDrinkOfTheDay_WhenNoExistingVote_CallsAddNewVote()
        {
            // Arrange
            SetupVoteDbSet(new List<Vote>()); // No existing votes

            bool addNewVoteCalled = false;

            this.mockVoteDbSet.Setup(set => set.Add(It.IsAny<Vote>()))
                .Callback<Vote>(vote =>
                {
                    addNewVoteCalled = true;
                    Assert.Equal(this.userId, vote.UserId);
                    Assert.Equal(DRINK_ID, vote.DrinkId);
                    Assert.True((DateTime.UtcNow - vote.VoteTime).TotalSeconds < 5);
                });

            // Act
            this.drinkRepository.VoteDrinkOfTheDay(this.userId, DRINK_ID);

            // Assert
            Assert.True(addNewVoteCalled);
        }

        [Fact]
        public void VoteDrinkOfTheDay_WhenExistingVote_CallsUpdateExistingVote()
        {
            // Arrange
            Vote existingVote = new Vote
            {
                UserId = this.userId,
                DrinkId = 1,
                VoteTime = DateTime.UtcNow.Date
            };
            SetupVoteDbSet(new List<Vote> { existingVote });

            Vote? updatedVote = null;
            this.mockVoteDbSet.Setup(set => set.Update(It.IsAny<Vote>()))
                .Callback<Vote>(vote => updatedVote = vote);

            // Act
            this.drinkRepository.VoteDrinkOfTheDay(this.userId, DRINK_ID);

            // Assert
            Assert.NotNull(updatedVote);
            Assert.Equal(DRINK_ID, updatedVote.DrinkId);
        }
    }
}