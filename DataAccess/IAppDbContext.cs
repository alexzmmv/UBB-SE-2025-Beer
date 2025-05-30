using DataAccess.Data;
using DataAccess.Model.AdminDashboard;
using DataAccess.Model.Authentication;
using Microsoft.EntityFrameworkCore;
using WinUiApp.Data.Data;

namespace WinUiApp.Data.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Brand> Brands { get; set; }

        DbSet<Category> Categories { get; set; }

        DbSet<Drink> Drinks { get; set; }
        DbSet<DrinkRequestingApproval> DrinksRequestingApproval { get; set; }
        DbSet<DrinkModificationRequest> DrinkModificationRequests { get; set; }

        DbSet<DrinkCategory> DrinkCategories { get; set; }

        DbSet<User> Users { get; set; }

        DbSet<Vote> Votes { get; set; }
        DbSet<DrinkOfTheDay> DrinkOfTheDays { get; set; }

        DbSet<UserDrink> UserDrinks { get; set; }

        DbSet<Review> Reviews { get; set; }

        DbSet<Session> Sessions { get; set; }

        DbSet<Role> Roles { get; set; }

        DbSet<UpgradeRequest> UpgradeRequests { get; set; }

        DbSet<OffensiveWord> OffensiveWords { get; set; }

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
