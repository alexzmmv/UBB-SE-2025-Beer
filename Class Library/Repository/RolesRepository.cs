namespace DataAccess.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DataAccess.Model.AdminDashboard;
    using IRepository;
    using Microsoft.EntityFrameworkCore;
    using WinUiApp.Data;

    public class RolesRepository : IRolesRepository
    {
        private readonly AppDbContext dataContext;

        public RolesRepository(AppDbContext context)
        {
            dataContext = context;
        }

        public async Task<List<Role>> GetAllRoles()
        {
            CheckForExistingRoles();

            return await dataContext.Roles.ToListAsync();
        }

        public async Task<Role?> GetNextRoleInHierarchy(RoleType currentRoleType)
        {
            CheckForExistingRoles();

            if (currentRoleType.Equals(RoleType.Admin))
            {
                return await dataContext.Roles.FirstOrDefaultAsync(role => role.RoleType == currentRoleType);
            }
            Role? nextRole = await dataContext.Roles.FirstOrDefaultAsync(role => role.RoleType == currentRoleType + 1);
            return nextRole;
        }

        private void CheckForExistingRoles()
        {
            if (!dataContext.Roles.Any())
            {
                AddRole(RoleType.Banned, "Banned");
                AddRole(RoleType.User, "User");
                AddRole(RoleType.Admin, "Admin");
            }
        }

        private void AddRole(RoleType roleType, string roleName)
        {
            dataContext.Roles.Add(new Role(roleType, roleName));
            dataContext.SaveChanges();
        }
    }
}