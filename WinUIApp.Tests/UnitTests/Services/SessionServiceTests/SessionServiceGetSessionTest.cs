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
            Guid sessionId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            Session expectedSession = new Session(sessionId, userId);

            this.sessionRepositoryMock.Setup(sessionRepository => sessionRepository.GetSession(sessionId))
                .ReturnsAsync(expectedSession);

            // Act
            Session? result = await this.sessionService.GetSessionAsync(sessionId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedSession.SessionId, result.SessionId);
            Assert.Equal(expectedSession.UserId, result.UserId);
        }

        [Fact]
        public async Task GetSessionAsync_NotFound_ReturnsNull()
        {
            // Arrange
            Guid sessionId = Guid.NewGuid();
            this.sessionRepositoryMock.Setup(sessionRepository => sessionRepository.GetSession(sessionId))
                .ReturnsAsync((Session?)null);

            // Act
            Session? result = await this.sessionService.GetSessionAsync(sessionId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetSessionAsync_Exception_ReturnsNull()
        {
            // Arrange
            Guid sessionId = Guid.NewGuid();
            this.sessionRepositoryMock.Setup(sessionRepository => sessionRepository.GetSession(sessionId))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            Session? result = await this.sessionService.GetSessionAsync(sessionId);

            // Assert
            Assert.Null(result);
        }
    }
} 