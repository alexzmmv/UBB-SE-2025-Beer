using DataAccess.IRepository;
using DataAccess.Model.Authentication;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.SessionServiceTests
{
    public class SessionServiceGetSessionByUserIdTest
    {
        private readonly Mock<ISessionRepository> sessionRepositoryMock;
        private readonly SessionService sessionService;

        public SessionServiceGetSessionByUserIdTest()
        {
            this.sessionRepositoryMock = new Mock<ISessionRepository>();
            this.sessionService = new SessionService(this.sessionRepositoryMock.Object);
        }

        [Fact]
        public async Task GetSessionByUserIdAsync_Success_ReturnsSession()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var sessionId = Guid.NewGuid();
            var expectedSession = new Session(sessionId, userId);

            this.sessionRepositoryMock.Setup(x => x.GetSessionByUserId(userId))
                .ReturnsAsync(expectedSession);

            // Act
            var result = await this.sessionService.GetSessionByUserIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedSession.SessionId, result.SessionId);
            Assert.Equal(expectedSession.UserId, result.UserId);
        }

        [Fact]
        public async Task GetSessionByUserIdAsync_NotFound_ReturnsNull()
        {
            // Arrange
            var userId = Guid.NewGuid();
            this.sessionRepositoryMock.Setup(x => x.GetSessionByUserId(userId))
                .ReturnsAsync((Session?)null);

            // Act
            var result = await this.sessionService.GetSessionByUserIdAsync(userId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetSessionByUserIdAsync_Exception_ReturnsNull()
        {
            // Arrange
            var userId = Guid.NewGuid();
            this.sessionRepositoryMock.Setup(x => x.GetSessionByUserId(userId))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await this.sessionService.GetSessionByUserIdAsync(userId);

            // Assert
            Assert.Null(result);
        }
    }
} 