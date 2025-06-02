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
            var user = new User { UserId = Guid.NewGuid(), Username = "testUser" };
            userRepositoryMock.Setup(x => x.CreateUser(user)).ReturnsAsync(true);

            // Act
            var result = await userService.CreateUser(user);

            // Assert
            Assert.True(result);
            userRepositoryMock.Verify(x => x.CreateUser(user), Times.Once);
        }

        [Fact]
        public async Task CreateUser_NullUser_ReturnsFalse()
        {
            // Act
            var result = await userService.CreateUser(null!);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CreateUser_RepositoryFails_ReturnsFalse()
        {
            // Arrange
            var user = new User { UserId = Guid.NewGuid(), Username = "testUser" };
            this.userRepositoryMock.Setup(x => x.CreateUser(user))
                .ReturnsAsync(false);

            // Act
            var result = await this.userService.CreateUser(user);

            // Assert
            Assert.False(result);
            this.userRepositoryMock.Verify(x => x.CreateUser(user), Times.Once);
        }

        [Fact]
        public async Task CreateUser_RepositoryThrowsException_ReturnsFalse()
        {
            // Arrange
            var user = new User { UserId = Guid.NewGuid(), Username = "testUser" };
            userRepositoryMock.Setup(x => x.CreateUser(user)).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await userService.CreateUser(user);

            // Assert
            Assert.False(result);
            userRepositoryMock.Verify(x => x.CreateUser(user), Times.Once);
        }
    }
} 