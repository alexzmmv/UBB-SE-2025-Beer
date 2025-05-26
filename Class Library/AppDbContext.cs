using DataAccess.Model.AdminDashboard;
using DataAccess.Model.Authentication;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;

namespace WinUiApp.Data;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Drink> Drinks { get; set; }
    public virtual DbSet<DrinkCategory> DrinkCategories { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Vote> Votes { get; set; }
    public virtual DbSet<DrinkOfTheDay> DrinkOfTheDays { get; set; }
    public virtual DbSet<UserDrink> UserDrinks { get; set; }
    public virtual DbSet<Review> Reviews { get; set; }
    public virtual DbSet<Rating> Ratings { get; set; }
    public virtual DbSet<Session> Sessions { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<UpgradeRequest> UpgradeRequests { get; set; }
    public virtual DbSet<OffensiveWord> OffensiveWords { get; set; }

    public override int SaveChanges() => base.SaveChanges();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    private void ApplyConfigurationsFromAssembly(ModelBuilder modelBuilder)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var entityTypeConfigurations = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
            .ToList();

        foreach (var config in entityTypeConfigurations)
        {
            dynamic instance = Activator.CreateInstance(config);
            modelBuilder.ApplyConfiguration(instance);
        }
    }

}
