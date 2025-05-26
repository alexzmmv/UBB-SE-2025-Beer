using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DataAccess.Model.AdminDashboard;

namespace DataAccess.Configurations
{
    public class OffensiveWordConfiguration : IEntityTypeConfiguration<OffensiveWord>
    {
        public void Configure(EntityTypeBuilder<OffensiveWord> builder)
        {
            builder.HasKey(offensiveWord => offensiveWord.OffensiveWordId);
            builder.Property(offensiveWord => offensiveWord.Word);
        }
    }
}
