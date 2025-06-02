using DataAccess.Constants;
using DataAccess.Model.AdminDashboard;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using WinUiApp.Data.Interfaces;
using WinUIApp.Tests.UnitTests.TestHelpers;
using Moq;

namespace WinUIApp.Tests.UnitTests.Repositories.RolesRepositoryTests
{
    public class RolesRepositoryGetNextRoleInHierarchyTest
    {
        private readonly Mock<IAppDbContext> dbContextMock;
        private readonly Mock<DbSet<Role>> dbSetMock;
        private readonly RolesRepository rolesRepository;
        private readonly List<Role> roles;

        public RolesRepositoryGetNextRoleInHierarchyTest()
        {
            this.roles = new List<Role>();
            this.dbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.roles);
            this.dbContextMock = new Mock<IAppDbContext>();
            this.dbContextMock.Setup(dbContextMockObject => dbContextMockObject.Roles).Returns(this.dbSetMock.Object);
            this.rolesRepository = new RolesRepository(this.dbContextMock.Object);
        }

        [Fact]
        public async Task GetNextRoleInHierarchy_EmptyDatabase_CreatesDefaultRolesAndReturnsNext()
        {
            // Arrange
            List<Role> rolesList = new List<Role>();
            Mock<DbSet<Role>> localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(rolesList);
            this.dbContextMock.Setup(dbContextMockObject => dbContextMockObject.Roles).Returns(localDbSetMock.Object);
            this.dbContextMock.Setup(dbContextMockObject => dbContextMockObject.SaveChanges()).Callback(() => {
                // Simulate EF Core: roles are added to the in-memory list
                if (!rolesList.Any(roleItem => roleItem.RoleType == RoleType.Banned))
                    rolesList.Add(new Role(RoleType.Banned, "Banned"));
                else if (!rolesList.Any(roleItem => roleItem.RoleType == RoleType.User))
                    rolesList.Add(new Role(RoleType.User, "User"));
                else if (!rolesList.Any(roleItem => roleItem.RoleType == RoleType.Admin))
                    rolesList.Add(new Role(RoleType.Admin, "Admin"));
            }).Returns(1);

            // Act
            Role? result = await this.rolesRepository.GetNextRoleInHierarchy(RoleType.Banned);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(RoleType.User, result!.RoleType);
        }

        [Fact]
        public async Task GetNextRoleInHierarchy_HasExistingRoles_ReturnsNextRole()
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
            Role? result = await this.rolesRepository.GetNextRoleInHierarchy(RoleType.User);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(RoleType.Admin, result.RoleType);
        }

        [Fact]
        public async Task GetNextRoleInHierarchy_AdminRole_ReturnsAdmin()
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
            Role? result = await this.rolesRepository.GetNextRoleInHierarchy(RoleType.Admin);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(RoleType.Admin, result.RoleType);
        }

        [Fact]
        public async Task GetNextRoleInHierarchy_DbSetThrowsException_Throws()
        {
            // Arrange
            this.dbContextMock.Setup(dbContextMockObject => dbContextMockObject.Roles).Throws(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => this.rolesRepository.GetNextRoleInHierarchy(RoleType.User));
        }
    }
} 