using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WinUiApp.Data.Data;

namespace WinUiApp.Data.Configurations
{
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.HasKey(rating => rating.RatingId);

            builder.Property(rating => rating.RatingId)
                   .ValueGeneratedOnAdd();

            builder.Property(rating => rating.DrinkId)
                   .IsRequired();

            builder.Property(rating => rating.UserId)
                   .IsRequired();

            builder.Property(rating => rating.RatingValue)
                   .HasColumnType("float");

            builder.Property(rating => rating.RatingDate)
                   .HasColumnType("datetime");

            builder.Property(rating => rating.IsActive)
                   .HasColumnType("tinyint")
                   .HasDefaultValue(null);

            // User foreign key
            builder.HasOne(rating => rating.User)
                   .WithMany()
                   .HasForeignKey(rating => rating.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Drink foreign key
            builder.HasOne(d => d.Drink)
                   .WithMany()
                   .HasForeignKey(d => d.DrinkId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(rating => new { rating.UserId, rating.DrinkId })
                   .IsUnique();
        }
    }
}
