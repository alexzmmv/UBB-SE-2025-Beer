using DataAccess.Constants;
using DataAccess.IRepository;
using DataAccess.Service;
using WinUiApp.Data.Data;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.UserServiceTests
{
    public class UserServiceGetUserByUsernameTest
    {
        private readonly Mock<IUserRepository> userRepositoryMock;
        private readonly UserService userService;

        public UserServiceGetUserByUsernameTest()
        {
            this.userRepositoryMock = new Mock<IUserRepository>();
            this.userService = new UserService(this.userRepositoryMock.Object);
        }

        [Fact]
        public async Task GetUserByUsername_ExistingUser_ReturnsUser()
        {
            // Arrange
            string username = "testuser";
            User expectedUser = new User
            {
                UserId = Guid.NewGuid(),
                Username = username,
                EmailAddress = "test@example.com",
                AssignedRole = RoleType.User
            };

            this.userRepositoryMock.Setup(userRepository => userRepository.GetUserByUsername(username))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await this.userService.GetUserByUsername(username);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser.UserId, result.UserId);
            Assert.Equal(expectedUser.Username, result.Username);
            Assert.Equal(expectedUser.EmailAddress, result.EmailAddress);
            Assert.Equal(expectedUser.AssignedRole, result.AssignedRole);
        }

        [Fact]
        public async Task GetUserByUsername_NonExistingUser_ReturnsNull()
        {
            // Arrange
            string username = "nonexistentuser";
            this.userRepositoryMock.Setup(userRepository => userRepository.GetUserByUsername(username))
                .ReturnsAsync((User?)null);

            // Act
            var result = await this.userService.GetUserByUsername(username);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserByUsername_Exception_ReturnsNull()
        {
            // Arrange
            string username = "testuser";
            this.userRepositoryMock.Setup(userRepository => userRepository.GetUserByUsername(username))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await this.userService.GetUserByUsername(username);

            // Assert
            Assert.Null(result);
        }
    }
} 