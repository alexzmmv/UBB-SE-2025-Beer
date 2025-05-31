namespace DataAccess.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DataAccess.Constants;
    using DataAccess.Model.AdminDashboard;
    using IRepository;
    using Microsoft.EntityFrameworkCore;
    using WinUiApp.Data;
    using WinUiApp.Data.Interfaces;

    public class RolesRepository : IRolesRepository
    {
        private readonly IAppDbContext dataContext;

        public RolesRepository(IAppDbContext context)
        {
            this.dataContext = context;
        }

        public async Task<List<Role>> GetAllRoles()
        {
            this.CheckForExistingRoles();

            return await dataContext.Roles.ToListAsync();
        }

        public async Task<Role?> GetNextRoleInHierarchy(RoleType currentRoleType)
        {
            this.CheckForExistingRoles();

            if (currentRoleType.Equals(RoleType.Admin))
            {
                return await dataContext.Roles.FirstOrDefaultAsync(role => role.RoleType == currentRoleType);
            }
            Role? nextRole = await dataContext.Roles.FirstOrDefaultAsync(role => role.RoleType == currentRoleType + 1);
            return nextRole;
        }

        private void CheckForExistingRoles()
        {
            if (!this.dataContext.Roles.Any())
            {
                AddRole(RoleType.Banned, "Banned");
                AddRole(RoleType.User, "User");
                AddRole(RoleType.Admin, "Admin");
            }
        }

        private void AddRole(RoleType roleType, string roleName)
        {
            this.dataContext.Roles.Add(new Role(roleType, roleName));
            this.dataContext.SaveChanges();
        }
    }
}