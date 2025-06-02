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
            this.sessions = new List<Session>();
            this.dbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.sessions);
            this.dbContextMock = new Mock<IAppDbContext>();
            this.dbContextMock.Setup(dbContextMockObject => dbContextMockObject.Sessions).Returns(this.dbSetMock.Object);
            this.sessionRepository = new SessionRepository(this.dbContextMock.Object);
        }

        [Fact]
        public async Task EndSession_ExistingSession_ReturnsTrue()
        {
            // Arrange
            Guid sessionId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            Session session = new Session { SessionId = sessionId, UserId = userId };
            this.sessions.Add(session);

            Mock<DbSet<Session>> localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.sessions);
            this.dbContextMock.Setup(dbContextMockObject => dbContextMockObject.Sessions).Returns(localDbSetMock.Object);
            this.dbContextMock.Setup(dbContextMockObject => dbContextMockObject.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            bool result = await this.sessionRepository.EndSession(sessionId);

            // Assert
            Assert.True(result);
            localDbSetMock.Verify(dbSetMockObject => dbSetMockObject.Remove(session), Times.Once);
            this.dbContextMock.Verify(dbContextMockObject => dbContextMockObject.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task EndSession_NonExistentSession_ReturnsTrue()
        {
            // Arrange
            Guid sessionId = Guid.NewGuid();
            Mock<DbSet<Session>> localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(new List<Session>());
            this.dbContextMock.Setup(dbContextMockObject => dbContextMockObject.Sessions).Returns(localDbSetMock.Object);

            // Act
            bool result = await this.sessionRepository.EndSession(sessionId);

            // Assert
            Assert.True(result);
            localDbSetMock.Verify(dbSetMockObject => dbSetMockObject.Remove(It.IsAny<Session>()), Times.Never);
            this.dbContextMock.Verify(dbContextMockObject => dbContextMockObject.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task EndSession_SaveChangesFails_ReturnsFalse()
        {
            // Arrange
            Guid sessionId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            Session session = new Session { SessionId = sessionId, UserId = userId };
            this.sessions.Add(session);

            Mock<DbSet<Session>> localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.sessions);
            this.dbContextMock.Setup(dbContextMockObject => dbContextMockObject.Sessions).Returns(localDbSetMock.Object);
            this.dbContextMock.Setup(dbContextMockObject => dbContextMockObject.SaveChangesAsync()).ReturnsAsync(0);

            // Act
            bool result = await this.sessionRepository.EndSession(sessionId);

            // Assert
            Assert.False(result);
            localDbSetMock.Verify(dbSetMockObject => dbSetMockObject.Remove(session), Times.Once);
            this.dbContextMock.Verify(dbContextMockObject => dbContextMockObject.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task EndSession_ThrowsException_Throws()
        {
            // Arrange
            Guid sessionId = Guid.NewGuid();
            this.dbContextMock.Setup(dbContextMockObject => dbContextMockObject.Sessions).Throws(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => this.sessionRepository.EndSession(sessionId));
        }
    }
} 