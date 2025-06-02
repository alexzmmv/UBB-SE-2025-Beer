using DataAccess.Constants;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;
using WinUIApp.Tests.UnitTests.TestHelpers;
using Moq;

namespace WinUIApp.Tests.UnitTests.Repositories.UserRepositoryTests
{
    public class UserRepositoryCreateUserTest
    {
        private readonly Mock<IAppDbContext> dbContextMock;
        private readonly Mock<DbSet<User>> dbSetMock;
        private readonly UserRepository userRepository;
        private readonly List<User> users;

        public UserRepositoryCreateUserTest()
        {
            this.users = new List<User>();
            this.dbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.users);
            this.dbContextMock = new Mock<IAppDbContext>();
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(this.dbSetMock.Object);
            this.userRepository = new UserRepository(this.dbContextMock.Object);
        }

        [Fact]
        public async Task CreateUser_Success_ReturnsTrue()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            User user = new User { UserId = userId, Username = "testUser" };
            Mock<DbSet<User>> localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(new List<User>());
            this.dbContextMock.Setup(context => context.Users).Returns(localDbSetMock.Object);
            this.dbContextMock.Setup(context => context.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            bool result = await this.userRepository.CreateUser(user);

            // Assert
            Assert.True(result);
            localDbSetMock.Verify(dbSet => dbSet.Add(user), Times.Once);
            this.dbContextMock.Verify(context => context.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateUser_NullUser_ThrowsException()
        {
            // Arrange
            this.dbSetMock.Setup(databaseSet => databaseSet.Add(null!)).Throws(new NullReferenceException());
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(this.dbSetMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => this.userRepository.CreateUser(null!));
        }

        [Fact]
        public async Task CreateUser_SaveChangesFails_ReturnsFalse()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            User user = new User { UserId = userId, Username = "testUser" };
            Mock<DbSet<User>> localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(new List<User>());
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(localDbSetMock.Object);
            this.dbContextMock.Setup(databaseContext => databaseContext.SaveChangesAsync()).ReturnsAsync(0);

            // Act
            bool result = await this.userRepository.CreateUser(user);

            // Assert
            Assert.False(result);
            localDbSetMock.Verify(databaseSet => databaseSet.Add(user), Times.Once);
            this.dbContextMock.Verify(databaseContext => databaseContext.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateUser_ThrowsException_ReturnsFalse()
        {
            // Arrange
            User user = new User { UserId = Guid.NewGuid(), Username = "testUser" };
            this.dbContextMock.Setup(databaseContext => databaseContext.Users.Add(user)).Throws(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => this.userRepository.CreateUser(user));
        }
    }
} 