using DataAccess.AutoChecker;
using DataAccess.Service;
using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.CheckersServiceTests
{
    public class CheckersServiceGetOffensiveWordsListTest
    {
        private readonly Mock<IReviewService> reviewServiceMock;
        private readonly Mock<IAutoCheck> autoCheckMock;
        private readonly CheckersService checkersService;

        public CheckersServiceGetOffensiveWordsListTest()
        {
            this.reviewServiceMock = new Mock<IReviewService>();
            this.autoCheckMock = new Mock<IAutoCheck>();
            this.checkersService = new CheckersService(this.reviewServiceMock.Object, this.autoCheckMock.Object);
        }

        [Fact]
        public async Task GetOffensiveWordsList_Success_ReturnsWordsList()
        {
            // Arrange
            HashSet<string> expectedWords = new() { "word1", "word2", "word3" };
            this.autoCheckMock.Setup(x => x.GetOffensiveWordsList()).ReturnsAsync(expectedWords);

            // Act
            HashSet<string> result = await this.checkersService.GetOffensiveWordsList();

            // Assert
            Assert.Equal(expectedWords, result);
        }

        [Fact]
        public async Task GetOffensiveWordsList_Success_CallsAutoCheckMethod()
        {
            // Arrange
            HashSet<string> expectedWords = new() { "word1", "word2" };
            this.autoCheckMock.Setup(x => x.GetOffensiveWordsList()).ReturnsAsync(expectedWords);

            // Act
            await this.checkersService.GetOffensiveWordsList();

            // Assert
            this.autoCheckMock.Verify(x => x.GetOffensiveWordsList(), Times.Once);
        }

        [Fact]
        public async Task GetOffensiveWordsList_EmptyList_ReturnsEmptyHashSet()
        {
            // Arrange
            HashSet<string> emptyWords = new();
            this.autoCheckMock.Setup(x => x.GetOffensiveWordsList()).ReturnsAsync(emptyWords);

            // Act
            HashSet<string> result = await this.checkersService.GetOffensiveWordsList();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetOffensiveWordsList_ExceptionThrown_ReturnsEmptyHashSet()
        {
            // Arrange
            this.autoCheckMock.Setup(x => x.GetOffensiveWordsList()).ThrowsAsync(new Exception("Test exception"));

            // Act
            HashSet<string> result = await this.checkersService.GetOffensiveWordsList();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetOffensiveWordsList_ExceptionThrown_CallsAutoCheckMethod()
        {
            // Arrange
            this.autoCheckMock.Setup(x => x.GetOffensiveWordsList()).ThrowsAsync(new Exception("Test exception"));

            // Act
            await this.checkersService.GetOffensiveWordsList();

            // Assert
            this.autoCheckMock.Verify(x => x.GetOffensiveWordsList(), Times.Once);
        }

        [Fact]
        public async Task GetOffensiveWordsList_NullAutoCheck_ReturnsEmptyHashSet()
        {
            // Arrange
            CheckersService checkersServiceWithNullAutoCheck = new(this.reviewServiceMock.Object, null);

            // Act
            HashSet<string> result = await checkersServiceWithNullAutoCheck.GetOffensiveWordsList();

            // Assert
            Assert.Empty(result);
        }
    }
}