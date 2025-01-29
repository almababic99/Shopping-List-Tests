using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class New : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemShoppingList_ShoppingList_shoppingListsId",
                table: "ItemShoppingList");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingList_Shoppers_ShopperId",
                table: "ShoppingList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingList",
                table: "ShoppingList");

            migrationBuilder.RenameTable(
                name: "ShoppingList",
                newName: "shoppingLists");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingList_ShopperId",
                table: "shoppingLists",
                newName: "IX_shoppingLists_ShopperId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_shoppingLists",
                table: "shoppingLists",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemShoppingList_shoppingLists_shoppingListsId",
                table: "ItemShoppingList",
                column: "shoppingListsId",
                principalTable: "shoppingLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_shoppingLists_Shoppers_ShopperId",
                table: "shoppingLists",
                column: "ShopperId",
                principalTable: "Shoppers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemShoppingList_shoppingLists_shoppingListsId",
                table: "ItemShoppingList");

            migrationBuilder.DropForeignKey(
                name: "FK_shoppingLists_Shoppers_ShopperId",
                table: "shoppingLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_shoppingLists",
                table: "shoppingLists");

            migrationBuilder.RenameTable(
                name: "shoppingLists",
                newName: "ShoppingList");

            migrationBuilder.RenameIndex(
                name: "IX_shoppingLists_ShopperId",
                table: "ShoppingList",
                newName: "IX_ShoppingList_ShopperId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingList",
                table: "ShoppingList",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemShoppingList_ShoppingList_shoppingListsId",
                table: "ItemShoppingList",
                column: "shoppingListsId",
                principalTable: "ShoppingList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingList_Shoppers_ShopperId",
                table: "ShoppingList",
                column: "ShopperId",
                principalTable: "Shoppers",
                principalColumn: "Id");
        }
    }
}
