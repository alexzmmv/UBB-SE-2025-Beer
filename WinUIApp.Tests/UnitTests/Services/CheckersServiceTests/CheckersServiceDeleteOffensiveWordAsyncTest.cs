using DataAccess.AutoChecker;
using DataAccess.Service;
using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.CheckersServiceTests
{
    public class CheckersServiceDeleteOffensiveWordAsyncTest
    {
        private readonly Mock<IReviewService> reviewServiceMock;
        private readonly Mock<IAutoCheck> autoCheckMock;
        private readonly CheckersService checkersService;

        public CheckersServiceDeleteOffensiveWordAsyncTest()
        {
            reviewServiceMock = new Mock<IReviewService>();
            autoCheckMock = new Mock<IAutoCheck>();
            checkersService = new CheckersService(reviewServiceMock.Object, autoCheckMock.Object);
        }

        [Fact]
        public async Task DeleteOffensiveWordAsync_ValidWord_CallsAutoCheckMethod()
        {
            // Arrange
            var word = "testword";

            // Act
            await checkersService.DeleteOffensiveWordAsync(word);

            // Assert
            autoCheckMock.Verify(x => x.DeleteOffensiveWordAsync(word), Times.Once);
        }

        [Fact]
        public async Task DeleteOffensiveWordAsync_NullWord_DoesNotCallAutoCheckMethod()
        {
            // Act
            await checkersService.DeleteOffensiveWordAsync(null);

            // Assert
            autoCheckMock.Verify(x => x.DeleteOffensiveWordAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task DeleteOffensiveWordAsync_EmptyWord_DoesNotCallAutoCheckMethod()
        {
            // Act
            await checkersService.DeleteOffensiveWordAsync(string.Empty);

            // Assert
            autoCheckMock.Verify(x => x.DeleteOffensiveWordAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task DeleteOffensiveWordAsync_WhitespaceWord_DoesNotCallAutoCheckMethod()
        {
            // Act
            await checkersService.DeleteOffensiveWordAsync("   ");

            // Assert
            autoCheckMock.Verify(x => x.DeleteOffensiveWordAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task DeleteOffensiveWordAsync_ExceptionThrown_DoesNotThrow()
        {
            // Arrange
            var word = "testword";
            autoCheckMock.Setup(x => x.DeleteOffensiveWordAsync(word)).ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            await checkersService.DeleteOffensiveWordAsync(word);
        }

        [Fact]
        public async Task DeleteOffensiveWordAsync_ExceptionThrown_CallsAutoCheckMethod()
        {
            // Arrange
            var word = "testword";
            autoCheckMock.Setup(x => x.DeleteOffensiveWordAsync(word)).ThrowsAsync(new Exception("Test exception"));

            // Act
            await checkersService.DeleteOffensiveWordAsync(word);

            // Assert
            autoCheckMock.Verify(x => x.DeleteOffensiveWordAsync(word), Times.Once);
        }

        [Fact]
        public async Task DeleteOffensiveWordAsync_NullAutoCheck_DoesNotCallAutoCheckMethod()
        {
            // Arrange
            var word = "testword";
            var checkersServiceWithNullAutoCheck = new CheckersService(reviewServiceMock.Object, null);

            // Act
            await checkersServiceWithNullAutoCheck.DeleteOffensiveWordAsync(word);

            // Assert
            autoCheckMock.Verify(x => x.DeleteOffensiveWordAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task DeleteOffensiveWordAsync_NullAutoCheck_DoesNotThrow()
        {
            // Arrange
            var word = "testword";
            var checkersServiceWithNullAutoCheck = new CheckersService(reviewServiceMock.Object, null);

            // Act & Assert (should not throw)
            await checkersServiceWithNullAutoCheck.DeleteOffensiveWordAsync(word);
        }
    }
} 