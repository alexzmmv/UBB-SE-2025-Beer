namespace DataAccess.Repository
{
    using System;
    using System.Collections.Generic;
    using DataAccess.Model.AdminDashboard;
    using IRepository;
    using Microsoft.EntityFrameworkCore;
    using WinUiApp.Data;
    using WinUiApp.Data.Interfaces;

    public class UpgradeRequestsRepository : IUpgradeRequestsRepository
    {
        private readonly IAppDbContext dataContext;

        public UpgradeRequestsRepository(IAppDbContext context)
        {
            dataContext = context;
        }

        public async Task<List<UpgradeRequest>> RetrieveAllUpgradeRequests()
        {
            return await dataContext.UpgradeRequests.ToListAsync();
        }

        public async Task AddUpgradeRequest(Guid userId)
        {
            var username = dataContext.Users.Where(user => user.UserId == userId).FirstOrDefault();

            await dataContext.UpgradeRequests.AddAsync(
                new UpgradeRequest
                {
                    RequestingUserIdentifier = username.UserId,
                    RequestingUserDisplayName = username.Username,
                });
            await dataContext.SaveChangesAsync();
        }

        public async Task RemoveUpgradeRequestByIdentifier(int upgradeRequestIdentifier)
        {
            UpgradeRequest? upgradeRequest = await dataContext.UpgradeRequests.FirstOrDefaultAsync(ur => ur.UpgradeRequestId == upgradeRequestIdentifier);

            if (upgradeRequest == null)
            {
                return;
            }

            dataContext.UpgradeRequests.Remove(upgradeRequest);
            await dataContext.SaveChangesAsync();
        }

        public async Task<UpgradeRequest?> RetrieveUpgradeRequestByIdentifier(int upgradeRequestIdentifier)
        {
            return await dataContext.UpgradeRequests.FirstOrDefaultAsync(ur => ur.UpgradeRequestId == upgradeRequestIdentifier);
        }
    }
}