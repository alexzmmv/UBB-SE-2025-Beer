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
            users = new List<User>();
            dbSetMock = AsyncQueryableHelper.CreateDbSetMock(users);
            dbContextMock = new Mock<IAppDbContext>();
            dbContextMock.Setup(x => x.Users).Returns(dbSetMock.Object);
            userRepository = new UserRepository(dbContextMock.Object);
        }

        [Fact]
        public async Task DeleteUser_ExistingUser_ReturnsTrue()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { UserId = userId, Username = "testUser" };
            users.Add(user);

            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(users);
            dbContextMock.Setup(x => x.Users).Returns(localDbSetMock.Object);
            dbContextMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await userRepository.DeleteUser(userId);

            // Assert
            Assert.True(result);
            localDbSetMock.Verify(x => x.Remove(user), Times.Once);
            dbContextMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteUser_NonExistentUser_ReturnsFalse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(new List<User>());
            dbContextMock.Setup(x => x.Users).Returns(localDbSetMock.Object);

            // Act
            var result = await userRepository.DeleteUser(userId);

            // Assert
            Assert.False(result);
            localDbSetMock.Verify(x => x.Remove(It.IsAny<User>()), Times.Never);
            dbContextMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteUser_SaveChangesFails_ReturnsFalse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { UserId = userId, Username = "testUser" };
            users.Add(user);

            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(users);
            dbContextMock.Setup(x => x.Users).Returns(localDbSetMock.Object);
            dbContextMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);

            // Act
            var result = await userRepository.DeleteUser(userId);

            // Assert
            Assert.False(result);
            localDbSetMock.Verify(x => x.Remove(user), Times.Once);
            dbContextMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteUser_DbSetThrowsException_Throws()
        {
            // Arrange
            var userId = Guid.NewGuid();
            dbContextMock.Setup(x => x.Users).Throws(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => userRepository.DeleteUser(userId));
        }
    }
} 