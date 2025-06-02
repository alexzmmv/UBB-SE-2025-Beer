using DataAccess.Constants;
using DataAccess.IRepository;
using DataAccess.Service;
using WinUiApp.Data.Data;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.UserServiceTests
{
    public class UserServiceGetActiveUsersByRoleTypeTest
    {
        private readonly Mock<IUserRepository> userRepositoryMock;
        private readonly UserService userService;

        public UserServiceGetActiveUsersByRoleTypeTest()
        {
            this.userRepositoryMock = new Mock<IUserRepository>();
            this.userService = new UserService(this.userRepositoryMock.Object);
        }

        [Fact]
        public async Task GetActiveUsersByRoleType_ValidRole_ReturnsUsersList()
        {
            // Arrange
            var roleType = RoleType.Admin;
            var expectedUsers = new List<User>
            {
                new User
                {
                    UserId = Guid.NewGuid(),
                    Username = "admin1",
                    EmailAddress = "admin1@example.com",
                    AssignedRole = roleType
                },
                new User
                {
                    UserId = Guid.NewGuid(),
                    Username = "admin2",
                    EmailAddress = "admin2@example.com",
                    AssignedRole = roleType
                }
            };

            this.userRepositoryMock.Setup(x => x.GetUsersByRoleType(roleType))
                .ReturnsAsync(expectedUsers);

            // Act
            var result = await this.userService.GetActiveUsersByRoleType(roleType);

            // Assert
            Assert.Equal(expectedUsers.Count, result.Count);
            Assert.Equal(expectedUsers[0].UserId, result[0].UserId);
            Assert.Equal(expectedUsers[0].Username, result[0].Username);
            Assert.Equal(expectedUsers[1].UserId, result[1].UserId);
            Assert.Equal(expectedUsers[1].Username, result[1].Username);
            Assert.All(result, user => Assert.Equal(roleType, user.AssignedRole));
        }

        [Fact]
        public async Task GetActiveUsersByRoleType_BannedRole_ReturnsEmptyList()
        {
            // Arrange
            var roleType = RoleType.Banned;

            // Act
            var result = await this.userService.GetActiveUsersByRoleType(roleType);

            // Assert
            Assert.Empty(result);
            this.userRepositoryMock.Verify(x => x.GetUsersByRoleType(It.IsAny<RoleType>()), Times.Never);
        }

        [Fact]
        public async Task GetActiveUsersByRoleType_Exception_ReturnsEmptyList()
        {
            // Arrange
            var roleType = RoleType.Admin;
            this.userRepositoryMock.Setup(x => x.GetUsersByRoleType(roleType))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await this.userService.GetActiveUsersByRoleType(roleType);

            // Assert
            Assert.Empty(result);
        }
    }
} 