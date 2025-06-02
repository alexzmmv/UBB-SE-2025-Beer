using DataAccess.Constants;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;
using WinUIApp.Tests.UnitTests.TestHelpers;
using Moq;

namespace WinUIApp.Tests.UnitTests.Repositories.UserRepositoryTests
{
    public class UserRepositoryGetAllUsersTest
    {
        private readonly Mock<IAppDbContext> dbContextMock;
        private readonly Mock<DbSet<User>> dbSetMock;
        private readonly UserRepository userRepository;
        private readonly List<User> users;

        public UserRepositoryGetAllUsersTest()
        {
            users = new List<User>();
            dbSetMock = AsyncQueryableHelper.CreateDbSetMock(users);
            dbContextMock = new Mock<IAppDbContext>();
            dbContextMock.Setup(x => x.Users).Returns(dbSetMock.Object);
            userRepository = new UserRepository(dbContextMock.Object);
        }

        [Fact]
        public async Task GetAllUsers_Success_ReturnsAllUsers()
        {
            // Arrange
            var user1 = new User { UserId = Guid.NewGuid(), Username = "user1" };
            var user2 = new User { UserId = Guid.NewGuid(), Username = "user2" };
            users.AddRange(new[] { user1, user2 });

            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(users);
            dbContextMock.Setup(x => x.Users).Returns(localDbSetMock.Object);

            // Act
            var result = await userRepository.GetAllUsers();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, u => u.UserId == user1.UserId);
            Assert.Contains(result, u => u.UserId == user2.UserId);
        }

        [Fact]
        public async Task GetAllUsers_EmptyDatabase_ReturnsEmptyList()
        {
            // Arrange
            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(new List<User>());
            dbContextMock.Setup(x => x.Users).Returns(localDbSetMock.Object);

            // Act
            var result = await userRepository.GetAllUsers();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllUsers_DbSetThrowsException_Throws()
        {
            // Arrange
            dbContextMock.Setup(x => x.Users).Throws(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => userRepository.GetAllUsers());
        }
    }
} 