using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreManagement_Project.Migrations
{
    /// <inheritdoc />
    public partial class M32 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderItems_Units_UnitId",
                table: "PurchaseOrderItems");

            migrationBuilder.RenameColumn(
                name: "UnitId",
                table: "PurchaseOrderItems",
                newName: "UOM");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseOrderItems_UnitId",
                table: "PurchaseOrderItems",
                newName: "IX_PurchaseOrderItems_UOM");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderItems_Units_UOM",
                table: "PurchaseOrderItems",
                column: "UOM",
                principalTable: "Units",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderItems_Units_UOM",
                table: "PurchaseOrderItems");

            migrationBuilder.RenameColumn(
                name: "UOM",
                table: "PurchaseOrderItems",
                newName: "UnitId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseOrderItems_UOM",
                table: "PurchaseOrderItems",
                newName: "IX_PurchaseOrderItems_UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderItems_Units_UnitId",
                table: "PurchaseOrderItems",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
