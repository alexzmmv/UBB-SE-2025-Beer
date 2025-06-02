using DataAccess.Constants;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;
using WinUIApp.Tests.UnitTests.TestHelpers;
using Moq;

namespace WinUIApp.Tests.UnitTests.Repositories.UserRepositoryTests
{
    public class UserRepositoryGetUserByUsernameTest
    {
        private readonly Mock<IAppDbContext> dbContextMock;
        private readonly Mock<DbSet<User>> dbSetMock;
        private readonly UserRepository userRepository;
        private readonly List<User> users;

        public UserRepositoryGetUserByUsernameTest()
        {
            users = new List<User>();
            dbSetMock = AsyncQueryableHelper.CreateDbSetMock(users);
            dbContextMock = new Mock<IAppDbContext>();
            dbContextMock.Setup(x => x.Users).Returns(dbSetMock.Object);
            userRepository = new UserRepository(dbContextMock.Object);
        }

        [Fact]
        public async Task GetUserByUsername_Success_ReturnsUser()
        {
            // Arrange
            var username = "testUser";
            var user = new User { UserId = Guid.NewGuid(), Username = username };
            users.Add(user);

            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(users);
            dbContextMock.Setup(x => x.Users).Returns(localDbSetMock.Object);

            // Act
            var result = await userRepository.GetUserByUsername(username);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(username, result.Username);
        }

        [Fact]
        public async Task GetUserByUsername_NonExistentUser_ReturnsNull()
        {
            // Arrange
            var username = "nonexistent";
            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(new List<User>());
            dbContextMock.Setup(x => x.Users).Returns(localDbSetMock.Object);

            // Act
            var result = await userRepository.GetUserByUsername(username);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserByUsername_DbSetThrowsException_Throws()
        {
            // Arrange
            var username = "testUser";
            dbContextMock.Setup(x => x.Users).Throws(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => userRepository.GetUserByUsername(username));
        }
    }
} 