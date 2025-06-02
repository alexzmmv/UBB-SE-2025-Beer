using DataAccess.Constants;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;
using WinUIApp.Tests.UnitTests.TestHelpers;
using Moq;

namespace WinUIApp.Tests.UnitTests.Repositories.UserRepositoryTests
{
    public class UserRepositoryGetUsersByRoleTypeTest
    {
        private readonly Mock<IAppDbContext> dbContextMock;
        private readonly Mock<DbSet<User>> dbSetMock;
        private readonly UserRepository userRepository;
        private readonly List<User> users;

        public UserRepositoryGetUsersByRoleTypeTest()
        {
            this.users = new List<User>();
            this.dbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.users);
            this.dbContextMock = new Mock<IAppDbContext>();
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(this.dbSetMock.Object);
            this.userRepository = new UserRepository(this.dbContextMock.Object);
        }

        [Fact]
        public async Task GetUsersByRoleType_Success_ReturnsFilteredUsers()
        {
            // Arrange
            User adminUser = new User { UserId = Guid.NewGuid(), Username = "admin", AssignedRole = RoleType.Admin };
            User userUser = new User { UserId = Guid.NewGuid(), Username = "user", AssignedRole = RoleType.User };
            this.users.AddRange(new[] { adminUser, userUser });

            Mock<DbSet<User>> localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.users);
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(localDbSetMock.Object);

            // Act
            List<User> result = await this.userRepository.GetUsersByRoleType(RoleType.Admin);

            // Assert
            Assert.Single(result);
            Assert.Equal(adminUser.UserId, result[0].UserId);
            Assert.Equal(RoleType.Admin, result[0].AssignedRole);
        }

        [Fact]
        public async Task GetUsersByRoleType_NoUsersWithRole_ReturnsEmptyList()
        {
            // Arrange
            User user = new User { UserId = Guid.NewGuid(), Username = "user", AssignedRole = RoleType.User };
            this.users.Add(user);

            Mock<DbSet<User>> localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.users);
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(localDbSetMock.Object);

            // Act
            List<User> result = await this.userRepository.GetUsersByRoleType(RoleType.Admin);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetUsersByRoleType_EmptyDatabase_ReturnsEmptyList()
        {
            // Arrange
            Mock<DbSet<User>> localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(new List<User>());
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(localDbSetMock.Object);

            // Act
            List<User> result = await this.userRepository.GetUsersByRoleType(RoleType.Admin);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetUsersByRoleType_DbSetThrowsException_Throws()
        {
            // Arrange
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Throws(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => this.userRepository.GetUsersByRoleType(RoleType.Admin));
        }
    }
} 