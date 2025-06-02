using DataAccess.Constants;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;
using WinUIApp.Tests.UnitTests.TestHelpers;
using Moq;

namespace WinUIApp.Tests.UnitTests.Repositories.UserRepositoryTests
{
    public class UserRepositoryCreateUserTest
    {
        private readonly Mock<IAppDbContext> dbContextMock;
        private readonly Mock<DbSet<User>> dbSetMock;
        private readonly UserRepository userRepository;
        private readonly List<User> users;

        public UserRepositoryCreateUserTest()
        {
            users = new List<User>();
            dbSetMock = AsyncQueryableHelper.CreateDbSetMock(users);
            dbContextMock = new Mock<IAppDbContext>();
            dbContextMock.Setup(x => x.Users).Returns(dbSetMock.Object);
            userRepository = new UserRepository(dbContextMock.Object);
        }

        [Fact]
        public async Task CreateUser_Success_ReturnsTrue()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { UserId = userId, Username = "testUser" };
            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(new List<User>());
            dbContextMock.Setup(x => x.Users).Returns(localDbSetMock.Object);
            dbContextMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await userRepository.CreateUser(user);

            // Assert
            Assert.True(result);
            localDbSetMock.Verify(x => x.Add(user), Times.Once);
            dbContextMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateUser_NullUser_ThrowsException()
        {
            // Arrange
            dbSetMock.Setup(x => x.Add(null!)).Throws(new NullReferenceException());
            dbContextMock.Setup(x => x.Users).Returns(dbSetMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => userRepository.CreateUser(null!));
        }

        [Fact]
        public async Task CreateUser_SaveChangesFails_ReturnsFalse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { UserId = userId, Username = "testUser" };
            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(new List<User>());
            dbContextMock.Setup(x => x.Users).Returns(localDbSetMock.Object);
            dbContextMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);

            // Act
            var result = await userRepository.CreateUser(user);

            // Assert
            Assert.False(result);
            localDbSetMock.Verify(x => x.Add(user), Times.Once);
            dbContextMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateUser_ThrowsException_ReturnsFalse()
        {
            // Arrange
            var user = new User { UserId = Guid.NewGuid(), Username = "testUser" };
            dbContextMock.Setup(x => x.Users.Add(user)).Throws(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => userRepository.CreateUser(user));
        }
    }
} 