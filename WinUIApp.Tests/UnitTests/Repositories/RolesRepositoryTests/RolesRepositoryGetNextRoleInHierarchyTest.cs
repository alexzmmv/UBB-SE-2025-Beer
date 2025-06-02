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
            roles = new List<Role>();
            dbSetMock = AsyncQueryableHelper.CreateDbSetMock(roles);
            dbContextMock = new Mock<IAppDbContext>();
            dbContextMock.Setup(x => x.Roles).Returns(dbSetMock.Object);
            rolesRepository = new RolesRepository(dbContextMock.Object);
        }

        [Fact]
        public async Task GetNextRoleInHierarchy_EmptyDatabase_CreatesDefaultRolesAndReturnsNext()
        {
            // Arrange
            var rolesList = new List<Role>();
            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(rolesList);
            dbContextMock.Setup(x => x.Roles).Returns(localDbSetMock.Object);
            dbContextMock.Setup(x => x.SaveChanges()).Callback(() => {
                // Simulate EF Core: roles are added to the in-memory list
                if (!rolesList.Any(r => r.RoleType == RoleType.Banned))
                    rolesList.Add(new Role(RoleType.Banned, "Banned"));
                else if (!rolesList.Any(r => r.RoleType == RoleType.User))
                    rolesList.Add(new Role(RoleType.User, "User"));
                else if (!rolesList.Any(r => r.RoleType == RoleType.Admin))
                    rolesList.Add(new Role(RoleType.Admin, "Admin"));
            }).Returns(1);

            // Act
            var result = await rolesRepository.GetNextRoleInHierarchy(RoleType.Banned);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(RoleType.User, result!.RoleType);
        }

        [Fact]
        public async Task GetNextRoleInHierarchy_HasExistingRoles_ReturnsNextRole()
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
            var result = await rolesRepository.GetNextRoleInHierarchy(RoleType.User);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(RoleType.Admin, result.RoleType);
        }

        [Fact]
        public async Task GetNextRoleInHierarchy_AdminRole_ReturnsAdmin()
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
            var result = await rolesRepository.GetNextRoleInHierarchy(RoleType.Admin);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(RoleType.Admin, result.RoleType);
        }

        [Fact]
        public async Task GetNextRoleInHierarchy_DbSetThrowsException_Throws()
        {
            // Arrange
            dbContextMock.Setup(x => x.Roles).Throws(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => rolesRepository.GetNextRoleInHierarchy(RoleType.User));
        }
    }
} 