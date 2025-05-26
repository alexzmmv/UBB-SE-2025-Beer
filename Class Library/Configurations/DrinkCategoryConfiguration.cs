using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WinUiApp.Data.Data;

namespace WinUiApp.Data.Configurations
{
    public class DrinkCategoryConfiguration : IEntityTypeConfiguration<DrinkCategory>
    {
        public void Configure(EntityTypeBuilder<DrinkCategory> builder)
        {
            builder.HasKey(drinkCategory => new { drinkCategory.DrinkId, drinkCategory.CategoryId });

            builder.HasOne(drinkCategory => drinkCategory.Drink)
                   .WithMany(drink => drink.DrinkCategories)
                   .HasForeignKey(drinkCategory => drinkCategory.DrinkId);

            builder.HasOne(drinkCategory => drinkCategory.Category)
                   .WithMany(category => category.DrinkCategories)
                   .HasForeignKey(drinkCategory => drinkCategory.CategoryId);
        }
    }
}
