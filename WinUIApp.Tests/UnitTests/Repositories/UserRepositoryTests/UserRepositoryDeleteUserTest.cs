using DataAccess.Constants;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;
using WinUIApp.Tests.UnitTests.TestHelpers;
using Moq;

namespace WinUIApp.Tests.UnitTests.Repositories.UserRepositoryTests
{
    public class UserRepositoryDeleteUserTest
    {
        private readonly Mock<IAppDbContext> dbContextMock;
        private readonly Mock<DbSet<User>> dbSetMock;
        private readonly UserRepository userRepository;
        private readonly List<User> users;

        public UserRepositoryDeleteUserTest()
        {
            this.users = new List<User>();
            this.dbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.users);
            this.dbContextMock = new Mock<IAppDbContext>();
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(this.dbSetMock.Object);
            this.userRepository = new UserRepository(this.dbContextMock.Object);
        }

        [Fact]
        public async Task DeleteUser_ExistingUser_ReturnsTrue()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            User user = new User { UserId = userId, Username = "testUser" };
            this.users.Add(user);

            Mock<DbSet<User>> localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.users);
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(localDbSetMock.Object);
            this.dbContextMock.Setup(databaseContext => databaseContext.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            bool result = await this.userRepository.DeleteUser(userId);

            // Assert
            Assert.True(result);
            localDbSetMock.Verify(databaseSet => databaseSet.Remove(user), Times.Once);
            this.dbContextMock.Verify(databaseContext => databaseContext.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteUser_NonExistentUser_ReturnsFalse()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            Mock<DbSet<User>> localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(new List<User>());
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(localDbSetMock.Object);

            // Act
            bool result = await this.userRepository.DeleteUser(userId);

            // Assert
            Assert.False(result);
            localDbSetMock.Verify(databaseSet => databaseSet.Remove(It.IsAny<User>()), Times.Never);
            this.dbContextMock.Verify(databaseContext => databaseContext.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteUser_SaveChangesFails_ReturnsFalse()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            User user = new User { UserId = userId, Username = "testUser" };
            this.users.Add(user);

            Mock<DbSet<User>> localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.users);
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(localDbSetMock.Object);
            this.dbContextMock.Setup(databaseContext => databaseContext.SaveChangesAsync()).ReturnsAsync(0);

            // Act
            bool result = await this.userRepository.DeleteUser(userId);

            // Assert
            Assert.False(result);
            localDbSetMock.Verify(databaseSet => databaseSet.Remove(user), Times.Once);
            this.dbContextMock.Verify(databaseContext => databaseContext.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteUser_DbSetThrowsException_Throws()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Throws(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => this.userRepository.DeleteUser(userId));
        }
    }
} 