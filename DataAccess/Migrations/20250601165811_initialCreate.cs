using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class initialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    BrandId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.BrandId);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "OffensiveWords",
                columns: table => new
                {
                    OffensiveWordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Word = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OffensiveWords", x => x.OffensiveWordId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleType = table.Column<int>(type: "int", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleType);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TwoFASecret = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfDeletedReviews = table.Column<int>(type: "int", nullable: false),
                    HasSubmittedAppeal = table.Column<bool>(type: "bit", nullable: false),
                    AssignedRole = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Drinks",
                columns: table => new
                {
                    DrinkId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DrinkName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DrinkURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrandId = table.Column<int>(type: "int", nullable: true),
                    AlcoholContent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsRequestingApproval = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drinks", x => x.DrinkId);
                    table.ForeignKey(
                        name: "FK_Drinks_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.SessionId);
                    table.ForeignKey(
                        name: "FK_Sessions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UpgradeRequests",
                columns: table => new
                {
                    UpgradeRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestingUserIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestingUserDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpgradeRequests", x => x.UpgradeRequestId);
                    table.ForeignKey(
                        name: "FK_UpgradeRequests_Users_RequestingUserIdentifier",
                        column: x => x.RequestingUserIdentifier,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DrinkCategories",
                columns: table => new
                {
                    DrinkId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrinkCategories", x => new { x.DrinkId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_DrinkCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DrinkCategories_Drinks_DrinkId",
                        column: x => x.DrinkId,
                        principalTable: "Drinks",
                        principalColumn: "DrinkId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DrinkModificationRequests",
                columns: table => new
                {
                    DrinkModificationRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModificationType = table.Column<int>(type: "int", nullable: false),
                    OldDrinkId = table.Column<int>(type: "int", nullable: true),
                    NewDrinkId = table.Column<int>(type: "int", nullable: true),
                    RequestingUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrinkModificationRequests", x => x.DrinkModificationRequestId);
                    table.ForeignKey(
                        name: "FK_DrinkModificationRequests_Drinks_NewDrinkId",
                        column: x => x.NewDrinkId,
                        principalTable: "Drinks",
                        principalColumn: "DrinkId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DrinkModificationRequests_Drinks_OldDrinkId",
                        column: x => x.OldDrinkId,
                        principalTable: "Drinks",
                        principalColumn: "DrinkId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DrinkModificationRequests_Users_RequestingUserId",
                        column: x => x.RequestingUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DrinkOfTheDays",
                columns: table => new
                {
                    DrinkId = table.Column<int>(type: "int", nullable: false),
                    DrinkTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrinkOfTheDays", x => x.DrinkId);
                    table.ForeignKey(
                        name: "FK_DrinkOfTheDays_Drinks_DrinkId",
                        column: x => x.DrinkId,
                        principalTable: "Drinks",
                        principalColumn: "DrinkId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DrinkId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<byte>(type: "tinyint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    NumberOfFlags = table.Column<int>(type: "int", nullable: false),
                    IsHidden = table.Column<bool>(type: "bit", nullable: false),
                    RatingValue = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_Reviews_Drinks_DrinkId",
                        column: x => x.DrinkId,
                        principalTable: "Drinks",
                        principalColumn: "DrinkId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDrinks",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrinkId = table.Column<int>(type: "int", nullable: false),
                    DrinkId1 = table.Column<int>(type: "int", nullable: true),
                    UserId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDrinks", x => new { x.UserId, x.DrinkId });
                    table.ForeignKey(
                        name: "FK_UserDrinks_Drinks_DrinkId",
                        column: x => x.DrinkId,
                        principalTable: "Drinks",
                        principalColumn: "DrinkId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDrinks_Drinks_DrinkId1",
                        column: x => x.DrinkId1,
                        principalTable: "Drinks",
                        principalColumn: "DrinkId");
                    table.ForeignKey(
                        name: "FK_UserDrinks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDrinks_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Votes",
                columns: table => new
                {
                    VoteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrinkId = table.Column<int>(type: "int", nullable: false),
                    VoteTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DrinkId1 = table.Column<int>(type: "int", nullable: true),
                    UserId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votes", x => x.VoteId);
                    table.ForeignKey(
                        name: "FK_Votes_Drinks_DrinkId",
                        column: x => x.DrinkId,
                        principalTable: "Drinks",
                        principalColumn: "DrinkId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Votes_Drinks_DrinkId1",
                        column: x => x.DrinkId1,
                        principalTable: "Drinks",
                        principalColumn: "DrinkId");
                    table.ForeignKey(
                        name: "FK_Votes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Votes_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

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
                table: "Roles",
                columns: new[] { "RoleType", "RoleName" },
                values: new object[,]
                {
                    { 0, "Banned" },
                    { 1, "User" },
                    { 2, "Admin" }
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

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CategoryName",
                table: "Categories",
                column: "CategoryName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DrinkCategories_CategoryId",
                table: "DrinkCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DrinkModificationRequests_NewDrinkId",
                table: "DrinkModificationRequests",
                column: "NewDrinkId");

            migrationBuilder.CreateIndex(
                name: "IX_DrinkModificationRequests_OldDrinkId",
                table: "DrinkModificationRequests",
                column: "OldDrinkId");

            migrationBuilder.CreateIndex(
                name: "IX_DrinkModificationRequests_RequestingUserId",
                table: "DrinkModificationRequests",
                column: "RequestingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Drinks_BrandId",
                table: "Drinks",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_DrinkId",
                table: "Reviews",
                column: "DrinkId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId_DrinkId",
                table: "Reviews",
                columns: new[] { "UserId", "DrinkId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_UserId",
                table: "Sessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UpgradeRequests_RequestingUserIdentifier",
                table: "UpgradeRequests",
                column: "RequestingUserIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_UserDrinks_DrinkId",
                table: "UserDrinks",
                column: "DrinkId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDrinks_DrinkId1",
                table: "UserDrinks",
                column: "DrinkId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserDrinks_UserId1",
                table: "UserDrinks",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_DrinkId",
                table: "Votes",
                column: "DrinkId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_DrinkId1",
                table: "Votes",
                column: "DrinkId1");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_UserId",
                table: "Votes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_UserId1",
                table: "Votes",
                column: "UserId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DrinkCategories");

            migrationBuilder.DropTable(
                name: "DrinkModificationRequests");

            migrationBuilder.DropTable(
                name: "DrinkOfTheDays");

            migrationBuilder.DropTable(
                name: "OffensiveWords");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "UpgradeRequests");

            migrationBuilder.DropTable(
                name: "UserDrinks");

            migrationBuilder.DropTable(
                name: "Votes");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Drinks");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Brands");
        }
    }
}
