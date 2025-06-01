namespace DataAccess.Repository
{
    using System;
    using System.Collections.Generic;
    using DataAccess.Model.AdminDashboard;
    using IRepository;
    using Microsoft.EntityFrameworkCore;
    using WinUiApp.Data;
    using WinUiApp.Data.Data;
    using WinUiApp.Data.Interfaces;

    public class UpgradeRequestsRepository : IUpgradeRequestsRepository
    {
        private readonly IAppDbContext dataContext;

        public UpgradeRequestsRepository(IAppDbContext context)
        {
            this.dataContext = context;
        }

        public async Task<List<UpgradeRequest>> RetrieveAllUpgradeRequests()
        {
            return await this.dataContext.UpgradeRequests.ToListAsync();
        }

        public async Task AddUpgradeRequest(Guid userId)
        {
            User? username = this.dataContext.Users.Where(user => user.UserId == userId).FirstOrDefault();

            await this.dataContext.UpgradeRequests.AddAsync(
                new UpgradeRequest
                {
                    RequestingUserIdentifier = username.UserId,
                    RequestingUserDisplayName = username.Username,
                });
            await this.dataContext.SaveChangesAsync();
        }
        public async Task<bool> HasPendingUpgradeRequest(Guid userId)
        {
            return await dataContext.UpgradeRequests
                .AnyAsync(ur => ur.RequestingUserIdentifier == userId);
        }
        public async Task RemoveUpgradeRequestByIdentifier(int upgradeRequestIdentifier)
        {
            UpgradeRequest? upgradeRequest = await this.dataContext.UpgradeRequests.FirstOrDefaultAsync(ur => ur.UpgradeRequestId == upgradeRequestIdentifier);

            if (upgradeRequest == null)
            {
                return;
            }

            this.dataContext.UpgradeRequests.Remove(upgradeRequest);
            await this.dataContext.SaveChangesAsync();
        }

        public async Task<UpgradeRequest?> RetrieveUpgradeRequestByIdentifier(int upgradeRequestIdentifier)
        {
            return await this.dataContext.UpgradeRequests.FirstOrDefaultAsync(ur => ur.UpgradeRequestId == upgradeRequestIdentifier);
        }
    }
}