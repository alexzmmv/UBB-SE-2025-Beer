﻿namespace DataAccess.Service.Interfaces
{
    using DataAccess.Constants;
    using DataAccess.Model.AdminDashboard;
    using DataAccess.Model.Authentication;
    using WinUiApp.Data.Data;

    public interface IUserService
    {
        Task<List<User>> GetAllUsers();

        Task<User?> GetUserByUsername(string username);

        Task<List<User>> GetUsersByRoleType(RoleType roleType);

        Task<List<User>> GetActiveUsersByRoleType(RoleType roleType);

        Task<List<User>> GetBannedUsers();

        Task<List<User>> GetBannedUsersWhoHaveSubmittedAppeals();

        Task<List<User>> GetAdminUsers();

        Task<List<User>> GetRegularUsers();

        Task<User?> GetUserById(Guid id);

        Task<RoleType?> GetHighestRoleTypeForUser(Guid id);

        Task UpdateUserRole(Guid userId, RoleType roleType);

        Task UpdateUserAppleaed(User user, bool newValue);

        Task ChangeRoleToUser(Guid userId, Role roleToAdd);

        Task<bool> UpdateUser(User user);

        Task<bool> CreateUser(User user);

        Task<List<User>> GetUsersWhoHaveSubmittedAppeals();

        Task<List<User>> GetUsersWithHiddenReviews();
    }
}