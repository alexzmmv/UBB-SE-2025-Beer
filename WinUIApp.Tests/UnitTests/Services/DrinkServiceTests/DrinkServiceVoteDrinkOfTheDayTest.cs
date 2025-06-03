using DataAccess.IRepository;
using Moq;
using WinUIApp.WebAPI.Models;
using WinUIApp.WebAPI.Services;

namespace WinUIApp.Tests.UnitTests.Services.DrinkServiceTests
{
    public class DrinkServiceVoteDrinkOfTheDayTest
    {
        private readonly Mock<IDrinkRepository> mockDrinkRepository;
        private readonly DrinkService drinkService;

        private readonly Guid USER_ID = Guid.NewGuid();
        private const int DRINK_ID = 7;

        public DrinkServiceVoteDrinkOfTheDayTest()
        {
            this.mockDrinkRepository = new Mock<IDrinkRepository>();
            this.drinkService = new DrinkService(this.mockDrinkRepository.Object);

            this.mockDrinkRepository
                .Setup(repo => repo.VoteDrinkOfTheDay(USER_ID, DRINK_ID));

            this.mockDrinkRepository
                .Setup(repo => repo.GetDrinkById(DRINK_ID))
                .Returns(new DrinkDTO { DrinkId = DRINK_ID });
        }

        [Fact]
        public void VoteDrinkOfTheDay_WhenCalled_PerformsVoteAndReturnsDrink()
        {
            // Act
            DrinkDTO result = this.drinkService.VoteDrinkOfTheDay(USER_ID, DRINK_ID);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(DRINK_ID, result.DrinkId);
            this.mockDrinkRepository.Verify(repo => repo.VoteDrinkOfTheDay(USER_ID, DRINK_ID), Times.Once);
            this.mockDrinkRepository.Verify(repo => repo.GetDrinkById(DRINK_ID), Times.Once);
        }

        [Fact]
        public void VoteDrinkOfTheDay_WhenRepositoryThrows_ThrowsWrappedException()
        {
            // Arrange
            this.mockDrinkRepository
                .Setup(repo => repo.VoteDrinkOfTheDay(USER_ID, DRINK_ID))
                .Throws(new Exception("Vote failure"));

            // Act & Assert
            Exception exception = Assert.Throws<Exception>(() => this.drinkService.VoteDrinkOfTheDay(USER_ID, DRINK_ID));
            Assert.Contains("Error voting drink", exception.Message);
            Assert.Contains("Vote failure", exception.InnerException?.Message);
        }
    }
}