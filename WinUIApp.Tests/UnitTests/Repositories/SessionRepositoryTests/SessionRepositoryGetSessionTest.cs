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
            this.sessions = new List<Session>();
            this.dbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.sessions);
            this.dbContextMock = new Mock<IAppDbContext>();
            this.dbContextMock.Setup(dbContextMockObject => dbContextMockObject.Sessions).Returns(this.dbSetMock.Object);
            this.sessionRepository = new SessionRepository(this.dbContextMock.Object);
        }

        [Fact]
        public async Task GetSession_ExistingSession_ReturnsSession()
        {
            // Arrange
            Guid sessionId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            Session session = new Session { SessionId = sessionId, UserId = userId };
            this.sessions.Add(session);

            Mock<DbSet<Session>> localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.sessions);
            this.dbContextMock.Setup(dbContextMockObject => dbContextMockObject.Sessions).Returns(localDbSetMock.Object);

            // Act
            Session? result = await this.sessionRepository.GetSession(sessionId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(sessionId, result.SessionId);
            Assert.Equal(userId, result.UserId);
        }

        [Fact]
        public async Task GetSession_NonExistentSession_ReturnsNull()
        {
            // Arrange
            Guid sessionId = Guid.NewGuid();
            Mock<DbSet<Session>> localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(new List<Session>());
            this.dbContextMock.Setup(dbContextMockObject => dbContextMockObject.Sessions).Returns(localDbSetMock.Object);

            // Act
            Session? result = await this.sessionRepository.GetSession(sessionId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetSession_DbSetThrowsException_Throws()
        {
            // Arrange
            Guid sessionId = Guid.NewGuid();
            this.dbContextMock.Setup(dbContextMockObject => dbContextMockObject.Sessions).Throws(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => this.sessionRepository.GetSession(sessionId));
        }
    }
} 