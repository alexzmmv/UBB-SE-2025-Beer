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
            this.roles = new List<Role>();
            this.dbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.roles);
            this.dbContextMock = new Mock<IAppDbContext>();
            this.dbContextMock.Setup(dbContextMockObject => dbContextMockObject.Roles).Returns(this.dbSetMock.Object);
            this.rolesRepository = new RolesRepository(this.dbContextMock.Object);
        }

        [Fact]
        public async Task GetAllRoles_Success_ReturnsAllRoles()
        {
            // Arrange
            List<Role> testRoles = new List<Role>
            {
                new Role(RoleType.User, "User"),
                new Role(RoleType.Admin, "Admin"),
                new Role(RoleType.Banned, "Banned")
            };
            Mock<DbSet<Role>> localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(testRoles);
            this.dbContextMock.Setup(dbContextMockObject => dbContextMockObject.Roles).Returns(localDbSetMock.Object);

            // Act
            List<Role> result = await this.rolesRepository.GetAllRoles();

            // Assert
            Assert.Equal(testRoles.Count, result.Count);
            Assert.Contains(result, role => role.RoleType == RoleType.User);
            Assert.Contains(result, role => role.RoleType == RoleType.Admin);
            Assert.Contains(result, role => role.RoleType == RoleType.Banned);
        }

        [Fact]
        public async Task GetAllRoles_EmptyDatabase_CreatesDefaultRoles()
        {
            // Arrange
            this.roles.Clear();
            this.dbSetMock.Setup(dbSetMockObject => dbSetMockObject.Add(It.IsAny<Role>())).Callback<Role>(role => this.roles.Add(role));
            this.dbContextMock.Setup(dbContextMockObject => dbContextMockObject.Roles).Returns(this.dbSetMock.Object);
            int saveCallCount = 0;
            this.dbContextMock.Setup(dbContextMockObject => dbContextMockObject.SaveChanges()).Callback(() => {
                saveCallCount++;
            }).Returns(1);

            // Act
            _ = await this.rolesRepository.GetAllRoles();
            // Re-setup the mock to reflect the updated roles list
            Mock<DbSet<Role>> refreshedDbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.roles);
            this.dbContextMock.Setup(dbContextMockObject => dbContextMockObject.Roles).Returns(refreshedDbSetMock.Object);
            List<Role> result = await this.rolesRepository.GetAllRoles();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains(result, role => role.RoleType == RoleType.Banned);
            Assert.Contains(result, role => role.RoleType == RoleType.User);
            Assert.Contains(result, role => role.RoleType == RoleType.Admin);
        }

        [Fact]
        public async Task GetAllRoles_HasExistingRoles_ReturnsRoles()
        {
            // Arrange
            List<Role> existingRoles = new List<Role>
            {
                new Role(RoleType.Banned, "Banned"),
                new Role(RoleType.User, "User"),
                new Role(RoleType.Admin, "Admin")
            };
            Mock<DbSet<Role>> localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(existingRoles);
            this.dbContextMock.Setup(dbContextMockObject => dbContextMockObject.Roles).Returns(localDbSetMock.Object);

            // Act
            List<Role> result = await this.rolesRepository.GetAllRoles();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains(result, role => role.RoleType == RoleType.Banned);
            Assert.Contains(result, role => role.RoleType == RoleType.User);
            Assert.Contains(result, role => role.RoleType == RoleType.Admin);
        }

        [Fact]
        public async Task GetAllRoles_DbSetThrowsException_Throws()
        {
            // Arrange
            this.dbContextMock.Setup(dbContextMockObject => dbContextMockObject.Roles).Throws(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => this.rolesRepository.GetAllRoles());
        }
    }
} 