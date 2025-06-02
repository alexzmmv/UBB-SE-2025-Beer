using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedTableForDrinksPending : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                        name: "FK_DrinkModificationRequests_Drinks_NewDrinkDrinkId",
                        column: x => x.NewDrinkDrinkId,
                        principalTable: "Drinks",
                        principalColumn: "DrinkId");
                    table.ForeignKey(
                        name: "FK_DrinkModificationRequests_Drinks_OldDrinkDrinkId",
                        column: x => x.OldDrinkDrinkId,
                        principalTable: "Drinks",
                        principalColumn: "DrinkId");
                    table.ForeignKey(
                        name: "FK_DrinkModificationRequests_Users_RequestingUserUserId",
                        column: x => x.RequestingUserUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DrinksRequestingApproval",
                columns: table => new
                {
                    DrinkId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DrinkName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BrandId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrinksRequestingApproval", x => x.DrinkId);
                    table.ForeignKey(
                        name: "FK_DrinksRequestingApproval_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "BrandId",
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

            migrationBuilder.CreateIndex(
                name: "IX_DrinksRequestingApproval_BrandId",
                table: "DrinksRequestingApproval",
                column: "BrandId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DrinkModificationRequests");

            migrationBuilder.DropTable(
                name: "DrinksRequestingApproval");
        }
    }
}
