using DataAccess.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WinUiApp.Data.Data;

namespace WinUiApp.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(currentUser => currentUser.UserId);
            builder.Property(currentUser => currentUser.Username).IsRequired().HasMaxLength(50);
            builder.Property(currentUser => currentUser.PasswordHash).IsRequired();
            builder.Property(currentUser => currentUser.TwoFASecret).IsRequired(false);
            builder.Property(currentUser => currentUser.EmailAddress).IsRequired(false);
            builder.Property(currentUser => currentUser.NumberOfDeletedReviews).IsRequired();
            builder.Property(currentUser => currentUser.HasSubmittedAppeal).IsRequired();
            builder.Property(currentUser => currentUser.AssignedRole).HasDefaultValue(RoleType.User);
        }
    }
}
