using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreManagement_Project.Migrations
{
    /// <inheritdoc />
    public partial class M31 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GRNs_LocationComponents_LocationComponentId",
                table: "GRNs");

            migrationBuilder.DropIndex(
                name: "IX_GRNs_LocationComponentId",
                table: "GRNs");

            migrationBuilder.DropColumn(
                name: "LocationComponentId",
                table: "GRNs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationComponentId",
                table: "GRNs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GRNs_LocationComponentId",
                table: "GRNs",
                column: "LocationComponentId");

            migrationBuilder.AddForeignKey(
                name: "FK_GRNs_LocationComponents_LocationComponentId",
                table: "GRNs",
                column: "LocationComponentId",
                principalTable: "LocationComponents",
                principalColumn: "LocationComponentId");
        }
    }
}
