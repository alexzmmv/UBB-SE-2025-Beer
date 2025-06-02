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
            Guid userId = Guid.NewGuid();
            Guid sessionId = Guid.NewGuid();
            Session expectedSession = new Session(sessionId, userId);

            this.sessionRepositoryMock.Setup(sessionRepository => sessionRepository.GetSessionByUserId(userId))
                .ReturnsAsync(expectedSession);

            // Act
            Session? result = await this.sessionService.GetSessionByUserIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedSession.SessionId, result.SessionId);
            Assert.Equal(expectedSession.UserId, result.UserId);
        }

        [Fact]
        public async Task GetSessionByUserIdAsync_NotFound_ReturnsNull()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            this.sessionRepositoryMock.Setup(sessionRepository => sessionRepository.GetSessionByUserId(userId))
                .ReturnsAsync((Session?)null);

            // Act
            Session? result = await this.sessionService.GetSessionByUserIdAsync(userId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetSessionByUserIdAsync_Exception_ReturnsNull()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            this.sessionRepositoryMock.Setup(sessionRepository => sessionRepository.GetSessionByUserId(userId))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            Session? result = await this.sessionService.GetSessionByUserIdAsync(userId);

            // Assert
            Assert.Null(result);
        }
    }
} 