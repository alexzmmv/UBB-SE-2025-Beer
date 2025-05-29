using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WinUiApp.Data.Data;
using DataAccess.Model.Authentication;

namespace DataAccess.Configurations
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.HasKey(currentSession => currentSession.SessionId);
            builder.HasOne<User>().WithMany().HasForeignKey(currentSession => currentSession.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
