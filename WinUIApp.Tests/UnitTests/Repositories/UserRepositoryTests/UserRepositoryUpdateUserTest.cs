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
            users = new List<User>();
            dbSetMock = AsyncQueryableHelper.CreateDbSetMock(users);
            dbContextMock = new Mock<IAppDbContext>();
            dbContextMock.Setup(x => x.Users).Returns(dbSetMock.Object);
            userRepository = new UserRepository(dbContextMock.Object);
        }

        [Fact]
        public async Task UpdateUser_Success_ReturnsTrue()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { UserId = userId, Username = "testUser" };
            users.Add(user);

            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(users);
            dbContextMock.Setup(x => x.Users).Returns(localDbSetMock.Object);
            dbContextMock.Setup(x => x.Users.FindAsync(userId)).ReturnsAsync(user);
            dbContextMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await userRepository.UpdateUser(user);

            // Assert
            Assert.True(result);
            dbContextMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateUser_UserNotFound_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { UserId = userId, Username = "testUser" };
            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(new List<User>());
            dbContextMock.Setup(x => x.Users).Returns(localDbSetMock.Object);
            dbContextMock.Setup(x => x.Users.FindAsync(userId)).ReturnsAsync((User)null!);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => userRepository.UpdateUser(user));
        }

        [Fact]
        public async Task UpdateUser_NullUser_ThrowsException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => userRepository.UpdateUser(null!));
        }

        [Fact]
        public async Task UpdateUser_SaveChangesFails_ReturnsTrue()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { UserId = userId, Username = "testUser" };
            users.Add(user);

            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(users);
            dbContextMock.Setup(x => x.Users).Returns(localDbSetMock.Object);
            dbContextMock.Setup(x => x.Users.FindAsync(userId)).ReturnsAsync(user);
            dbContextMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);

            // Act
            var result = await userRepository.UpdateUser(user);

            // Assert
            Assert.True(result); // >= 0 returns true
            dbContextMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
} 