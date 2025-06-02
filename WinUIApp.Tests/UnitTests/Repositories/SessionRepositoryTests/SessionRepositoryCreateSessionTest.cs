using DataAccess.Model.Authentication;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using WinUiApp.Data.Interfaces;
using WinUIApp.Tests.UnitTests.TestHelpers;
using Moq;

namespace WinUIApp.Tests.UnitTests.Repositories.SessionRepositoryTests
{
    public class SessionRepositoryCreateSessionTest
    {
        private readonly Mock<IAppDbContext> dbContextMock;
        private readonly Mock<DbSet<Session>> dbSetMock;
        private readonly SessionRepository sessionRepository;
        private readonly List<Session> sessions;

        public SessionRepositoryCreateSessionTest()
        {
            this.sessions = new List<Session>();
            this.dbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.sessions);
            this.dbContextMock = new Mock<IAppDbContext>();
            this.dbContextMock.Setup(dbContextMockObject => dbContextMockObject.Sessions).Returns(this.dbSetMock.Object);
            this.sessionRepository = new SessionRepository(this.dbContextMock.Object);
        }

        [Fact]
        public async Task CreateSession_Success_ReturnsNewSession()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            Mock<DbSet<Session>> localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(new List<Session>());
            this.dbContextMock.Setup(dbContextMockObject => dbContextMockObject.Sessions).Returns(localDbSetMock.Object);
            this.dbContextMock.Setup(dbContextMockObject => dbContextMockObject.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            Session result = await this.sessionRepository.CreateSession(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            localDbSetMock.Verify(dbSetMockObject => dbSetMockObject.AddAsync(It.Is<Session>(sessionItem => sessionItem.UserId == userId), default), Times.Once);
            this.dbContextMock.Verify(dbContextMockObject => dbContextMockObject.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateSession_SaveChangesFails_ReturnsNull()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            Mock<DbSet<Session>> localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(new List<Session>());
            this.dbContextMock.Setup(dbContextMockObject => dbContextMockObject.Sessions).Returns(localDbSetMock.Object);
            this.dbContextMock.Setup(dbContextMockObject => dbContextMockObject.SaveChangesAsync()).ReturnsAsync(0);

            // Act
            Session result = await this.sessionRepository.CreateSession(userId);

            // Assert
            Assert.NotNull(result);
            localDbSetMock.Verify(dbSetMockObject => dbSetMockObject.AddAsync(It.Is<Session>(sessionItem => sessionItem.UserId == userId), default), Times.Once);
            this.dbContextMock.Verify(dbContextMockObject => dbContextMockObject.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateSession_ThrowsException_Throws()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            this.dbContextMock.Setup(dbContextMockObject => dbContextMockObject.Sessions).Throws(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => this.sessionRepository.CreateSession(userId));
        }
    }
} 