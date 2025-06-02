namespace DataAccess.IRepository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DataAccess.Model.AdminDashboard;

    public interface IUpgradeRequestsRepository
    {
        Task<List<UpgradeRequest>> RetrieveAllUpgradeRequests();

        Task RemoveUpgradeRequestByIdentifier(int upgradeRequestIdentifier);
        Task AddUpgradeRequest(Guid userId);

        Task<UpgradeRequest?> RetrieveUpgradeRequestByIdentifier(int upgradeRequestIdentifier);

        Task<bool> HasPendingUpgradeRequest(Guid userId);
    }
}