using DataAccess.Model.AdminDashboard;

namespace DataAccess.Service.Interfaces
{
    public interface IRolesService
    {
        Task<List<Role>> GetAllRolesAsync();

        Task<Role?> GetNextRoleInHierarchyAsync(RoleType currentRoleType);
    }
}