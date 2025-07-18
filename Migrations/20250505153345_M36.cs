using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreManagement_Project.Migrations
{
    /// <inheritdoc />
    public partial class M36 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "GRNItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GRNItems_UnitId",
                table: "GRNItems",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_GRNItems_Units_UnitId",
                table: "GRNItems",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "UnitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GRNItems_Units_UnitId",
                table: "GRNItems");

            migrationBuilder.DropIndex(
                name: "IX_GRNItems_UnitId",
                table: "GRNItems");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "GRNItems");
        }
    }
}
