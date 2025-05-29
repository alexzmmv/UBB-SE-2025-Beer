using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WinUiApp.Data.Data;

namespace WinUiApp.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(category => category.CategoryId);

            builder.Property(category => category.CategoryId)
                   .ValueGeneratedOnAdd();

            builder.Property(category => category.CategoryName)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.HasIndex(category => category.CategoryName)
                   .IsUnique();
        }
    }
}
