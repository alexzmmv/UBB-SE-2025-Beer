using Microsoft.EntityFrameworkCore;
using WinUiApp.Data.Data;

namespace WinUiApp.Data.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Brand> Brands { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Drink> Drinks { get; set; }
        DbSet<DrinkCategory> DrinkCategories { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Vote> Votes { get; set; }
        DbSet<DrinkOfTheDay> DrinkOfTheDays { get; set; }
        DbSet<UserDrink> UserDrinks { get; set; }
        DbSet<Review> Reviews { get; set; }
        DbSet<Rating> Ratings { get; set; }

        int SaveChanges();
    }
}
