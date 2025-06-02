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
            this.users = new List<User>();
            this.dbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.users);
            this.dbContextMock = new Mock<IAppDbContext>();
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(this.dbSetMock.Object);
            this.userRepository = new UserRepository(this.dbContextMock.Object);
        }

        [Fact]
        public async Task GetAllUsers_Success_ReturnsAllUsers()
        {
            // Arrange
            User user1 = new User { UserId = Guid.NewGuid(), Username = "user1" };
            User user2 = new User { UserId = Guid.NewGuid(), Username = "user2" };
            this.users.AddRange(new[] { user1, user2 });

            Mock<DbSet<User>> localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.users);
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(localDbSetMock.Object);

            // Act
            List<User> result = await this.userRepository.GetAllUsers();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, userItem => userItem.UserId == user1.UserId);
            Assert.Contains(result, userItem => userItem.UserId == user2.UserId);
        }

        [Fact]
        public async Task GetAllUsers_EmptyDatabase_ReturnsEmptyList()
        {
            // Arrange
            Mock<DbSet<User>> localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(new List<User>());
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(localDbSetMock.Object);

            // Act
            List<User> result = await this.userRepository.GetAllUsers();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllUsers_DbSetThrowsException_Throws()
        {
            // Arrange
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Throws(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => this.userRepository.GetAllUsers());
        }
    }
} 