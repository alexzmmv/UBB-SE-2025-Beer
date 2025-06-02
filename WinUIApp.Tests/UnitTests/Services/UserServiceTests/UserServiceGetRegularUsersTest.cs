using DataAccess.Constants;
using DataAccess.IRepository;
using DataAccess.Service;
using WinUiApp.Data.Data;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.UserServiceTests
{
    public class UserServiceGetRegularUsersTest
    {
        private readonly Mock<IUserRepository> userRepositoryMock;
        private readonly UserService userService;

        public UserServiceGetRegularUsersTest()
        {
            this.userRepositoryMock = new Mock<IUserRepository>();
            this.userService = new UserService(this.userRepositoryMock.Object);
        }

        [Fact]
        public async Task GetRegularUsers_HasRegularUsers_ReturnsRegularUsersList()
        {
            // Arrange
            List<User> expectedUsers = new List<User>
            {
                new User
                {
                    UserId = Guid.NewGuid(),
                    Username = "user1",
                    EmailAddress = "user1@example.com",
                    AssignedRole = RoleType.User
                },
                new User
                {
                    UserId = Guid.NewGuid(),
                    Username = "user2",
                    EmailAddress = "user2@example.com",
                    AssignedRole = RoleType.User
                }
            };

            this.userRepositoryMock.Setup(userRepository => userRepository.GetUsersByRoleType(RoleType.User))
                .ReturnsAsync(expectedUsers);

            // Act
            var result = await this.userService.GetRegularUsers();

            // Assert
            Assert.Equal(expectedUsers.Count, result.Count);
            Assert.Equal(expectedUsers[0].UserId, result[0].UserId);
            Assert.Equal(expectedUsers[0].Username, result[0].Username);
            Assert.Equal(expectedUsers[1].UserId, result[1].UserId);
            Assert.Equal(expectedUsers[1].Username, result[1].Username);
            Assert.All(result, userEntity => Assert.Equal(RoleType.User, userEntity.AssignedRole));
        }

        [Fact]
        public async Task GetRegularUsers_NoRegularUsers_ReturnsEmptyList()
        {
            // Arrange
            this.userRepositoryMock.Setup(userRepository => userRepository.GetUsersByRoleType(RoleType.User))
                .ReturnsAsync(new List<User>());

            // Act
            var result = await this.userService.GetRegularUsers();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetRegularUsers_Exception_ReturnsEmptyList()
        {
            // Arrange
            this.userRepositoryMock.Setup(userRepository => userRepository.GetUsersByRoleType(RoleType.User))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await this.userService.GetRegularUsers();

            // Assert
            Assert.Empty(result);
        }
    }
} 