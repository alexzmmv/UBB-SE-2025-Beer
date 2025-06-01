using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedOnDeleteCascade : Migration
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

                    migrationBuilder.Sql(@"
                ALTER TABLE [DrinkModificationRequests] 
                ADD CONSTRAINT [CK_DrinkModificationRequest_DifferentDrinks] 
                CHECK ([OldDrinkId] != [NewDrinkId] OR [OldDrinkId] IS NULL OR [NewDrinkId] IS NULL)
            ");

            // Step 3: Create foreign keys one at a time with individual transactions
            migrationBuilder.AddForeignKey(
name: "FK_DrinkModificationRequests_Drinks_OldDrinkId",
table: "DrinkModificationRequests",
column: "OldDrinkId",
principalTable: "Drinks",
principalColumn: "DrinkId",
onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_DrinkModificationRequests_Drinks_NewDrinkId",
                table: "DrinkModificationRequests",
                column: "NewDrinkId",
                principalTable: "Drinks",
                principalColumn: "DrinkId",
                onDelete: ReferentialAction.NoAction);

            // Create trigger to handle SET NULL behavior
            migrationBuilder.Sql(@"
        CREATE TRIGGER TR_Drinks_Delete_SetNull
        ON [Drinks]
        INSTEAD OF DELETE
        AS
        BEGIN
            -- Set foreign keys to null
            UPDATE [DrinkModificationRequests] 
            SET [OldDrinkId] = NULL 
            WHERE [OldDrinkId] IN (SELECT [DrinkId] FROM deleted);
            
            UPDATE [DrinkModificationRequests] 
            SET [NewDrinkId] = NULL 
            WHERE [NewDrinkId] IN (SELECT [DrinkId] FROM deleted);
            
            -- Delete the drinks
            DELETE FROM [Drinks] WHERE [DrinkId] IN (SELECT [DrinkId] FROM deleted);
        END
    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE [DrinkModificationRequests] 
                DROP CONSTRAINT [CK_DrinkModificationRequest_DifferentDrinks]
            ");

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
    }
}
