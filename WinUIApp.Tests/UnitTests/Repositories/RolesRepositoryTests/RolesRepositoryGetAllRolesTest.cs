using DataAccess.Constants;
using DataAccess.Model.AdminDashboard;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using WinUiApp.Data.Interfaces;
using WinUIApp.Tests.UnitTests.TestHelpers;
using Moq;

namespace WinUIApp.Tests.UnitTests.Repositories.RolesRepositoryTests
{
    public class RolesRepositoryGetAllRolesTest
    {
        private readonly Mock<IAppDbContext> dbContextMock;
        private readonly Mock<DbSet<Role>> dbSetMock;
        private readonly RolesRepository rolesRepository;
        private readonly List<Role> roles;

        public RolesRepositoryGetAllRolesTest()
        {
            roles = new List<Role>();
            dbSetMock = AsyncQueryableHelper.CreateDbSetMock(roles);
            dbContextMock = new Mock<IAppDbContext>();
            dbContextMock.Setup(x => x.Roles).Returns(dbSetMock.Object);
            rolesRepository = new RolesRepository(dbContextMock.Object);
        }

        [Fact]
        public async Task GetAllRoles_Success_ReturnsAllRoles()
        {
            // Arrange
            var testRoles = new List<Role>
            {
                new Role(RoleType.User, "User"),
                new Role(RoleType.Admin, "Admin"),
                new Role(RoleType.Banned, "Banned")
            };
            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(testRoles);
            dbContextMock.Setup(x => x.Roles).Returns(localDbSetMock.Object);

            // Act
            var result = await rolesRepository.GetAllRoles();

            // Assert
            Assert.Equal(testRoles.Count, result.Count);
            Assert.Contains(result, r => r.RoleType == RoleType.User);
            Assert.Contains(result, r => r.RoleType == RoleType.Admin);
            Assert.Contains(result, r => r.RoleType == RoleType.Banned);
        }

        [Fact]
        public async Task GetAllRoles_EmptyDatabase_CreatesDefaultRoles()
        {
            // Arrange
            roles.Clear();
            dbSetMock.Setup(x => x.Add(It.IsAny<Role>())).Callback<Role>(role => roles.Add(role));
            dbContextMock.Setup(x => x.Roles).Returns(dbSetMock.Object);
            int saveCallCount = 0;
            dbContextMock.Setup(x => x.SaveChanges()).Callback(() => {
                saveCallCount++;
            }).Returns(1);

            // Act
            var _ = await rolesRepository.GetAllRoles();
            // Re-setup the mock to reflect the updated roles list
            var refreshedDbSetMock = AsyncQueryableHelper.CreateDbSetMock(roles);
            dbContextMock.Setup(x => x.Roles).Returns(refreshedDbSetMock.Object);
            var result = await rolesRepository.GetAllRoles();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains(result, r => r.RoleType == RoleType.Banned);
            Assert.Contains(result, r => r.RoleType == RoleType.User);
            Assert.Contains(result, r => r.RoleType == RoleType.Admin);
        }

        [Fact]
        public async Task GetAllRoles_HasExistingRoles_ReturnsRoles()
        {
            // Arrange
            var existingRoles = new List<Role>
            {
                new Role(RoleType.Banned, "Banned"),
                new Role(RoleType.User, "User"),
                new Role(RoleType.Admin, "Admin")
            };
            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(existingRoles);
            dbContextMock.Setup(x => x.Roles).Returns(localDbSetMock.Object);

            // Act
            var result = await rolesRepository.GetAllRoles();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains(result, r => r.RoleType == RoleType.Banned);
            Assert.Contains(result, r => r.RoleType == RoleType.User);
            Assert.Contains(result, r => r.RoleType == RoleType.Admin);
        }

        [Fact]
        public async Task GetAllRoles_DbSetThrowsException_Throws()
        {
            // Arrange
            dbContextMock.Setup(x => x.Roles).Throws(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => rolesRepository.GetAllRoles());
        }
    }
} 