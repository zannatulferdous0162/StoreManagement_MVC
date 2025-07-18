using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreManagement_Project.Migrations
{
    /// <inheritdoc />
    public partial class M44 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RequisitionIssueItems_RequisitionItemId",
                table: "RequisitionIssueItems",
                column: "RequisitionItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequisitionIssueItems_RequisitionItems_RequisitionItemId",
                table: "RequisitionIssueItems",
                column: "RequisitionItemId",
                principalTable: "RequisitionItems",
                principalColumn: "RequisitionItemId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequisitionIssueItems_RequisitionItems_RequisitionItemId",
                table: "RequisitionIssueItems");

            migrationBuilder.DropIndex(
                name: "IX_RequisitionIssueItems_RequisitionItemId",
                table: "RequisitionIssueItems");
        }
    }
}
