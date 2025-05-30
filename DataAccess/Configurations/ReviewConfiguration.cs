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

            builder.Property(review => review.DrinkId)
                   .IsRequired();

            builder.Property(review => review.UserId)
                   .IsRequired();

            builder.Property(review => review.RatingValue)
                .HasColumnType("float");
            // Added IsRequired
            builder.Property(review => review.Content)
                   .HasColumnType("text")
                   .IsRequired();

            builder.Property(review => review.IsActive)
                   .HasColumnType("tinyint")
                   .HasDefaultValue(null);

            builder.HasOne(review => review.Drink)
                .WithMany()
                .HasForeignKey(review => review.DrinkId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(review => review.User)
                   .WithMany()
                   .HasForeignKey(review => review.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(review => new { review.UserId, review.DrinkId })
                .IsUnique();

            builder.Property(currentReview => currentReview.CreatedDate).HasColumnType("datetime").IsRequired();
            builder.Property(currentReview => currentReview.NumberOfFlags).IsRequired();
            builder.Property(currentReview => currentReview.IsHidden).IsRequired();
        }
    }
}
