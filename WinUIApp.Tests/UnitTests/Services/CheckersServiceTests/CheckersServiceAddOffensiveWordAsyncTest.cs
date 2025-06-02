using DataAccess.AutoChecker;
using DataAccess.Service;
using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.CheckersServiceTests
{
    public class CheckersServiceAddOffensiveWordAsyncTest
    {
        private readonly Mock<IReviewService> reviewServiceMock;
        private readonly Mock<IAutoCheck> autoCheckMock;
        private readonly CheckersService checkersService;

        public CheckersServiceAddOffensiveWordAsyncTest()
        {
            reviewServiceMock = new Mock<IReviewService>();
            autoCheckMock = new Mock<IAutoCheck>();
            checkersService = new CheckersService(reviewServiceMock.Object, autoCheckMock.Object);
        }

        [Fact]
        public async Task AddOffensiveWordAsync_ValidWord_CallsAutoCheckMethod()
        {
            // Arrange
            var word = "testword";

            // Act
            await checkersService.AddOffensiveWordAsync(word);

            // Assert
            autoCheckMock.Verify(x => x.AddOffensiveWordAsync(word), Times.Once);
        }

        [Fact]
        public async Task AddOffensiveWordAsync_NullWord_DoesNotCallAutoCheckMethod()
        {
            // Act
            await checkersService.AddOffensiveWordAsync(null);

            // Assert
            autoCheckMock.Verify(x => x.AddOffensiveWordAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task AddOffensiveWordAsync_EmptyWord_DoesNotCallAutoCheckMethod()
        {
            // Act
            await checkersService.AddOffensiveWordAsync(string.Empty);

            // Assert
            autoCheckMock.Verify(x => x.AddOffensiveWordAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task AddOffensiveWordAsync_WhitespaceWord_DoesNotCallAutoCheckMethod()
        {
            // Act
            await checkersService.AddOffensiveWordAsync("   ");

            // Assert
            autoCheckMock.Verify(x => x.AddOffensiveWordAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task AddOffensiveWordAsync_ExceptionThrown_DoesNotThrow()
        {
            // Arrange
            var word = "testword";
            autoCheckMock.Setup(x => x.AddOffensiveWordAsync(word)).ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            await checkersService.AddOffensiveWordAsync(word);
        }

        [Fact]
        public async Task AddOffensiveWordAsync_ExceptionThrown_CallsAutoCheckMethod()
        {
            // Arrange
            var word = "testword";
            autoCheckMock.Setup(x => x.AddOffensiveWordAsync(word)).ThrowsAsync(new Exception("Test exception"));

            // Act
            await checkersService.AddOffensiveWordAsync(word);

            // Assert
            autoCheckMock.Verify(x => x.AddOffensiveWordAsync(word), Times.Once);
        }

        [Fact]
        public async Task AddOffensiveWordAsync_NullAutoCheck_DoesNotCallAutoCheckMethod()
        {
            // Arrange
            var word = "testword";
            var checkersServiceWithNullAutoCheck = new CheckersService(reviewServiceMock.Object, null);

            // Act
            await checkersServiceWithNullAutoCheck.AddOffensiveWordAsync(word);

            // Assert
            autoCheckMock.Verify(x => x.AddOffensiveWordAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task AddOffensiveWordAsync_NullAutoCheck_DoesNotThrow()
        {
            // Arrange
            var word = "testword";
            var checkersServiceWithNullAutoCheck = new CheckersService(reviewServiceMock.Object, null);

            // Act & Assert (should not throw)
            await checkersServiceWithNullAutoCheck.AddOffensiveWordAsync(word);
        }
    }
} 