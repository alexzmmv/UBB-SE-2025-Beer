using DataAccess.Constants;
using DataAccess.IRepository;
using DataAccess.Service;
using WinUiApp.Data.Data;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.UserServiceTests
{
    public class UserServiceCreateUserTest
    {
        private readonly Mock<IUserRepository> userRepositoryMock;
        private readonly UserService userService;

        public UserServiceCreateUserTest()
        {
            userRepositoryMock = new Mock<IUserRepository>();
            userService = new UserService(userRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateUser_Success_ReturnsTrue()
        {
            // Arrange
            User user = new User { UserId = Guid.NewGuid(), Username = "testUser" };
            this.userRepositoryMock.Setup(userRepository => userRepository.CreateUser(user)).ReturnsAsync(true);

            // Act
            bool result = await this.userService.CreateUser(user);

            // Assert
            Assert.True(result);
            this.userRepositoryMock.Verify(userRepository => userRepository.CreateUser(user), Times.Once);
        }

        [Fact]
        public async Task CreateUser_NullUser_ReturnsFalse()
        {
            // Act
            bool result = await this.userService.CreateUser(null!);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CreateUser_RepositoryFails_ReturnsFalse()
        {
            // Arrange
            User user = new User { UserId = Guid.NewGuid(), Username = "testUser" };
            this.userRepositoryMock.Setup(userRepository => userRepository.CreateUser(user))
                .ReturnsAsync(false);

            // Act
            bool result = await this.userService.CreateUser(user);

            // Assert
            Assert.False(result);
            this.userRepositoryMock.Verify(userRepository => userRepository.CreateUser(user), Times.Once);
        }

        [Fact]
        public async Task CreateUser_RepositoryThrowsException_ReturnsFalse()
        {
            // Arrange
            User user = new User { UserId = Guid.NewGuid(), Username = "testUser" };
            this.userRepositoryMock.Setup(userRepository => userRepository.CreateUser(user)).ThrowsAsync(new Exception("Test exception"));

            // Act
            bool result = await this.userService.CreateUser(user);

            // Assert
            Assert.False(result);
            this.userRepositoryMock.Verify(userRepository => userRepository.CreateUser(user), Times.Once);
        }
    }
} 