using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreManagement_Project.Migrations
{
    /// <inheritdoc />
    public partial class M54 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequisitionId",
                table: "PurchaseRequisitions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequisitions_RequisitionId",
                table: "PurchaseRequisitions",
                column: "RequisitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequisitions_Requisitions_RequisitionId",
                table: "PurchaseRequisitions",
                column: "RequisitionId",
                principalTable: "Requisitions",
                principalColumn: "RequisitionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequisitions_Requisitions_RequisitionId",
                table: "PurchaseRequisitions");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequisitions_RequisitionId",
                table: "PurchaseRequisitions");

            migrationBuilder.DropColumn(
                name: "RequisitionId",
                table: "PurchaseRequisitions");
        }
    }
}
