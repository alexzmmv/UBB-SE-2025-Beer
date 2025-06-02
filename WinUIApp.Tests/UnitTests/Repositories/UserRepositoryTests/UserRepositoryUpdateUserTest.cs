using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;
using WinUIApp.Tests.UnitTests.TestHelpers;
using Moq;

namespace WinUIApp.Tests.UnitTests.Repositories.UserRepositoryTests
{
    public class UserRepositoryUpdateUserTest
    {
        private readonly Mock<IAppDbContext> dbContextMock;
        private readonly Mock<DbSet<User>> dbSetMock;
        private readonly UserRepository userRepository;
        private readonly List<User> users;

        public UserRepositoryUpdateUserTest()
        {
            this.users = new List<User>();
            this.dbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.users);
            this.dbContextMock = new Mock<IAppDbContext>();
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(this.dbSetMock.Object);
            this.userRepository = new UserRepository(this.dbContextMock.Object);
        }

        [Fact]
        public async Task UpdateUser_Success_ReturnsTrue()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            User user = new User { UserId = userId, Username = "testUser" };
            this.users.Add(user);

            Mock<DbSet<User>> localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.users);
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(localDbSetMock.Object);
            this.dbContextMock.Setup(databaseContext => databaseContext.Users.FindAsync(userId)).ReturnsAsync(user);
            this.dbContextMock.Setup(databaseContext => databaseContext.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            bool result = await this.userRepository.UpdateUser(user);

            // Assert
            Assert.True(result);
            this.dbContextMock.Verify(databaseContext => databaseContext.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateUser_UserNotFound_ThrowsException()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            User user = new User { UserId = userId, Username = "testUser" };
            Mock<DbSet<User>> localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(new List<User>());
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(localDbSetMock.Object);
            this.dbContextMock.Setup(databaseContext => databaseContext.Users.FindAsync(userId)).ReturnsAsync((User)null!);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => this.userRepository.UpdateUser(user));
        }

        [Fact]
        public async Task UpdateUser_NullUser_ThrowsException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => this.userRepository.UpdateUser(null!));
        }

        [Fact]
        public async Task UpdateUser_SaveChangesFails_ReturnsTrue()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            User user = new User { UserId = userId, Username = "testUser" };
            this.users.Add(user);

            Mock<DbSet<User>> localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.users);
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(localDbSetMock.Object);
            this.dbContextMock.Setup(databaseContext => databaseContext.Users.FindAsync(userId)).ReturnsAsync(user);
            this.dbContextMock.Setup(databaseContext => databaseContext.SaveChangesAsync()).ReturnsAsync(0);

            // Act
            bool result = await this.userRepository.UpdateUser(user);

            // Assert
            Assert.True(result); // >= 0 returns true
            this.dbContextMock.Verify(databaseContext => databaseContext.SaveChangesAsync(), Times.Once);
        }
    }
} 