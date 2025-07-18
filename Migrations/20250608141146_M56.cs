using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreManagement_Project.Migrations
{
    /// <inheritdoc />
    public partial class M56 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PurchaseRequisitionItemId",
                table: "PurchaseOrderItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItems_PurchaseRequisitionItemId",
                table: "PurchaseOrderItems",
                column: "PurchaseRequisitionItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderItems_PurchaseRequisitionItems_PurchaseRequisitionItemId",
                table: "PurchaseOrderItems",
                column: "PurchaseRequisitionItemId",
                principalTable: "PurchaseRequisitionItems",
                principalColumn: "PurchaseRequisitionItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderItems_PurchaseRequisitionItems_PurchaseRequisitionItemId",
                table: "PurchaseOrderItems");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderItems_PurchaseRequisitionItemId",
                table: "PurchaseOrderItems");

            migrationBuilder.DropColumn(
                name: "PurchaseRequisitionItemId",
                table: "PurchaseOrderItems");
        }
    }
}
