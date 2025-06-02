using DataAccess.IRepository;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.SessionServiceTests
{
    public class SessionServiceEndSessionTest
    {
        private readonly Mock<ISessionRepository> sessionRepositoryMock;
        private readonly SessionService sessionService;

        public SessionServiceEndSessionTest()
        {
            this.sessionRepositoryMock = new Mock<ISessionRepository>();
            this.sessionService = new SessionService(this.sessionRepositoryMock.Object);
        }

        [Fact]
        public async Task EndSessionAsync_Success_ReturnsTrue()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            this.sessionRepositoryMock.Setup(x => x.EndSession(sessionId))
                .ReturnsAsync(true);

            // Act
            var result = await this.sessionService.EndSessionAsync(sessionId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task EndSessionAsync_Failure_ReturnsFalse()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            this.sessionRepositoryMock.Setup(x => x.EndSession(sessionId))
                .ReturnsAsync(false);

            // Act
            var result = await this.sessionService.EndSessionAsync(sessionId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task EndSessionAsync_Exception_ReturnsFalse()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            this.sessionRepositoryMock.Setup(x => x.EndSession(sessionId))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await this.sessionService.EndSessionAsync(sessionId);

            // Assert
            Assert.False(result);
        }
    }
} 