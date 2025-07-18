using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreManagement_Project.Migrations
{
    /// <inheritdoc />
    public partial class M47 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RequisitionIssueItems_LocationComponentId",
                table: "RequisitionIssueItems",
                column: "LocationComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionIssueItems_WarehouseId",
                table: "RequisitionIssueItems",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequisitionIssueItems_LocationComponents_LocationComponentId",
                table: "RequisitionIssueItems",
                column: "LocationComponentId",
                principalTable: "LocationComponents",
                principalColumn: "LocationComponentId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequisitionIssueItems_Warehouses_WarehouseId",
                table: "RequisitionIssueItems",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequisitionIssueItems_LocationComponents_LocationComponentId",
                table: "RequisitionIssueItems");

            migrationBuilder.DropForeignKey(
                name: "FK_RequisitionIssueItems_Warehouses_WarehouseId",
                table: "RequisitionIssueItems");

            migrationBuilder.DropIndex(
                name: "IX_RequisitionIssueItems_LocationComponentId",
                table: "RequisitionIssueItems");

            migrationBuilder.DropIndex(
                name: "IX_RequisitionIssueItems_WarehouseId",
                table: "RequisitionIssueItems");
        }
    }
}
