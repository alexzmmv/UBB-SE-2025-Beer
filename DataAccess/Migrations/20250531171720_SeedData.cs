using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Check and seed Roles only if they don't exist
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM Roles WHERE RoleType = 1)
                    INSERT INTO Roles (RoleType, RoleName) VALUES (1, 'Customer')
                IF NOT EXISTS (SELECT 1 FROM Roles WHERE RoleType = 2)
                    INSERT INTO Roles (RoleType, RoleName) VALUES (2, 'Admin')
            ");

            // Check and seed Brands only if they don't exist
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM Brands WHERE BrandName = 'Heineken')
                    INSERT INTO Brands (BrandName) VALUES ('Heineken')
                IF NOT EXISTS (SELECT 1 FROM Brands WHERE BrandName = 'Guinness')
                    INSERT INTO Brands (BrandName) VALUES ('Guinness')
                IF NOT EXISTS (SELECT 1 FROM Brands WHERE BrandName = 'Corona')
                    INSERT INTO Brands (BrandName) VALUES ('Corona')
                IF NOT EXISTS (SELECT 1 FROM Brands WHERE BrandName = 'Craft Brewery Co')
                    INSERT INTO Brands (BrandName) VALUES ('Craft Brewery Co')
                IF NOT EXISTS (SELECT 1 FROM Brands WHERE BrandName = 'Blue Moon Brewing')
                    INSERT INTO Brands (BrandName) VALUES ('Blue Moon Brewing')
                IF NOT EXISTS (SELECT 1 FROM Brands WHERE BrandName = 'Stella Artois')
                    INSERT INTO Brands (BrandName) VALUES ('Stella Artois')
                IF NOT EXISTS (SELECT 1 FROM Brands WHERE BrandName = 'Budweiser')
                    INSERT INTO Brands (BrandName) VALUES ('Budweiser')
                IF NOT EXISTS (SELECT 1 FROM Brands WHERE BrandName = 'Carlsberg')
                    INSERT INTO Brands (BrandName) VALUES ('Carlsberg')
            ");

            // Check and seed Categories only if they don't exist
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM Categories WHERE CategoryName = 'Lager')
                    INSERT INTO Categories (CategoryName) VALUES ('Lager')
                IF NOT EXISTS (SELECT 1 FROM Categories WHERE CategoryName = 'Stout')
                    INSERT INTO Categories (CategoryName) VALUES ('Stout')
                IF NOT EXISTS (SELECT 1 FROM Categories WHERE CategoryName = 'IPA')
                    INSERT INTO Categories (CategoryName) VALUES ('IPA')
                IF NOT EXISTS (SELECT 1 FROM Categories WHERE CategoryName = 'Wheat Beer')
                    INSERT INTO Categories (CategoryName) VALUES ('Wheat Beer')
                IF NOT EXISTS (SELECT 1 FROM Categories WHERE CategoryName = 'Pilsner')
                    INSERT INTO Categories (CategoryName) VALUES ('Pilsner')
                IF NOT EXISTS (SELECT 1 FROM Categories WHERE CategoryName = 'Light Beer')
                    INSERT INTO Categories (CategoryName) VALUES ('Light Beer')
            ");

            // Seed Drinks (check if they exist first)
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM Drinks WHERE DrinkName = 'Heineken')
                    INSERT INTO Drinks (DrinkName, DrinkURL, BrandId, AlcoholContent) 
                    VALUES ('Heineken', 'https://www.heineken.com/media/fbb2edc2-e281-4bec-9f0e-0d6df6e3b3b5/YBjkdQ/MasterBrand/Main%20Media/Heineken-bottle.png', 
                    (SELECT BrandId FROM Brands WHERE BrandName = 'Heineken'), 5.0)

                IF NOT EXISTS (SELECT 1 FROM Drinks WHERE DrinkName = 'Guinness Draught')
                    INSERT INTO Drinks (DrinkName, DrinkURL, BrandId, AlcoholContent)
                    VALUES ('Guinness Draught', 'https://www.guinness.com/content/dam/guinness/ie/en/products/draught/Guinness_Draught_Can_500ml.png',
                    (SELECT BrandId FROM Brands WHERE BrandName = 'Guinness'), 4.2)

                IF NOT EXISTS (SELECT 1 FROM Drinks WHERE DrinkName = 'Corona Extra')
                    INSERT INTO Drinks (DrinkName, DrinkURL, BrandId, AlcoholContent)
                    VALUES ('Corona Extra', 'https://cdn.shopify.com/s/files/1/0001/5755/files/corona-extra-bottle.png',
                    (SELECT BrandId FROM Brands WHERE BrandName = 'Corona'), 4.5)

                IF NOT EXISTS (SELECT 1 FROM Drinks WHERE DrinkName = 'IPA Craft Beer')
                    INSERT INTO Drinks (DrinkName, DrinkURL, BrandId, AlcoholContent)
                    VALUES ('IPA Craft Beer', 'https://images.unsplash.com/photo-1558618047-3c8c76ca7d13?w=300',
                    (SELECT BrandId FROM Brands WHERE BrandName = 'Craft Brewery Co'), 6.5)

                IF NOT EXISTS (SELECT 1 FROM Drinks WHERE DrinkName = 'Blue Moon Belgian White')
                    INSERT INTO Drinks (DrinkName, DrinkURL, BrandId, AlcoholContent)
                    VALUES ('Blue Moon Belgian White', 'https://images.unsplash.com/photo-1572635196237-14b3f281503f?w=300',
                    (SELECT BrandId FROM Brands WHERE BrandName = 'Blue Moon Brewing'), 5.4)

                IF NOT EXISTS (SELECT 1 FROM Drinks WHERE DrinkName = 'Stella Artois')
                    INSERT INTO Drinks (DrinkName, DrinkURL, BrandId, AlcoholContent)
                    VALUES ('Stella Artois', 'https://images.unsplash.com/photo-1581636625402-29b2a704ef13?w=300',
                    (SELECT BrandId FROM Brands WHERE BrandName = 'Stella Artois'), 5.2)

                IF NOT EXISTS (SELECT 1 FROM Drinks WHERE DrinkName = 'Budweiser')
                    INSERT INTO Drinks (DrinkName, DrinkURL, BrandId, AlcoholContent)
                    VALUES ('Budweiser', 'https://images.unsplash.com/photo-1571613316887-6f8d5cbf7ef7?w=300',
                    (SELECT BrandId FROM Brands WHERE BrandName = 'Budweiser'), 5.0)

                IF NOT EXISTS (SELECT 1 FROM Drinks WHERE DrinkName = 'Carlsberg')
                    INSERT INTO Drinks (DrinkName, DrinkURL, BrandId, AlcoholContent)
                    VALUES ('Carlsberg', 'https://images.unsplash.com/photo-1566737236500-c8ac43014a8e?w=300',
                    (SELECT BrandId FROM Brands WHERE BrandName = 'Carlsberg'), 5.0)
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = 'admin')
                    INSERT INTO Users (UserId, Username, PasswordHash, EmailAddress, NumberOfDeletedReviews, HasSubmittedAppeal, AssignedRole)
                    VALUES (NEWID(), 'admin', 'admin_password_hash', 'admin@beerapp.com', 0, 0, 2)

                IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = 'bartender1')
                    INSERT INTO Users (UserId, Username, PasswordHash, EmailAddress, NumberOfDeletedReviews, HasSubmittedAppeal, AssignedRole)
                    VALUES (NEWID(), 'bartender1', 'bartender_password_hash', 'bartender@beerapp.com', 0, 0, 1)

                IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = 'manager')
                    INSERT INTO Users (UserId, Username, PasswordHash, EmailAddress, NumberOfDeletedReviews, HasSubmittedAppeal, AssignedRole)
                    VALUES (NEWID(), 'manager', 'manager_password_hash', 'manager@beerapp.com', 0, 0, 2)

                IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = 'john.doe')
                    INSERT INTO Users (UserId, Username, PasswordHash, EmailAddress, NumberOfDeletedReviews, HasSubmittedAppeal, AssignedRole)
                    VALUES (NEWID(), 'john.doe', 'customer_password_hash', 'john.doe@email.com', 0, 0, 1)

                IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = 'jane.smith')
                    INSERT INTO Users (UserId, Username, PasswordHash, EmailAddress, NumberOfDeletedReviews, HasSubmittedAppeal, AssignedRole)
                    VALUES (NEWID(), 'jane.smith', 'customer_password_hash2', 'jane.smith@email.com', 0, 0, 1)
            ");

            // Seed OffensiveWords
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM OffensiveWords WHERE Word = 'spam')
                    INSERT INTO OffensiveWords (Word) VALUES ('spam')
                IF NOT EXISTS (SELECT 1 FROM OffensiveWords WHERE Word = 'fake')
                    INSERT INTO OffensiveWords (Word) VALUES ('fake')
                IF NOT EXISTS (SELECT 1 FROM OffensiveWords WHERE Word = 'terrible')
                    INSERT INTO OffensiveWords (Word) VALUES ('terrible')
                IF NOT EXISTS (SELECT 1 FROM OffensiveWords WHERE Word = 'worst')
                    INSERT INTO OffensiveWords (Word) VALUES ('worst')
                IF NOT EXISTS (SELECT 1 FROM OffensiveWords WHERE Word = 'scam')
                    INSERT INTO OffensiveWords (Word) VALUES ('scam')
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM OffensiveWords WHERE Word IN ('spam', 'fake', 'terrible', 'worst', 'scam')");
            migrationBuilder.Sql("DELETE FROM Users WHERE Username IN ('admin', 'bartender1', 'manager', 'john.doe', 'jane.smith')");
            migrationBuilder.Sql("DELETE FROM Drinks WHERE DrinkName IN ('Heineken', 'Guinness Draught', 'Corona Extra', 'IPA Craft Beer', 'Blue Moon Belgian White', 'Stella Artois', 'Budweiser', 'Carlsberg')");
            migrationBuilder.Sql("DELETE FROM Categories WHERE CategoryName IN ('Lager', 'Stout', 'IPA', 'Wheat Beer', 'Pilsner', 'Light Beer')");
            migrationBuilder.Sql("DELETE FROM Brands WHERE BrandName IN ('Heineken', 'Guinness', 'Corona', 'Craft Brewery Co', 'Blue Moon Brewing', 'Stella Artois', 'Budweiser', 'Carlsberg')");
            // Don't delete roles as they might be needed by the existing user
        }
    }
}