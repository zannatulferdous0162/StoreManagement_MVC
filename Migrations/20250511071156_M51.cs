using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreManagement_Project.Migrations
{
    /// <inheritdoc />
    public partial class M51 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequisitionItems_RequisitionItems_RequisitionItemId",
                table: "PurchaseRequisitionItems");

            migrationBuilder.AlterColumn<int>(
                name: "RequisitionItemId",
                table: "PurchaseRequisitionItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequisitionItems_RequisitionItems_RequisitionItemId",
                table: "PurchaseRequisitionItems",
                column: "RequisitionItemId",
                principalTable: "RequisitionItems",
                principalColumn: "RequisitionItemId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequisitionItems_RequisitionItems_RequisitionItemId",
                table: "PurchaseRequisitionItems");

            migrationBuilder.AlterColumn<int>(
                name: "RequisitionItemId",
                table: "PurchaseRequisitionItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequisitionItems_RequisitionItems_RequisitionItemId",
                table: "PurchaseRequisitionItems",
                column: "RequisitionItemId",
                principalTable: "RequisitionItems",
                principalColumn: "RequisitionItemId");
        }
    }
}
