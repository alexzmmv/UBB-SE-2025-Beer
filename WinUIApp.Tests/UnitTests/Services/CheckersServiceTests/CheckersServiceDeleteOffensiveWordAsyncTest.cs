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
            this.reviewServiceMock = new Mock<IReviewService>();
            this.autoCheckMock = new Mock<IAutoCheck>();
            this.checkersService = new CheckersService(this.reviewServiceMock.Object, this.autoCheckMock.Object);
        }

        [Fact]
        public async Task DeleteOffensiveWordAsync_ValidWord_CallsAutoCheckMethod()
        {
            // Arrange
            string word = "testword";

            // Act
            await this.checkersService.DeleteOffensiveWordAsync(word);

            // Assert
            this.autoCheckMock.Verify(x => x.DeleteOffensiveWordAsync(word), Times.Once);
        }

        [Fact]
        public async Task DeleteOffensiveWordAsync_NullWord_DoesNotCallAutoCheckMethod()
        {
            // Act
            await this.checkersService.DeleteOffensiveWordAsync(null);

            // Assert
            this.autoCheckMock.Verify(x => x.DeleteOffensiveWordAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task DeleteOffensiveWordAsync_EmptyWord_DoesNotCallAutoCheckMethod()
        {
            // Act
            await this.checkersService.DeleteOffensiveWordAsync(string.Empty);

            // Assert
            this.autoCheckMock.Verify(x => x.DeleteOffensiveWordAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task DeleteOffensiveWordAsync_WhitespaceWord_DoesNotCallAutoCheckMethod()
        {
            // Act
            await this.checkersService.DeleteOffensiveWordAsync("   ");

            // Assert
            this.autoCheckMock.Verify(x => x.DeleteOffensiveWordAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task DeleteOffensiveWordAsync_ExceptionThrown_DoesNotThrow()
        {
            // Arrange
            string word = "testword";
            this.autoCheckMock.Setup(x => x.DeleteOffensiveWordAsync(word)).ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            await this.checkersService.DeleteOffensiveWordAsync(word);
        }

        [Fact]
        public async Task DeleteOffensiveWordAsync_ExceptionThrown_CallsAutoCheckMethod()
        {
            // Arrange
            string word = "testword";
            this.autoCheckMock.Setup(x => x.DeleteOffensiveWordAsync(word)).ThrowsAsync(new Exception("Test exception"));

            // Act
            await this.checkersService.DeleteOffensiveWordAsync(word);

            // Assert
            this.autoCheckMock.Verify(x => x.DeleteOffensiveWordAsync(word), Times.Once);
        }

        [Fact]
        public async Task DeleteOffensiveWordAsync_NullAutoCheck_DoesNotCallAutoCheckMethod()
        {
            // Arrange
            string word = "testword";
            CheckersService checkersServiceWithNullAutoCheck = new(this.reviewServiceMock.Object, null);

            // Act
            await checkersServiceWithNullAutoCheck.DeleteOffensiveWordAsync(word);

            // Assert
            this.autoCheckMock.Verify(x => x.DeleteOffensiveWordAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task DeleteOffensiveWordAsync_NullAutoCheck_DoesNotThrow()
        {
            // Arrange
            string word = "testword";
            CheckersService checkersServiceWithNullAutoCheck = new(this.reviewServiceMock.Object, null);

            // Act & Assert (should not throw)
            await checkersServiceWithNullAutoCheck.DeleteOffensiveWordAsync(word);
        }
    }
}