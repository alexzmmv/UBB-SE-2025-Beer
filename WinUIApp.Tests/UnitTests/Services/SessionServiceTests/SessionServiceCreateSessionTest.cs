using DataAccess.IRepository;
using DataAccess.Model.Authentication;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.SessionServiceTests
{
    public class SessionServiceCreateSessionTest
    {
        private readonly Mock<ISessionRepository> sessionRepositoryMock;
        private readonly SessionService sessionService;

        public SessionServiceCreateSessionTest()
        {
            this.sessionRepositoryMock = new Mock<ISessionRepository>();
            this.sessionService = new SessionService(this.sessionRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateSessionAsync_Success_ReturnsSession()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            Session expectedSession = new Session(Guid.NewGuid(), userId);

            this.sessionRepositoryMock.Setup(sessionRepository => sessionRepository.CreateSession(userId))
                .ReturnsAsync(expectedSession);

            // Act
            Session? result = await this.sessionService.CreateSessionAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedSession.SessionId, result.SessionId);
            Assert.Equal(expectedSession.UserId, result.UserId);
        }

        [Fact]
        public async Task CreateSessionAsync_Exception_ReturnsNull()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            this.sessionRepositoryMock.Setup(sessionRepository => sessionRepository.CreateSession(userId))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            Session? result = await this.sessionService.CreateSessionAsync(userId);

            // Assert
            Assert.Null(result);
        }
    }
} 