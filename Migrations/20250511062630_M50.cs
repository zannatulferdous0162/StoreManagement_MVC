using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreManagement_Project.Migrations
{
    /// <inheritdoc />
    public partial class M50 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequisitionItemId",
                table: "PurchaseRequisitionItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequisitionItems_RequisitionItemId",
                table: "PurchaseRequisitionItems",
                column: "RequisitionItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequisitionItems_RequisitionItems_RequisitionItemId",
                table: "PurchaseRequisitionItems",
                column: "RequisitionItemId",
                principalTable: "RequisitionItems",
                principalColumn: "RequisitionItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequisitionItems_RequisitionItems_RequisitionItemId",
                table: "PurchaseRequisitionItems");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequisitionItems_RequisitionItemId",
                table: "PurchaseRequisitionItems");

            migrationBuilder.DropColumn(
                name: "RequisitionItemId",
                table: "PurchaseRequisitionItems");
        }
    }
}
