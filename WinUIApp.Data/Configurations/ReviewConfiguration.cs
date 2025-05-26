using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WinUiApp.Data.Data;

namespace WinUiApp.Data.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(review => review.ReviewId);

            builder.Property(review => review.ReviewId)
                   .ValueGeneratedOnAdd();

            builder.Property(review => review.RatingId)
                   .IsRequired();

            builder.Property(review => review.UserId)
                   .IsRequired();

            builder.Property(review => review.Content)
                   .HasColumnType("text");

            builder.Property(review => review.CreationDate)
                   .HasColumnType("datetime");

            builder.Property(review => review.IsActive)
                   .HasColumnType("tinyint")
                   .HasDefaultValue(null);
                   
            builder.HasOne(review => review.Rating)
                   .WithMany()
                   .HasForeignKey(review => review.RatingId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(review => review.User)
                   .WithMany()
                   .HasForeignKey(review => review.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
