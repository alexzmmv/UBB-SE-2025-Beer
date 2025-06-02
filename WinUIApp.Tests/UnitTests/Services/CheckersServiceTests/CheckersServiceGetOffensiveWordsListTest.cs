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
            reviewServiceMock = new Mock<IReviewService>();
            autoCheckMock = new Mock<IAutoCheck>();
            checkersService = new CheckersService(reviewServiceMock.Object, autoCheckMock.Object);
        }

        [Fact]
        public async Task GetOffensiveWordsList_Success_ReturnsWordsList()
        {
            // Arrange
            var expectedWords = new HashSet<string> { "word1", "word2", "word3" };
            autoCheckMock.Setup(x => x.GetOffensiveWordsList()).ReturnsAsync(expectedWords);

            // Act
            var result = await checkersService.GetOffensiveWordsList();

            // Assert
            Assert.Equal(expectedWords, result);
        }

        [Fact]
        public async Task GetOffensiveWordsList_Success_CallsAutoCheckMethod()
        {
            // Arrange
            var expectedWords = new HashSet<string> { "word1", "word2" };
            autoCheckMock.Setup(x => x.GetOffensiveWordsList()).ReturnsAsync(expectedWords);

            // Act
            await checkersService.GetOffensiveWordsList();

            // Assert
            autoCheckMock.Verify(x => x.GetOffensiveWordsList(), Times.Once);
        }

        [Fact]
        public async Task GetOffensiveWordsList_EmptyList_ReturnsEmptyHashSet()
        {
            // Arrange
            var emptyWords = new HashSet<string>();
            autoCheckMock.Setup(x => x.GetOffensiveWordsList()).ReturnsAsync(emptyWords);

            // Act
            var result = await checkersService.GetOffensiveWordsList();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetOffensiveWordsList_ExceptionThrown_ReturnsEmptyHashSet()
        {
            // Arrange
            autoCheckMock.Setup(x => x.GetOffensiveWordsList()).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await checkersService.GetOffensiveWordsList();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetOffensiveWordsList_ExceptionThrown_CallsAutoCheckMethod()
        {
            // Arrange
            autoCheckMock.Setup(x => x.GetOffensiveWordsList()).ThrowsAsync(new Exception("Test exception"));

            // Act
            await checkersService.GetOffensiveWordsList();

            // Assert
            autoCheckMock.Verify(x => x.GetOffensiveWordsList(), Times.Once);
        }

        [Fact]
        public async Task GetOffensiveWordsList_NullAutoCheck_ReturnsEmptyHashSet()
        {
            // Arrange
            var checkersServiceWithNullAutoCheck = new CheckersService(reviewServiceMock.Object, null);

            // Act
            var result = await checkersServiceWithNullAutoCheck.GetOffensiveWordsList();

            // Assert
            Assert.Empty(result);
        }
    }
} 