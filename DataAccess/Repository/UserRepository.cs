namespace DataAccess.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using DataAccess.Constants;
    using DataAccess.Model.AdminDashboard;
    using DataAccess.Model.Authentication;
    using IRepository;
    using Microsoft.EntityFrameworkCore;
    using WinUiApp.Data;
    using WinUiApp.Data.Data;
    using WinUiApp.Data.Interfaces;

    public class UserRepository : IUserRepository
    {
        private readonly IAppDbContext dataContext;
        public UserRepository(IAppDbContext context)
        {
            dataContext = context;
        }

        public async Task<List<User>> GetUsersWhoHaveSubmittedAppeals()
        {
            return await dataContext.Users
                .Where(user => user.HasSubmittedAppeal)
                .ToListAsync();
        }

        public async Task<List<User>> GetUsersByRoleType(RoleType roleType)
        {
            return await dataContext.Users
                .Where(user => user.AssignedRole == roleType)
                .ToListAsync();
        }

        public async Task<RoleType?> GetRoleTypeForUser(Guid userId)
        {
            User? user = await GetUserById(userId);
            return user?.AssignedRole;
        }

        public async Task<List<User>> GetBannedUsersWhoHaveSubmittedAppeals()
        {
             return await dataContext.Users
                .Where(user => user.HasSubmittedAppeal && user.AssignedRole == RoleType.Banned).ToListAsync();
        }

        public async Task ChangeRoleToUser(Guid userId, Role roleToAdd)
        {
            User? user = await GetUserById(userId);

            if (user == null)
            {
                return;
            }

            user.AssignedRole = roleToAdd.RoleType;
            dataContext.Users.Update(user);
            await dataContext.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await dataContext.Users.ToListAsync();
        }

        public virtual async Task<User?> GetUserById(Guid userId)
        {
            return await dataContext.Users.Where(user => user.UserId == userId).FirstOrDefaultAsync();
        }

        public virtual async Task<User?> GetUserByUsername(string username)
        {
            return await dataContext.Users.Where(user => user.Username == username).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateUser(User user)
        {
            User? dbUser = await dataContext.Users.FindAsync(user.UserId);

            if (dbUser == null)
            {
                throw new Exception($"No user found with ID {user.UserId}");
            }

            dbUser.Username = user.Username;
            dbUser.PasswordHash = user.PasswordHash;
            dbUser.EmailAddress = user.EmailAddress;
            dbUser.NumberOfDeletedReviews = user.NumberOfDeletedReviews;
            dbUser.HasSubmittedAppeal = user.HasSubmittedAppeal;
            dbUser.AssignedRole = user.AssignedRole;

            // When is the TwoFactor empty?
            if (!string.IsNullOrEmpty(user.TwoFASecret) && user.TwoFASecret != dbUser.TwoFASecret)
            {
                dbUser.TwoFASecret = user.TwoFASecret;
            }

            // If the update itself has no changes, but no errors occured, return true
            return await dataContext.SaveChangesAsync() >= 0;
        }
        public async Task<bool> DeleteUser(Guid userId)
        {
            User? user = await GetUserById(userId);

            if (user == null)
            {
                return false;
            }

            dataContext.Users.Remove(user);
            return await dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> CreateUser(User user)
        {
            dataContext.Users.Add(user);
            return await dataContext.SaveChangesAsync() > 0;
        }

        public async Task<List<User>> GetUsersWithHiddenReviews()
        {
            return await dataContext.Reviews
                .Where(review => review.IsHidden)
                .Select(review => review.User)
                .Distinct()
                .ToListAsync();
        }
    }
}