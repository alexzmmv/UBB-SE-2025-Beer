using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WinUiApp.Data.Data;

namespace WinUiApp.Data.Configurations
{
    public class DrinkOfTheDayConfiguration : IEntityTypeConfiguration<DrinkOfTheDay>
    {
        public void Configure(EntityTypeBuilder<DrinkOfTheDay> builder)
        {
            builder.HasKey(drink => drink.DrinkId);

            builder.HasOne(drink => drink.Drink)
                   .WithMany()
                   .HasForeignKey(drink => drink.DrinkId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
