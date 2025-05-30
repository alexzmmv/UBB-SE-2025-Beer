using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DataAccess.Model.AdminDashboard;
using DataAccess.Constants;

namespace DataAccess.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(currentRole => currentRole.RoleType);
            builder.Property(currentRole => currentRole.RoleName).IsRequired().HasMaxLength(10);
            builder.HasData(
                new Role(RoleType.Banned, "Banned"),
                new Role(RoleType.User, "User"),
                new Role(RoleType.Admin, "Admin"));
        }
    }
}
