using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ModifyDrinkEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrinkModificationRequests_DrinksRequestingApproval_NewDrinkDrinkId",
                table: "DrinkModificationRequests");

            migrationBuilder.DropTable(
                name: "DrinksRequestingApproval");

            migrationBuilder.AddColumn<bool>(
                name: "IsRequestingApproval",
                table: "Drinks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_DrinkModificationRequests_Drinks_NewDrinkDrinkId",
                table: "DrinkModificationRequests",
                column: "NewDrinkDrinkId",
                principalTable: "Drinks",
                principalColumn: "DrinkId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrinkModificationRequests_Drinks_NewDrinkDrinkId",
                table: "DrinkModificationRequests");

            migrationBuilder.DropColumn(
                name: "IsRequestingApproval",
                table: "Drinks");

            migrationBuilder.CreateTable(
                name: "DrinksRequestingApproval",
                columns: table => new
                {
                    DrinkId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandId = table.Column<int>(type: "int", nullable: true),
                    AlcoholContent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DrinkName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DrinkURL = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrinksRequestingApproval", x => x.DrinkId);
                    table.ForeignKey(
                        name: "FK_DrinksRequestingApproval_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "BrandId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DrinksRequestingApproval_BrandId",
                table: "DrinksRequestingApproval",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_DrinkModificationRequests_DrinksRequestingApproval_NewDrinkDrinkId",
                table: "DrinkModificationRequests",
                column: "NewDrinkDrinkId",
                principalTable: "DrinksRequestingApproval",
                principalColumn: "DrinkId");
        }
    }
}
