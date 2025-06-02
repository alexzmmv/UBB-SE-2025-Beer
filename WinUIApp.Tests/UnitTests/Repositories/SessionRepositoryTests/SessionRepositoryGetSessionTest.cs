using DataAccess.Model.Authentication;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using WinUiApp.Data.Interfaces;
using WinUIApp.Tests.UnitTests.TestHelpers;
using Moq;

namespace WinUIApp.Tests.UnitTests.Repositories.SessionRepositoryTests
{
    public class SessionRepositoryGetSessionTest
    {
        private readonly Mock<IAppDbContext> dbContextMock;
        private readonly Mock<DbSet<Session>> dbSetMock;
        private readonly SessionRepository sessionRepository;
        private readonly List<Session> sessions;

        public SessionRepositoryGetSessionTest()
        {
            sessions = new List<Session>();
            dbSetMock = AsyncQueryableHelper.CreateDbSetMock(sessions);
            dbContextMock = new Mock<IAppDbContext>();
            dbContextMock.Setup(x => x.Sessions).Returns(dbSetMock.Object);
            sessionRepository = new SessionRepository(dbContextMock.Object);
        }

        [Fact]
        public async Task GetSession_ExistingSession_ReturnsSession()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var session = new Session { SessionId = sessionId, UserId = userId };
            sessions.Add(session);

            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(sessions);
            dbContextMock.Setup(x => x.Sessions).Returns(localDbSetMock.Object);

            // Act
            var result = await sessionRepository.GetSession(sessionId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(sessionId, result.SessionId);
            Assert.Equal(userId, result.UserId);
        }

        [Fact]
        public async Task GetSession_NonExistentSession_ReturnsNull()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(new List<Session>());
            dbContextMock.Setup(x => x.Sessions).Returns(localDbSetMock.Object);

            // Act
            var result = await sessionRepository.GetSession(sessionId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetSession_DbSetThrowsException_Throws()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            dbContextMock.Setup(x => x.Sessions).Throws(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => sessionRepository.GetSession(sessionId));
        }
    }
} 