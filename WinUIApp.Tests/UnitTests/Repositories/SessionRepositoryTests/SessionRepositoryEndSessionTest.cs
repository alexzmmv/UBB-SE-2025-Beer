using DataAccess.Model.Authentication;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using WinUiApp.Data.Interfaces;
using WinUIApp.Tests.UnitTests.TestHelpers;
using Moq;

namespace WinUIApp.Tests.UnitTests.Repositories.SessionRepositoryTests
{
    public class SessionRepositoryEndSessionTest
    {
        private readonly Mock<IAppDbContext> dbContextMock;
        private readonly Mock<DbSet<Session>> dbSetMock;
        private readonly SessionRepository sessionRepository;
        private readonly List<Session> sessions;

        public SessionRepositoryEndSessionTest()
        {
            sessions = new List<Session>();
            dbSetMock = AsyncQueryableHelper.CreateDbSetMock(sessions);
            dbContextMock = new Mock<IAppDbContext>();
            dbContextMock.Setup(x => x.Sessions).Returns(dbSetMock.Object);
            sessionRepository = new SessionRepository(dbContextMock.Object);
        }

        [Fact]
        public async Task EndSession_ExistingSession_ReturnsTrue()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var session = new Session { SessionId = sessionId, UserId = userId };
            sessions.Add(session);

            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(sessions);
            dbContextMock.Setup(x => x.Sessions).Returns(localDbSetMock.Object);
            dbContextMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await sessionRepository.EndSession(sessionId);

            // Assert
            Assert.True(result);
            localDbSetMock.Verify(x => x.Remove(session), Times.Once);
            dbContextMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task EndSession_NonExistentSession_ReturnsTrue()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(new List<Session>());
            dbContextMock.Setup(x => x.Sessions).Returns(localDbSetMock.Object);

            // Act
            var result = await sessionRepository.EndSession(sessionId);

            // Assert
            Assert.True(result);
            localDbSetMock.Verify(x => x.Remove(It.IsAny<Session>()), Times.Never);
            dbContextMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task EndSession_SaveChangesFails_ReturnsFalse()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var session = new Session { SessionId = sessionId, UserId = userId };
            sessions.Add(session);

            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(sessions);
            dbContextMock.Setup(x => x.Sessions).Returns(localDbSetMock.Object);
            dbContextMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);

            // Act
            var result = await sessionRepository.EndSession(sessionId);

            // Assert
            Assert.False(result);
            localDbSetMock.Verify(x => x.Remove(session), Times.Once);
            dbContextMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task EndSession_ThrowsException_Throws()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            dbContextMock.Setup(x => x.Sessions).Throws(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => sessionRepository.EndSession(sessionId));
        }
    }
} 