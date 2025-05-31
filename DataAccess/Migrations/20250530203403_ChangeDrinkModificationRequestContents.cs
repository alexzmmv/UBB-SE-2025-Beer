using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDrinkModificationRequestContents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrinkModificationRequests_Drinks_NewDrinkDrinkId",
                table: "DrinkModificationRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_DrinkModificationRequests_Drinks_OldDrinkDrinkId",
                table: "DrinkModificationRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_DrinkModificationRequests_Users_RequestingUserUserId",
                table: "DrinkModificationRequests");

            migrationBuilder.RenameColumn(
                name: "RequestingUserUserId",
                table: "DrinkModificationRequests",
                newName: "RequestingUserId");

            migrationBuilder.RenameColumn(
                name: "OldDrinkDrinkId",
                table: "DrinkModificationRequests",
                newName: "OldDrinkId");

            migrationBuilder.RenameColumn(
                name: "NewDrinkDrinkId",
                table: "DrinkModificationRequests",
                newName: "NewDrinkId");

            migrationBuilder.RenameIndex(
                name: "IX_DrinkModificationRequests_RequestingUserUserId",
                table: "DrinkModificationRequests",
                newName: "IX_DrinkModificationRequests_RequestingUserId");

            migrationBuilder.RenameIndex(
                name: "IX_DrinkModificationRequests_OldDrinkDrinkId",
                table: "DrinkModificationRequests",
                newName: "IX_DrinkModificationRequests_OldDrinkId");

            migrationBuilder.RenameIndex(
                name: "IX_DrinkModificationRequests_NewDrinkDrinkId",
                table: "DrinkModificationRequests",
                newName: "IX_DrinkModificationRequests_NewDrinkId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_DrinkModificationRequests_Users_RequestingUserId",
                table: "DrinkModificationRequests",
                column: "RequestingUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.DropForeignKey(
                name: "FK_DrinkModificationRequests_Users_RequestingUserId",
                table: "DrinkModificationRequests");

            migrationBuilder.RenameColumn(
                name: "RequestingUserId",
                table: "DrinkModificationRequests",
                newName: "RequestingUserUserId");

            migrationBuilder.RenameColumn(
                name: "OldDrinkId",
                table: "DrinkModificationRequests",
                newName: "OldDrinkDrinkId");

            migrationBuilder.RenameColumn(
                name: "NewDrinkId",
                table: "DrinkModificationRequests",
                newName: "NewDrinkDrinkId");

            migrationBuilder.RenameIndex(
                name: "IX_DrinkModificationRequests_RequestingUserId",
                table: "DrinkModificationRequests",
                newName: "IX_DrinkModificationRequests_RequestingUserUserId");

            migrationBuilder.RenameIndex(
                name: "IX_DrinkModificationRequests_OldDrinkId",
                table: "DrinkModificationRequests",
                newName: "IX_DrinkModificationRequests_OldDrinkDrinkId");

            migrationBuilder.RenameIndex(
                name: "IX_DrinkModificationRequests_NewDrinkId",
                table: "DrinkModificationRequests",
                newName: "IX_DrinkModificationRequests_NewDrinkDrinkId");

            migrationBuilder.AddForeignKey(
                name: "FK_DrinkModificationRequests_Drinks_NewDrinkDrinkId",
                table: "DrinkModificationRequests",
                column: "NewDrinkDrinkId",
                principalTable: "Drinks",
                principalColumn: "DrinkId");

            migrationBuilder.AddForeignKey(
                name: "FK_DrinkModificationRequests_Drinks_OldDrinkDrinkId",
                table: "DrinkModificationRequests",
                column: "OldDrinkDrinkId",
                principalTable: "Drinks",
                principalColumn: "DrinkId");

            migrationBuilder.AddForeignKey(
                name: "FK_DrinkModificationRequests_Users_RequestingUserUserId",
                table: "DrinkModificationRequests",
                column: "RequestingUserUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
