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
            this.reviewServiceMock = new Mock<IReviewService>();
            this.autoCheckMock = new Mock<IAutoCheck>();
            this.checkersService = new CheckersService(this.reviewServiceMock.Object, this.autoCheckMock.Object);
        }

        [Fact]
        public async Task AddOffensiveWordAsync_ValidWord_CallsAutoCheckMethod()
        {
            // Arrange
            string word = "testword";

            // Act
            await this.checkersService.AddOffensiveWordAsync(word);

            // Assert
            this.autoCheckMock.Verify(x => x.AddOffensiveWordAsync(word), Times.Once);
        }

        [Fact]
        public async Task AddOffensiveWordAsync_NullWord_DoesNotCallAutoCheckMethod()
        {
            // Act
            await this.checkersService.AddOffensiveWordAsync(null);

            // Assert
            this.autoCheckMock.Verify(x => x.AddOffensiveWordAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task AddOffensiveWordAsync_EmptyWord_DoesNotCallAutoCheckMethod()
        {
            // Act
            await this.checkersService.AddOffensiveWordAsync(string.Empty);

            // Assert
            this.autoCheckMock.Verify(x => x.AddOffensiveWordAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task AddOffensiveWordAsync_WhitespaceWord_DoesNotCallAutoCheckMethod()
        {
            // Act
            await this.checkersService.AddOffensiveWordAsync("   ");

            // Assert
            this.autoCheckMock.Verify(x => x.AddOffensiveWordAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task AddOffensiveWordAsync_ExceptionThrown_DoesNotThrow()
        {
            // Arrange
            string word = "testword";
            this.autoCheckMock.Setup(x => x.AddOffensiveWordAsync(word)).ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            await this.checkersService.AddOffensiveWordAsync(word);
        }

        [Fact]
        public async Task AddOffensiveWordAsync_ExceptionThrown_CallsAutoCheckMethod()
        {
            // Arrange
            string word = "testword";
            this.autoCheckMock.Setup(x => x.AddOffensiveWordAsync(word)).ThrowsAsync(new Exception("Test exception"));

            // Act
            await this.checkersService.AddOffensiveWordAsync(word);

            // Assert
            this.autoCheckMock.Verify(x => x.AddOffensiveWordAsync(word), Times.Once);
        }

        [Fact]
        public async Task AddOffensiveWordAsync_NullAutoCheck_DoesNotCallAutoCheckMethod()
        {
            // Arrange
            string word = "testword";
            CheckersService checkersServiceWithNullAutoCheck = new(this.reviewServiceMock.Object, null);

            // Act
            await checkersServiceWithNullAutoCheck.AddOffensiveWordAsync(word);

            // Assert
            this.autoCheckMock.Verify(x => x.AddOffensiveWordAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task AddOffensiveWordAsync_NullAutoCheck_DoesNotThrow()
        {
            // Arrange
            string word = "testword";
            CheckersService checkersServiceWithNullAutoCheck = new(this.reviewServiceMock.Object, null);

            // Act & Assert (should not throw)
            await checkersServiceWithNullAutoCheck.AddOffensiveWordAsync(word);
        }
    }
}