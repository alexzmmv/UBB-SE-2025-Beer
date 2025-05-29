using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedDrinkModificationTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrinkCategories_Drinks_DrinkId",
                table: "DrinkCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_DrinkOfTheDays_Drinks_DrinkId",
                table: "DrinkOfTheDays");

            migrationBuilder.DropForeignKey(
                name: "FK_Drinks_Brands_BrandId",
                table: "Drinks");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Drinks_DrinkId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDrinks_Drinks_DrinkId",
                table: "UserDrinks");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDrinks_Drinks_DrinkId1",
                table: "UserDrinks");

            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Drinks_DrinkId",
                table: "Votes");

            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Drinks_DrinkId1",
                table: "Votes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Drinks",
                table: "Drinks");

            migrationBuilder.RenameTable(
                name: "Drinks",
                newName: "Drink");

            migrationBuilder.RenameIndex(
                name: "IX_Drinks_BrandId",
                table: "Drink",
                newName: "IX_Drink_BrandId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Drink",
                table: "Drink",
                column: "DrinkId");

            migrationBuilder.CreateTable(
                name: "DrinkModificationRequests",
                columns: table => new
                {
                    DrinkModificationRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModificationType = table.Column<int>(type: "int", nullable: false),
                    OldDrinkDrinkId = table.Column<int>(type: "int", nullable: true),
                    NewDrinkDrinkId = table.Column<int>(type: "int", nullable: true),
                    RequestingUserUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrinkModificationRequests", x => x.DrinkModificationRequestId);
                    table.ForeignKey(
                        name: "FK_DrinkModificationRequests_Drink_NewDrinkDrinkId",
                        column: x => x.NewDrinkDrinkId,
                        principalTable: "Drink",
                        principalColumn: "DrinkId");
                    table.ForeignKey(
                        name: "FK_DrinkModificationRequests_Drink_OldDrinkDrinkId",
                        column: x => x.OldDrinkDrinkId,
                        principalTable: "Drink",
                        principalColumn: "DrinkId");
                    table.ForeignKey(
                        name: "FK_DrinkModificationRequests_Users_RequestingUserUserId",
                        column: x => x.RequestingUserUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DrinkModificationRequests_NewDrinkDrinkId",
                table: "DrinkModificationRequests",
                column: "NewDrinkDrinkId");

            migrationBuilder.CreateIndex(
                name: "IX_DrinkModificationRequests_OldDrinkDrinkId",
                table: "DrinkModificationRequests",
                column: "OldDrinkDrinkId");

            migrationBuilder.CreateIndex(
                name: "IX_DrinkModificationRequests_RequestingUserUserId",
                table: "DrinkModificationRequests",
                column: "RequestingUserUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drink_Brands_BrandId",
                table: "Drink",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_DrinkCategories_Drink_DrinkId",
                table: "DrinkCategories",
                column: "DrinkId",
                principalTable: "Drink",
                principalColumn: "DrinkId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DrinkOfTheDays_Drink_DrinkId",
                table: "DrinkOfTheDays",
                column: "DrinkId",
                principalTable: "Drink",
                principalColumn: "DrinkId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Drink_DrinkId",
                table: "Ratings",
                column: "DrinkId",
                principalTable: "Drink",
                principalColumn: "DrinkId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDrinks_Drink_DrinkId",
                table: "UserDrinks",
                column: "DrinkId",
                principalTable: "Drink",
                principalColumn: "DrinkId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDrinks_Drink_DrinkId1",
                table: "UserDrinks",
                column: "DrinkId1",
                principalTable: "Drink",
                principalColumn: "DrinkId");

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Drink_DrinkId",
                table: "Votes",
                column: "DrinkId",
                principalTable: "Drink",
                principalColumn: "DrinkId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Drink_DrinkId1",
                table: "Votes",
                column: "DrinkId1",
                principalTable: "Drink",
                principalColumn: "DrinkId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drink_Brands_BrandId",
                table: "Drink");

            migrationBuilder.DropForeignKey(
                name: "FK_DrinkCategories_Drink_DrinkId",
                table: "DrinkCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_DrinkOfTheDays_Drink_DrinkId",
                table: "DrinkOfTheDays");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Drink_DrinkId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDrinks_Drink_DrinkId",
                table: "UserDrinks");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDrinks_Drink_DrinkId1",
                table: "UserDrinks");

            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Drink_DrinkId",
                table: "Votes");

            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Drink_DrinkId1",
                table: "Votes");

            migrationBuilder.DropTable(
                name: "DrinkModificationRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Drink",
                table: "Drink");

            migrationBuilder.RenameTable(
                name: "Drink",
                newName: "Drinks");

            migrationBuilder.RenameIndex(
                name: "IX_Drink_BrandId",
                table: "Drinks",
                newName: "IX_Drinks_BrandId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Drinks",
                table: "Drinks",
                column: "DrinkId");

            migrationBuilder.AddForeignKey(
                name: "FK_DrinkCategories_Drinks_DrinkId",
                table: "DrinkCategories",
                column: "DrinkId",
                principalTable: "Drinks",
                principalColumn: "DrinkId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DrinkOfTheDays_Drinks_DrinkId",
                table: "DrinkOfTheDays",
                column: "DrinkId",
                principalTable: "Drinks",
                principalColumn: "DrinkId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Drinks_Brands_BrandId",
                table: "Drinks",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Drinks_DrinkId",
                table: "Ratings",
                column: "DrinkId",
                principalTable: "Drinks",
                principalColumn: "DrinkId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDrinks_Drinks_DrinkId",
                table: "UserDrinks",
                column: "DrinkId",
                principalTable: "Drinks",
                principalColumn: "DrinkId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDrinks_Drinks_DrinkId1",
                table: "UserDrinks",
                column: "DrinkId1",
                principalTable: "Drinks",
                principalColumn: "DrinkId");

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Drinks_DrinkId",
                table: "Votes",
                column: "DrinkId",
                principalTable: "Drinks",
                principalColumn: "DrinkId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Drinks_DrinkId1",
                table: "Votes",
                column: "DrinkId1",
                principalTable: "Drinks",
                principalColumn: "DrinkId");
        }
    }
}
