using DataAccess.Constants;
using DataAccess.IRepository;
using DataAccess.Service;
using WinUiApp.Data.Data;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.UserServiceTests
{
    public class UserServiceGetUsersByRoleTypeTest
    {
        private readonly Mock<IUserRepository> userRepositoryMock;
        private readonly UserService userService;

        public UserServiceGetUsersByRoleTypeTest()
        {
            this.userRepositoryMock = new Mock<IUserRepository>();
            this.userService = new UserService(this.userRepositoryMock.Object);
        }

        [Fact]
        public async Task GetUsersByRoleType_HasUsers_ReturnsUsersList()
        {
            // Arrange
            RoleType roleType = RoleType.Admin;
            List<User> expectedUsers = new List<User>
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

            this.userRepositoryMock.Setup(userRepository => userRepository.GetUsersByRoleType(roleType))
                .ReturnsAsync(expectedUsers);

            // Act
            List<User> result = await this.userService.GetUsersByRoleType(roleType);

            // Assert
            Assert.Equal(expectedUsers.Count, result.Count);
            Assert.Equal(expectedUsers[0].UserId, result[0].UserId);
            Assert.Equal(expectedUsers[0].Username, result[0].Username);
            Assert.Equal(expectedUsers[1].UserId, result[1].UserId);
            Assert.Equal(expectedUsers[1].Username, result[1].Username);
            Assert.All(result, userEntity => Assert.Equal(roleType, userEntity.AssignedRole));
        }

        [Fact]
        public async Task GetUsersByRoleType_NoUsers_ReturnsEmptyList()
        {
            // Arrange
            RoleType roleType = RoleType.Admin;
            this.userRepositoryMock.Setup(userRepository => userRepository.GetUsersByRoleType(roleType))
                .ReturnsAsync(new List<User>());

            // Act
            List<User> result = await this.userService.GetUsersByRoleType(roleType);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetUsersByRoleType_Exception_ReturnsEmptyList()
        {
            // Arrange
            RoleType roleType = RoleType.Admin;
            this.userRepositoryMock.Setup(userRepository => userRepository.GetUsersByRoleType(roleType))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            List<User> result = await this.userService.GetUsersByRoleType(roleType);

            // Assert
            Assert.Empty(result);
        }
    }
} 