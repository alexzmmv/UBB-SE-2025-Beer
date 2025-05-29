using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WinUiApp.Data.Data;
using DataAccess.Model.AdminDashboard;

namespace DataAccess.Configurations
{
    public class UpgradeRequestsConfiguration : IEntityTypeConfiguration<UpgradeRequest>
    {
        public void Configure(EntityTypeBuilder<UpgradeRequest> builder)
        {
            builder.HasKey(currentRequest => currentRequest.UpgradeRequestId);
            builder.HasOne<User>().WithMany().HasForeignKey(upgradeRequest => upgradeRequest.RequestingUserIdentifier);
            builder.Property(upgradeRequest => upgradeRequest.RequestingUserIdentifier).IsRequired();
        }
    }
}
