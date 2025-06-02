using DataAccess.Constants;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;
using WinUIApp.Tests.UnitTests.TestHelpers;
using Moq;

namespace WinUIApp.Tests.UnitTests.Repositories.UserRepositoryTests
{
    public class UserRepositoryGetRoleTypeForUserTest
    {
        private readonly Mock<IAppDbContext> dbContextMock;
        private readonly Mock<DbSet<User>> dbSetMock;
        private readonly UserRepository userRepository;
        private readonly List<User> users;

        public UserRepositoryGetRoleTypeForUserTest()
        {
            users = new List<User>();
            dbSetMock = AsyncQueryableHelper.CreateDbSetMock(users);
            dbContextMock = new Mock<IAppDbContext>();
            dbContextMock.Setup(x => x.Users).Returns(dbSetMock.Object);
            userRepository = new UserRepository(dbContextMock.Object);
        }

        [Fact]
        public async Task GetRoleTypeForUser_Success_ReturnsRoleType()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { UserId = userId, AssignedRole = RoleType.Admin };
            users.Add(user);

            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(users);
            dbContextMock.Setup(x => x.Users).Returns(localDbSetMock.Object);

            // Act
            var result = await userRepository.GetRoleTypeForUser(userId);

            // Assert
            Assert.Equal(RoleType.Admin, result);
        }

        [Fact]
        public async Task GetRoleTypeForUser_UserNotFound_ReturnsNull()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(new List<User>());
            dbContextMock.Setup(x => x.Users).Returns(localDbSetMock.Object);

            // Act
            var result = await userRepository.GetRoleTypeForUser(userId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetRoleTypeForUser_DbSetThrowsException_Throws()
        {
            // Arrange
            var userId = Guid.NewGuid();
            dbContextMock.Setup(x => x.Users).Throws(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => userRepository.GetRoleTypeForUser(userId));
        }
    }
} 