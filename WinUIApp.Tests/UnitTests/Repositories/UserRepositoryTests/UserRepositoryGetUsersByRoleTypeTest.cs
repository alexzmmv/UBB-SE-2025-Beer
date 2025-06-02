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
            users = new List<User>();
            dbSetMock = AsyncQueryableHelper.CreateDbSetMock(users);
            dbContextMock = new Mock<IAppDbContext>();
            dbContextMock.Setup(x => x.Users).Returns(dbSetMock.Object);
            userRepository = new UserRepository(dbContextMock.Object);
        }

        [Fact]
        public async Task GetUsersByRoleType_Success_ReturnsFilteredUsers()
        {
            // Arrange
            var adminUser = new User { UserId = Guid.NewGuid(), Username = "admin", AssignedRole = RoleType.Admin };
            var userUser = new User { UserId = Guid.NewGuid(), Username = "user", AssignedRole = RoleType.User };
            users.AddRange(new[] { adminUser, userUser });

            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(users);
            dbContextMock.Setup(x => x.Users).Returns(localDbSetMock.Object);

            // Act
            var result = await userRepository.GetUsersByRoleType(RoleType.Admin);

            // Assert
            Assert.Single(result);
            Assert.Equal(adminUser.UserId, result[0].UserId);
            Assert.Equal(RoleType.Admin, result[0].AssignedRole);
        }

        [Fact]
        public async Task GetUsersByRoleType_NoUsersWithRole_ReturnsEmptyList()
        {
            // Arrange
            var user = new User { UserId = Guid.NewGuid(), Username = "user", AssignedRole = RoleType.User };
            users.Add(user);

            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(users);
            dbContextMock.Setup(x => x.Users).Returns(localDbSetMock.Object);

            // Act
            var result = await userRepository.GetUsersByRoleType(RoleType.Admin);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetUsersByRoleType_EmptyDatabase_ReturnsEmptyList()
        {
            // Arrange
            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(new List<User>());
            dbContextMock.Setup(x => x.Users).Returns(localDbSetMock.Object);

            // Act
            var result = await userRepository.GetUsersByRoleType(RoleType.Admin);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetUsersByRoleType_DbSetThrowsException_Throws()
        {
            // Arrange
            dbContextMock.Setup(x => x.Users).Throws(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => userRepository.GetUsersByRoleType(RoleType.Admin));
        }
    }
} 