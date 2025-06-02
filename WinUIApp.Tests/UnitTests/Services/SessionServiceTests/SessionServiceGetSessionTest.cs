using DataAccess.IRepository;
using DataAccess.Model.Authentication;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.SessionServiceTests
{
    public class SessionServiceGetSessionTest
    {
        private readonly Mock<ISessionRepository> sessionRepositoryMock;
        private readonly SessionService sessionService;

        public SessionServiceGetSessionTest()
        {
            this.sessionRepositoryMock = new Mock<ISessionRepository>();
            this.sessionService = new SessionService(this.sessionRepositoryMock.Object);
        }

        [Fact]
        public async Task GetSessionAsync_Success_ReturnsSession()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var expectedSession = new Session(sessionId, userId);

            this.sessionRepositoryMock.Setup(x => x.GetSession(sessionId))
                .ReturnsAsync(expectedSession);

            // Act
            var result = await this.sessionService.GetSessionAsync(sessionId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedSession.SessionId, result.SessionId);
            Assert.Equal(expectedSession.UserId, result.UserId);
        }

        [Fact]
        public async Task GetSessionAsync_NotFound_ReturnsNull()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            this.sessionRepositoryMock.Setup(x => x.GetSession(sessionId))
                .ReturnsAsync((Session?)null);

            // Act
            var result = await this.sessionService.GetSessionAsync(sessionId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetSessionAsync_Exception_ReturnsNull()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            this.sessionRepositoryMock.Setup(x => x.GetSession(sessionId))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await this.sessionService.GetSessionAsync(sessionId);

            // Assert
            Assert.Null(result);
        }
    }
} 