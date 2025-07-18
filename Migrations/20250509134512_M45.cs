using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreManagement_Project.Migrations
{
    /// <inheritdoc />
    public partial class M45 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "RequisitionItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "RequisitionIssueItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "PurchaseRequisitionItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionItems_UnitId",
                table: "RequisitionItems",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionIssueItems_ItemId",
                table: "RequisitionIssueItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionIssueItems_UnitId",
                table: "RequisitionIssueItems",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequisitionItems_UnitId",
                table: "PurchaseRequisitionItems",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequisitionItems_Units_UnitId",
                table: "PurchaseRequisitionItems",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequisitionIssueItems_Items_ItemId",
                table: "RequisitionIssueItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequisitionIssueItems_Units_UnitId",
                table: "RequisitionIssueItems",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequisitionItems_Units_UnitId",
                table: "RequisitionItems",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequisitionItems_Units_UnitId",
                table: "PurchaseRequisitionItems");

            migrationBuilder.DropForeignKey(
                name: "FK_RequisitionIssueItems_Items_ItemId",
                table: "RequisitionIssueItems");

            migrationBuilder.DropForeignKey(
                name: "FK_RequisitionIssueItems_Units_UnitId",
                table: "RequisitionIssueItems");

            migrationBuilder.DropForeignKey(
                name: "FK_RequisitionItems_Units_UnitId",
                table: "RequisitionItems");

            migrationBuilder.DropIndex(
                name: "IX_RequisitionItems_UnitId",
                table: "RequisitionItems");

            migrationBuilder.DropIndex(
                name: "IX_RequisitionIssueItems_ItemId",
                table: "RequisitionIssueItems");

            migrationBuilder.DropIndex(
                name: "IX_RequisitionIssueItems_UnitId",
                table: "RequisitionIssueItems");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequisitionItems_UnitId",
                table: "PurchaseRequisitionItems");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "RequisitionItems");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "RequisitionIssueItems");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "PurchaseRequisitionItems");
        }
    }
}
