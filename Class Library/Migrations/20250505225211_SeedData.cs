using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WinUIApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insert Brands
            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "BrandName" },
                values: new object[,]
                {
                    { "Ursugi" },
                    { "Bergenbir" },
                    { "Duvel" },
                    { "Heineken" },
                    { "Guinness" },
                    { "Stella Artois" },
                    { "Corona" },
                    { "BrewDog" },
                    { "Chimay" },
                    { "Trappistes Rochefort" }
                });

            // Insert Categories
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryName" },
                values: new object[,]
                {
                    { "Lager" },
                    { "IPA" },
                    { "Stout" },
                    { "Pilsner" },
                    { "Wheat Beer" },
                    { "Pale Ale" },
                    { "Sour" },
                    { "Porter" },
                    { "Belgian Dubbel" },
                    { "Belgian Tripel" },
                    { "Lambic" }
                });

            // Insert Drinks
            migrationBuilder.InsertData(
                table: "Drinks",
                columns: new[] { "DrinkName", "DrinkURL", "BrandId", "AlcoholContent" },
                values: new object[,]
                {
                    { "Ursugi IPA", "https://floradionline.ro/wp-content/uploads/2023/07/Bere-Ursus-IPA-0.33L-1000x1000-1.jpg", 1, 5.0m },
                    { "Bergenbir Lager", "https://magazin.dorsanimpex.ro/userfiles/944eb0c7-a695-44f0-8596-1da751d9458e/products/66412365_big.jpg", 2, 7.2m },
                    { "Duvel Belgian Strong", "https://vinulbun.ro/custom/imagini/produse/275036008_thb_1_5715_706096_bere-duvel-belgian-strong-blonde-0-33l.JPG", 3, 8.5m },
                    { "Heineken Lager", "https://c.cdnmp.net/877205478/p/l/3/heineken-sticla-0-66l-bax-12-buc~19803.jpg", 4, 5.0m },
                    { "Guinness Draught", "https://www.telegraph.co.uk/content/dam/health-fitness/2024/11/26/TELEMMGLPICT000403161538_17326343319300_trans_NvBQzQNjv4BqqVzuuqpFlyLIwiB6NTmJwfSVWeZ_vEN7c6bHu2jJnT8.jpeg?imwidth=680", 5, 4.2m },
                    { "Stella Artois Lager", "https://www.gourmetencasa-tcm.com/15353-large_default/stella-artois-33cl.jpg", 6, 5.0m },
                    { "Corona Extra", "https://la-bax.ro/wp-content/uploads/2024/10/Bere71.png", 7, 4.5m },
                    { "BrewDog Punk IPA", "https://mcgrocer.com/cdn/shop/files/brewdog-punk-ipa-post-modern-classic-40872180547822_grande.jpg?v=1737433484", 8, 5.6m },
                    { "Chimay Rouge", "https://www.belgasorozo.com/wp-content/uploads/Chimay-Rouge.jpg", 9, 7.0m },
                    { "Trappistes Rochefort 8", "https://belgianmart.com/cdn/shop/products/r8.jpg?v=1538785647", 10, 9.2m },
                    { "BrewDog Elvis Juice", "https://brewdog.com/cdn/shop/files/pdp-elvis-juice-beer-330ml-can-brewdog.jpg?v=1723310594", 8, 6.5m },
                    { "Heineken Silver", "https://nitelashop.ro/media/cache/700x700xf/media/catalog/product/h/e/heineken-silver-bere-0_70296465f7a6d110f.jpeg", 4, 4.0m },
                    { "Guinness Foreign Extra Stout", "https://bellbeverage.com/wp-content/uploads/2020/02/Screen-Shot-2020-05-21-at-4.47.42-PM.png", 5, 7.5m }
                });

            // Insert DrinkCategories
            migrationBuilder.InsertData(
                table: "DrinkCategories",
                columns: new[] { "DrinkId", "CategoryId" },
                values: new object[,]
                {
                    { 1, 2 }, { 1, 1 }, { 2, 1 }, { 2, 2 }, { 3, 9 }, { 4, 1 }, { 5, 3 }, { 6, 1 },
                    { 7, 1 }, { 8, 2 }, { 9, 9 }, { 10, 3 }, { 11, 2 }, { 12, 1 }, { 13, 3 }
                });

            // Insert Users
            migrationBuilder.InsertData(
                table: "Users",
                column: "UserId",
                values: new object[] { 1, 2, 3, 4, 5 });

            // Insert Votes
            migrationBuilder.InsertData(
                table: "Votes",
                columns: new[] { "UserId", "DrinkId", "VoteTime" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 3, 29, 12, 0, 0) },
                    { 2, 2, new DateTime(2025, 3, 29, 14, 0, 0) },
                    { 1, 5, new DateTime(2025, 3, 30, 16, 0, 0) },
                    { 3, 8, new DateTime(2025, 3, 30, 10, 0, 0) },
                    { 4, 5, new DateTime(2025, 3, 30, 12, 0, 0) },
                    { 2, 8, new DateTime(2025, 3, 31, 9, 0, 0) },
                    { 5, 9, new DateTime(2025, 3, 30, 14, 0, 0) }
                });

            // Insert DrinkOfTheDay
            migrationBuilder.InsertData(
                table: "DrinkOfTheDays",
                columns: new[] { "DrinkId", "DrinkTime" },
                values: new object[] { 1, new DateTime(2025, 4, 3, 8, 0, 0) });

            // Insert UserDrinks
            migrationBuilder.InsertData(
                table: "UserDrinks",
                columns: new[] { "UserId", "DrinkId" },
                values: new object[,]
                {
                    { 1, 1 }, { 1, 2 }, { 1, 3 }, { 1, 4 }, { 1, 5 }, { 1, 6 }, { 1, 7 }, { 1, 8 }, { 1, 9 },
                    { 2, 2 }, { 2, 1 },
                    { 3, 5 }, { 3, 8 },
                    { 4, 9 },
                    { 5, 5 }, { 5, 1 }
                });

            // Insert Ratings
            migrationBuilder.InsertData(
                table: "Ratings",
                columns: new[] { "DrinkId", "UserId", "RatingValue", "RatingDate", "IsActive" },
                values: new object[,]
                {
                    { 1, 1, 4.0, new DateTime(2025, 4, 1, 10, 30, 0), (byte)1 },
                    { 2, 2, 3.0, new DateTime(2025, 4, 2, 12, 0, 0), (byte)1 },
                    { 3, 3, 5.0, new DateTime(2025, 4, 3, 14, 15, 0), (byte)1 },
                    { 4, 4, 2.0, new DateTime(2025, 4, 4, 16, 45, 0), (byte)1 },
                    { 5, 5, 4.5, new DateTime(2025, 4, 5, 18, 20, 0), (byte)1 }
                });

            // Insert Reviews
            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "RatingId", "UserId", "Content", "CreationDate", "IsActive" },
                values: new object[,]
                {
                    { 1, 1, "Great product! Highly recommend.", new DateTime(2025, 4, 1, 11, 0, 0), (byte)1 },
                    { 2, 2, "It was okay, nothing special.", new DateTime(2025, 4, 2, 13, 0, 0), (byte)1 },
                    { 3, 3, "Absolutely loved it!", new DateTime(2025, 4, 3, 15, 0, 0), (byte)1 },
                    { 4, 4, "Not worth the money.", new DateTime(2025, 4, 4, 17, 0, 0), (byte)1 },
                    { 5, 5, "Very good quality for the price.", new DateTime(2025, 4, 5, 19, 0, 0), (byte)1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Delete in reverse order to handle foreign key constraints
            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValues: new object[] { 1, 2, 3, 4, 5 });

            migrationBuilder.DeleteData(
                table: "Ratings",
                keyColumn: "RatingId",
                keyValues: new object[] { 1, 2, 3, 4, 5 });

            migrationBuilder.DeleteData(
                table: "UserDrinks",
                keyColumns: new[] { "UserId", "DrinkId" },
                keyValues: new object[,]
                {
                    { 1, 1 }, { 1, 2 }, { 1, 3 }, { 1, 4 }, { 1, 5 }, { 1, 6 }, { 1, 7 }, { 1, 8 }, { 1, 9 },
                    { 2, 2 }, { 2, 1 },
                    { 3, 5 }, { 3, 8 },
                    { 4, 9 },
                    { 5, 5 }, { 5, 1 }
                });

            migrationBuilder.DeleteData(
                table: "DrinkOfTheDays",
                keyColumn: "DrinkId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Votes",
                keyColumns: new[] { "UserId", "DrinkId", "VoteTime" },
                keyValues: new object[,]
                {
                    { 1, 1, new DateTime(2025, 3, 29, 12, 0, 0) },
                    { 2, 2, new DateTime(2025, 3, 29, 14, 0, 0) },
                    { 1, 5, new DateTime(2025, 3, 30, 16, 0, 0) },
                    { 3, 8, new DateTime(2025, 3, 30, 10, 0, 0) },
                    { 4, 5, new DateTime(2025, 3, 30, 12, 0, 0) },
                    { 2, 8, new DateTime(2025, 3, 31, 9, 0, 0) },
                    { 5, 9, new DateTime(2025, 3, 30, 14, 0, 0) }
                });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValues: new object[] { 1, 2, 3, 4, 5 });

            migrationBuilder.DeleteData(
                table: "DrinkCategories",
                keyColumns: new[] { "DrinkId", "CategoryId" },
                keyValues: new object[,]
                {
                    { 1, 2 }, { 1, 1 }, { 2, 1 }, { 2, 2 }, { 3, 9 }, { 4, 1 }, { 5, 3 }, { 6, 1 },
                    { 7, 1 }, { 8, 2 }, { 9, 9 }, { 10, 3 }, { 11, 2 }, { 12, 1 }, { 13, 3 }
                });

            migrationBuilder.DeleteData(
                table: "Drinks",
                keyColumn: "DrinkId",
                keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 });

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 });

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "BrandId",
                keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
        }
    }
}
