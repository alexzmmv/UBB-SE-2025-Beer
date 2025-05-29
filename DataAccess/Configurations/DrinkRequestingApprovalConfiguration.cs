using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WinUiApp.Data.Data;

namespace WinUiApp.Data.Configurations
{
    public class DrinkRequestingApprovalConfiguration : IEntityTypeConfiguration<DrinkRequestingApproval>
    {
        public void Configure(EntityTypeBuilder<DrinkRequestingApproval> builder)
        {
            builder.HasKey(drink => drink.DrinkId);

            builder.Property(drink => drink.DrinkId)
                   .ValueGeneratedOnAdd();

            builder.Property(drink => drink.DrinkName)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.HasOne(drink => drink.Brand)
                   .WithMany()
                   .HasForeignKey(drink => drink.BrandId);
        }
    }
}
