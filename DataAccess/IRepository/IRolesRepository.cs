﻿namespace DataAccess.IRepository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DataAccess.Constants;
    using DataAccess.Model.AdminDashboard;

    public interface IRolesRepository
    {
        public Task<Role?> GetNextRoleInHierarchy(RoleType currentRoleType);

        public Task<List<Role>> GetAllRoles();
    }
}