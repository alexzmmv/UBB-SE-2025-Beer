using DataAccess.Constants;
using DataAccess.IRepository;
using DataAccess.Service;
using WinUiApp.Data.Data;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.UserServiceTests
{
    public class UserServiceGetAllUsersTest
    {
        private readonly Mock<IUserRepository> userRepositoryMock;
        private readonly UserService userService;

        public UserServiceGetAllUsersTest()
        {
            this.userRepositoryMock = new Mock<IUserRepository>();
            this.userService = new UserService(this.userRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllUsers_HasUsers_ReturnsUsersList()
        {
            // Arrange
            var expectedUsers = new List<User>
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
                    AssignedRole = RoleType.Admin
                }
            };

            this.userRepositoryMock.Setup(x => x.GetAllUsers())
                .ReturnsAsync(expectedUsers);

            // Act
            var result = await this.userService.GetAllUsers();

            // Assert
            Assert.Equal(expectedUsers.Count, result.Count);
            Assert.Equal(expectedUsers[0].UserId, result[0].UserId);
            Assert.Equal(expectedUsers[0].Username, result[0].Username);
            Assert.Equal(expectedUsers[1].UserId, result[1].UserId);
            Assert.Equal(expectedUsers[1].Username, result[1].Username);
        }

        [Fact]
        public async Task GetAllUsers_NoUsers_ReturnsEmptyList()
        {
            // Arrange
            this.userRepositoryMock.Setup(x => x.GetAllUsers())
                .ReturnsAsync(new List<User>());

            // Act
            var result = await this.userService.GetAllUsers();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllUsers_Exception_ReturnsEmptyList()
        {
            // Arrange
            this.userRepositoryMock.Setup(x => x.GetAllUsers())
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await this.userService.GetAllUsers();

            // Assert
            Assert.Empty(result);
        }
    }
} 