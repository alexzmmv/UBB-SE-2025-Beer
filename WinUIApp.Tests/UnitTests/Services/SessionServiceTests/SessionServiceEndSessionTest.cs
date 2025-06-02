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
            Guid sessionId = Guid.NewGuid();
            this.sessionRepositoryMock.Setup(sessionRepository => sessionRepository.EndSession(sessionId))
                .ReturnsAsync(true);

            // Act
            bool result = await this.sessionService.EndSessionAsync(sessionId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task EndSessionAsync_Failure_ReturnsFalse()
        {
            // Arrange
            Guid sessionId = Guid.NewGuid();
            this.sessionRepositoryMock.Setup(sessionRepository => sessionRepository.EndSession(sessionId))
                .ReturnsAsync(false);

            // Act
            bool result = await this.sessionService.EndSessionAsync(sessionId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task EndSessionAsync_Exception_ReturnsFalse()
        {
            // Arrange
            Guid sessionId = Guid.NewGuid();
            this.sessionRepositoryMock.Setup(sessionRepository => sessionRepository.EndSession(sessionId))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            bool result = await this.sessionService.EndSessionAsync(sessionId);

            // Assert
            Assert.False(result);
        }
    }
} 