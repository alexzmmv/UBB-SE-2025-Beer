namespace DataAccess.Service
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DataAccess.Constants;
    using DataAccess.IRepository;
    using DataAccess.Model.AdminDashboard;
    using DataAccess.Service.Interfaces;

    public class RolesService : IRolesService
    {
        private readonly IRolesRepository rolesRepository;

        public RolesService(IRolesRepository rolesRepository)
        {
            this.rolesRepository = rolesRepository;
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            try
            {
                return await this.rolesRepository.GetAllRoles();
            }
            catch
            {
                return new List<Role>();
            }
        }

        public async Task<Role?> GetNextRoleInHierarchyAsync(RoleType currentRoleType)
        {
            try
            {
                return await this.rolesRepository.GetNextRoleInHierarchy(currentRoleType);
            }
            catch
            {
                return null;
            }
        }
    }
}