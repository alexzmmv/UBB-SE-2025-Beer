using DataAccess.Constants;
using DataAccess.IRepository;
using DataAccess.Service;
using WinUiApp.Data.Data;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.UserServiceTests
{
    public class UserServiceGetAdminUsersTest
    {
        private readonly Mock<IUserRepository> userRepositoryMock;
        private readonly UserService userService;

        public UserServiceGetAdminUsersTest()
        {
            this.userRepositoryMock = new Mock<IUserRepository>();
            this.userService = new UserService(this.userRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAdminUsers_HasAdminUsers_ReturnsAdminUsersList()
        {
            // Arrange
            List<User> expectedUsers = new List<User>
            {
                new User
                {
                    UserId = Guid.NewGuid(),
                    Username = "admin1",
                    EmailAddress = "admin1@example.com",
                    AssignedRole = RoleType.Admin
                },
                new User
                {
                    UserId = Guid.NewGuid(),
                    Username = "admin2",
                    EmailAddress = "admin2@example.com",
                    AssignedRole = RoleType.Admin
                }
            };

            this.userRepositoryMock.Setup(userRepository => userRepository.GetUsersByRoleType(RoleType.Admin))
                .ReturnsAsync(expectedUsers);

            // Act
            var result = await this.userService.GetAdminUsers();

            // Assert
            Assert.Equal(expectedUsers.Count, result.Count);
            Assert.Equal(expectedUsers[0].UserId, result[0].UserId);
            Assert.Equal(expectedUsers[0].Username, result[0].Username);
            Assert.Equal(expectedUsers[1].UserId, result[1].UserId);
            Assert.Equal(expectedUsers[1].Username, result[1].Username);
            Assert.All(result, userEntity => Assert.Equal(RoleType.Admin, userEntity.AssignedRole));
        }

        [Fact]
        public async Task GetAdminUsers_NoAdminUsers_ReturnsEmptyList()
        {
            // Arrange
            this.userRepositoryMock.Setup(userRepository => userRepository.GetUsersByRoleType(RoleType.Admin))
                .ReturnsAsync(new List<User>());

            // Act
            var result = await this.userService.GetAdminUsers();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAdminUsers_Exception_ReturnsEmptyList()
        {
            // Arrange
            this.userRepositoryMock.Setup(userRepository => userRepository.GetUsersByRoleType(RoleType.Admin))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await this.userService.GetAdminUsers();

            // Assert
            Assert.Empty(result);
        }
    }
} 