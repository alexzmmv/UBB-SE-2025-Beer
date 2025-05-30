using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixedPendingDrinksTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrinkModificationRequests_Drinks_NewDrinkDrinkId",
                table: "DrinkModificationRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_DrinksRequestingApproval_Brands_BrandId",
                table: "DrinksRequestingApproval");

            migrationBuilder.AlterColumn<int>(
                name: "BrandId",
                table: "DrinksRequestingApproval",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<decimal>(
                name: "AlcoholContent",
                table: "DrinksRequestingApproval",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "DrinkURL",
                table: "DrinksRequestingApproval",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_DrinkModificationRequests_DrinksRequestingApproval_NewDrinkDrinkId",
                table: "DrinkModificationRequests",
                column: "NewDrinkDrinkId",
                principalTable: "DrinksRequestingApproval",
                principalColumn: "DrinkId");

            migrationBuilder.AddForeignKey(
                name: "FK_DrinksRequestingApproval_Brands_BrandId",
                table: "DrinksRequestingApproval",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "BrandId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrinkModificationRequests_DrinksRequestingApproval_NewDrinkDrinkId",
                table: "DrinkModificationRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_DrinksRequestingApproval_Brands_BrandId",
                table: "DrinksRequestingApproval");

            migrationBuilder.DropColumn(
                name: "AlcoholContent",
                table: "DrinksRequestingApproval");

            migrationBuilder.DropColumn(
                name: "DrinkURL",
                table: "DrinksRequestingApproval");

            migrationBuilder.AlterColumn<int>(
                name: "BrandId",
                table: "DrinksRequestingApproval",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DrinkModificationRequests_Drinks_NewDrinkDrinkId",
                table: "DrinkModificationRequests",
                column: "NewDrinkDrinkId",
                principalTable: "Drinks",
                principalColumn: "DrinkId");

            migrationBuilder.AddForeignKey(
                name: "FK_DrinksRequestingApproval_Brands_BrandId",
                table: "DrinksRequestingApproval",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "BrandId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
