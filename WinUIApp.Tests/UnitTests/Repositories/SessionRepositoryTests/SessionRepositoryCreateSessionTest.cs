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
            sessions = new List<Session>();
            dbSetMock = AsyncQueryableHelper.CreateDbSetMock(sessions);
            dbContextMock = new Mock<IAppDbContext>();
            dbContextMock.Setup(x => x.Sessions).Returns(dbSetMock.Object);
            sessionRepository = new SessionRepository(dbContextMock.Object);
        }

        [Fact]
        public async Task CreateSession_Success_ReturnsNewSession()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(new List<Session>());
            dbContextMock.Setup(x => x.Sessions).Returns(localDbSetMock.Object);
            dbContextMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await sessionRepository.CreateSession(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            localDbSetMock.Verify(x => x.AddAsync(It.Is<Session>(s => s.UserId == userId), default), Times.Once);
            dbContextMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateSession_SaveChangesFails_ReturnsNull()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(new List<Session>());
            dbContextMock.Setup(x => x.Sessions).Returns(localDbSetMock.Object);
            dbContextMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);

            // Act
            var result = await sessionRepository.CreateSession(userId);

            // Assert
            Assert.NotNull(result);
            localDbSetMock.Verify(x => x.AddAsync(It.Is<Session>(s => s.UserId == userId), default), Times.Once);
            dbContextMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateSession_ThrowsException_Throws()
        {
            // Arrange
            var userId = Guid.NewGuid();
            dbContextMock.Setup(x => x.Sessions).Throws(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => sessionRepository.CreateSession(userId));
        }
    }
} 