using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;

namespace WinUIApp.Tests.UnitTests.Repositories.DrinkRepositoryTests
{
    public class DrinkRepositoryGetCurrentTopVotedDrinkTest
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;
        private Mock<DbSet<Vote>> mockVoteDbSet;
        private readonly Mock<DbSet<Drink>> mockDrinkDbSet;
        private readonly DrinkRepository drinkRepository;
        private readonly List<Vote> voteData;
        private readonly List<Drink> drinkData;

        public DrinkRepositoryGetCurrentTopVotedDrinkTest()
        {
            this.mockAppDbContext = new Mock<IAppDbContext>();

            DateTime recentDate = DateTime.UtcNow.Date;

            this.voteData = new List<Vote>
            {
                new Vote { DrinkId = 1, VoteTime = recentDate.AddHours(1) },
                new Vote { DrinkId = 1, VoteTime = recentDate.AddHours(2) },
                new Vote { DrinkId = 2, VoteTime = recentDate.AddHours(3) }
            };

            this.drinkData = new List<Drink>
            {
                new Drink { DrinkId = 1, IsRequestingApproval = false },
                new Drink { DrinkId = 2, IsRequestingApproval = false },
                new Drink { DrinkId = 3, IsRequestingApproval = true } // Should be ignored
            };

            this.mockVoteDbSet = this.voteData.AsQueryable().BuildMockDbSet();
            this.mockDrinkDbSet = this.drinkData.AsQueryable().BuildMockDbSet();

            this.mockAppDbContext.Setup(context => context.Votes).Returns(this.mockVoteDbSet.Object);
            this.mockAppDbContext.Setup(context => context.Drinks).Returns(this.mockDrinkDbSet.Object);

            this.drinkRepository = new DrinkRepository(this.mockAppDbContext.Object);
        }

        [Fact]
        public void GetCurrentTopVotedDrink_WhenThereIsTopVote_ReturnsDrinkIdWithMostVotes()
        {
            // Act
            int topVotedDrinkId = this.drinkRepository.GetCurrentTopVotedDrink();

            // Assert
            Assert.Equal(1, topVotedDrinkId);
        }

        [Fact]
        public void GetCurrentTopVotedDrink_WhenNoVotes_ReturnsRandomDrinkId()
        {
            // Arrange
            this.mockVoteDbSet = new List<Vote>().AsQueryable().BuildMockDbSet();
            this.mockAppDbContext.Setup(context => context.Votes).Returns(this.mockVoteDbSet.Object);

            DrinkRepository repositoryWithNoVotes = new DrinkRepository(this.mockAppDbContext.Object);

            // Act
            int result = repositoryWithNoVotes.GetCurrentTopVotedDrink();

            // Assert
            this.mockAppDbContext.Verify(ctx => ctx.Drinks, Times.AtLeastOnce);
            Assert.InRange(result, 1, int.MaxValue); // Assuming GetRandomDrinkId returns positive int, no nulls
        }
    }
}