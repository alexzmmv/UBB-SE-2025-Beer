using DataAccess.Data;
using DataAccess.Model.AdminDashboard;
using DataAccess.Model.Authentication;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Constants;
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
    public virtual DbSet<DrinkRequestingApproval> DrinksRequestingApproval { get; set; }
    public virtual DbSet<DrinkModificationRequest> DrinkModificationRequests { get; set; }
    public virtual DbSet<DrinkCategory> DrinkCategories { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Vote> Votes { get; set; }
    public virtual DbSet<DrinkOfTheDay> DrinkOfTheDays { get; set; }
    public virtual DbSet<UserDrink> UserDrinks { get; set; }
    public virtual DbSet<Review> Reviews { get; set; }
    public virtual DbSet<Session> Sessions { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<UpgradeRequest> UpgradeRequests { get; set; }
    public virtual DbSet<OffensiveWord> OffensiveWords { get; set; }

    public override int SaveChanges() => base.SaveChanges();

    public Task<int> SaveChangesAsync() => base.SaveChangesAsync();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
        // Seed Brands
        modelBuilder.Entity<Brand>().HasData(
            new Brand { BrandId = 1, BrandName = "Sunbrew Co." },
            new Brand { BrandId = 2, BrandName = "Berry Spirits" },
            new Brand { BrandId = 3, BrandName = "Mocktails Inc." });
        // Seed Categories
        modelBuilder.Entity<Category>().HasData(
            new Category { CategoryId = 1, CategoryName = "Ale" },
            new Category { CategoryId = 2, CategoryName = "Vodka" },
            new Category { CategoryId = 3, CategoryName = "Soft Drink" });
        // Seed Drinks
        modelBuilder.Entity<Drink>().HasData(
            new Drink
            {
                DrinkId = 1,
                DrinkName = "Golden Ale",
                DrinkURL = "https://example.com/drinks/golden-ale.jpg",
                BrandId = 1,
                AlcoholContent = 5.2m
            },
            new Drink
            {
                DrinkId = 2,
                DrinkName = "Cherry Vodka",
                DrinkURL = "https://example.com/drinks/cherry-vodka.jpg",
                BrandId = 2,
                AlcoholContent = 37.5m
            },
            new Drink
            {
                DrinkId = 3,
                DrinkName = "Ginger Beer",
                DrinkURL = "https://example.com/drinks/ginger-beer.jpg",
                BrandId = 3,
                AlcoholContent = 0.0m
            });
        // Seed DrinkCategories (many-to-many)
        modelBuilder.Entity<DrinkCategory>().HasData(
            new DrinkCategory { DrinkId = 1, CategoryId = 1 },
            new DrinkCategory { DrinkId = 2, CategoryId = 2 },
            new DrinkCategory { DrinkId = 3, CategoryId = 3 } );
        modelBuilder.Entity<User>().HasData(
            new
            {
                UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Username = "john_doe",
                EmailAddress = "john.doe@example.com",
                PasswordHash = "$2a$11$K2xKJ9.vF8wHqJ4bK9mZXeJ8vKlM3nO2pQ7rS9tU1vW3xY4zA5bC6", // Example bcrypt hash
                TwoFASecret = (string?)null,
                NumberOfDeletedReviews = 0,
                HasSubmittedAppeal = false,
                AssignedRole = RoleType.User
            },
            new
            {
                UserId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Username = "jane_smith",
                EmailAddress = "jane.smith@example.com",
                PasswordHash = "$2a$11$L3yLK0.wG9xIrK5cL0nAYfK9wLmN4oP3qR8sT0uV2wX4yZ5aB6dD7", // Example bcrypt hash
                TwoFASecret = (string?)null,
                NumberOfDeletedReviews = 1,
                HasSubmittedAppeal = false,
                AssignedRole = RoleType.User
            }
        );
        // Seed Reviews
        modelBuilder.Entity<Review>().HasData(
            new Review
            {
                ReviewId = 1,
                UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                DrinkId = 1,
                RatingValue = 4.5f,
                Content = "Great taste, smooth and refreshing.",
                CreatedDate = DateTime.Parse("2024-12-20T10:30:00Z").ToUniversalTime(),
                NumberOfFlags = 0,
                IsHidden = false,
                IsActive = true
            },
            new Review
            {
                ReviewId = 2,
                UserId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                DrinkId = 2,
                RatingValue = 3.0f,
                Content = "Too bitter for my preference.",
                CreatedDate = DateTime.Parse("2024-12-20T10:30:00Z").ToUniversalTime(),
                NumberOfFlags = 1,
                IsHidden = false,
                IsActive = true
            }
        );
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
