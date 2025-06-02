using DataAccess.Constants;
using DataAccess.IRepository;
using DataAccess.Service;
using WinUiApp.Data.Data;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.UserServiceTests
{
    public class UserServiceGetUserByIdTest
    {
        private readonly Mock<IUserRepository> userRepositoryMock;
        private readonly UserService userService;

        public UserServiceGetUserByIdTest()
        {
            this.userRepositoryMock = new Mock<IUserRepository>();
            this.userService = new UserService(this.userRepositoryMock.Object);
        }

        [Fact]
        public async Task GetUserById_ExistingUser_ReturnsUser()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            User expectedUser = new User
            {
                UserId = userId,
                Username = "testuser",
                EmailAddress = "test@example.com",
                AssignedRole = RoleType.User
            };

            this.userRepositoryMock.Setup(userRepository => userRepository.GetUserById(userId))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await this.userService.GetUserById(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser.UserId, result.UserId);
            Assert.Equal(expectedUser.Username, result.Username);
            Assert.Equal(expectedUser.EmailAddress, result.EmailAddress);
            Assert.Equal(expectedUser.AssignedRole, result.AssignedRole);
        }

        [Fact]
        public async Task GetUserById_NonExistingUser_ReturnsNull()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            this.userRepositoryMock.Setup(userRepository => userRepository.GetUserById(userId))
                .ReturnsAsync((User?)null);

            // Act
            var result = await this.userService.GetUserById(userId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserById_Exception_ReturnsNull()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            this.userRepositoryMock.Setup(userRepository => userRepository.GetUserById(userId))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await this.userService.GetUserById(userId);

            // Assert
            Assert.Null(result);
        }
    }
} 