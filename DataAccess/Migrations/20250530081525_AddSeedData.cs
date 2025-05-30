using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "BrandId", "BrandName" },
                values: new object[,]
                {
                    { 1, "Sunbrew Co." },
                    { 2, "Berry Spirits" },
                    { 3, "Mocktails Inc." }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName" },
                values: new object[,]
                {
                    { 1, "Ale" },
                    { 2, "Vodka" },
                    { 3, "Soft Drink" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "AssignedRole", "EmailAddress", "HasSubmittedAppeal", "NumberOfDeletedReviews", "PasswordHash", "TwoFASecret", "Username" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), 1, "john.doe@example.com", false, 0, "$2a$11$K2xKJ9.vF8wHqJ4bK9mZXeJ8vKlM3nO2pQ7rS9tU1vW3xY4zA5bC6", null, "john_doe" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 1, "jane.smith@example.com", false, 1, "$2a$11$L3yLK0.wG9xIrK5cL0nAYfK9wLmN4oP3qR8sT0uV2wX4yZ5aB6dD7", null, "jane_smith" }
                });

            migrationBuilder.InsertData(
                table: "Drinks",
                columns: new[] { "DrinkId", "AlcoholContent", "BrandId", "DrinkName", "DrinkURL" },
                values: new object[,]
                {
                    { 1, 5.2m, 1, "Golden Ale", "https://example.com/drinks/golden-ale.jpg" },
                    { 2, 37.5m, 2, "Cherry Vodka", "https://example.com/drinks/cherry-vodka.jpg" },
                    { 3, 0.0m, 3, "Ginger Beer", "https://example.com/drinks/ginger-beer.jpg" }
                });

            migrationBuilder.InsertData(
                table: "DrinkCategories",
                columns: new[] { "CategoryId", "DrinkId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 }
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "ReviewId", "Content", "CreatedDate", "DrinkId", "IsActive", "IsHidden", "NumberOfFlags", "RatingValue", "UserId" },
                values: new object[,]
                {
                    { 1, "Great taste, smooth and refreshing.", new DateTime(2024, 12, 20, 10, 30, 0, 0, DateTimeKind.Utc), 1, (byte)1, false, 0, 4.5, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 2, "Too bitter for my preference.", new DateTime(2024, 12, 20, 10, 30, 0, 0, DateTimeKind.Utc), 2, (byte)1, false, 1, 3.0, new Guid("22222222-2222-2222-2222-222222222222") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DrinkCategories",
                keyColumns: new[] { "CategoryId", "DrinkId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "DrinkCategories",
                keyColumns: new[] { "CategoryId", "DrinkId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "DrinkCategories",
                keyColumns: new[] { "CategoryId", "DrinkId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Drinks",
                keyColumn: "DrinkId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Drinks",
                keyColumn: "DrinkId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Drinks",
                keyColumn: "DrinkId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "BrandId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "BrandId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "BrandId",
                keyValue: 3);
        }
    }
}
