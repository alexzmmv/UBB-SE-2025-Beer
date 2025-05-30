using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Migrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrinkModificationRequests_Drinks_NewDrinkId",
                table: "DrinkModificationRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_DrinkModificationRequests_Drinks_OldDrinkId",
                table: "DrinkModificationRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_DrinkModificationRequests_Drinks_NewDrinkId",
                table: "DrinkModificationRequests",
                column: "NewDrinkId",
                principalTable: "Drinks",
                principalColumn: "DrinkId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DrinkModificationRequests_Drinks_OldDrinkId",
                table: "DrinkModificationRequests",
                column: "OldDrinkId",
                principalTable: "Drinks",
                principalColumn: "DrinkId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrinkModificationRequests_Drinks_NewDrinkId",
                table: "DrinkModificationRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_DrinkModificationRequests_Drinks_OldDrinkId",
                table: "DrinkModificationRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_DrinkModificationRequests_Drinks_NewDrinkId",
                table: "DrinkModificationRequests",
                column: "NewDrinkId",
                principalTable: "Drinks",
                principalColumn: "DrinkId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DrinkModificationRequests_Drinks_OldDrinkId",
                table: "DrinkModificationRequests",
                column: "OldDrinkId",
                principalTable: "Drinks",
                principalColumn: "DrinkId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
