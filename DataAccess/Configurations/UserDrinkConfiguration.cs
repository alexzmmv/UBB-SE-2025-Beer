using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WinUiApp.Data.Data;

namespace WinUiApp.Data.Configurations
{
    public class UserDrinkConfiguration : IEntityTypeConfiguration<UserDrink>
    {
        public void Configure(EntityTypeBuilder<UserDrink> builder)
        {
            builder.HasKey(userDrink => new { userDrink.UserId, userDrink.DrinkId });

            builder.HasOne(userDrink => userDrink.User)
                   .WithMany()
                   .HasForeignKey(user => user.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(userDrink => userDrink.Drink)
                   .WithMany()
                   .HasForeignKey(drink => drink.DrinkId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
