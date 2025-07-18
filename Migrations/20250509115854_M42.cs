using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreManagement_Project.Migrations
{
    /// <inheritdoc />
    public partial class M42 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GRNItems_LocationComponents_LocationComponentId",
                table: "GRNItems");

            migrationBuilder.DropForeignKey(
                name: "FK_LocationComponents_Aisles_AisleId",
                table: "LocationComponents");

            migrationBuilder.DropForeignKey(
                name: "FK_LocationComponents_Bins_BinId",
                table: "LocationComponents");

            migrationBuilder.DropForeignKey(
                name: "FK_LocationComponents_Racks_RackId",
                table: "LocationComponents");

            migrationBuilder.DropForeignKey(
                name: "FK_LocationComponents_Warehouses_WarehouseId",
                table: "LocationComponents");

            migrationBuilder.DropForeignKey(
                name: "FK_LocationComponents_Zones_ZoneId",
                table: "LocationComponents");

            migrationBuilder.DropForeignKey(
                name: "FK_LocationComponents_shelves_ShelfId",
                table: "LocationComponents");

            migrationBuilder.AddColumn<int>(
                name: "LocationComponentId1",
                table: "GRNItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentId);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseRequisitions",
                columns: table => new
                {
                    PurchaseRequisitionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseRequisitionNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseRequisitions", x => x.PurchaseRequisitionId);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Designation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    EmployeeEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeePhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseRequisitionItems",
                columns: table => new
                {
                    PurchaseRequisitionItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseRequisitionId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseRequisitionItems", x => x.PurchaseRequisitionItemId);
                    table.ForeignKey(
                        name: "FK_PurchaseRequisitionItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseRequisitionItems_PurchaseRequisitions_PurchaseRequisitionId",
                        column: x => x.PurchaseRequisitionId,
                        principalTable: "PurchaseRequisitions",
                        principalColumn: "PurchaseRequisitionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Requisitions",
                columns: table => new
                {
                    RequisitionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequisitionNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RequisitionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requisitions", x => x.RequisitionId);
                    table.ForeignKey(
                        name: "FK_Requisitions_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequisitionIssues",
                columns: table => new
                {
                    RequisitionIssueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IssueNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequisitionId = table.Column<int>(type: "int", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequisitionIssues", x => x.RequisitionIssueId);
                    table.ForeignKey(
                        name: "FK_RequisitionIssues_Requisitions_RequisitionId",
                        column: x => x.RequisitionId,
                        principalTable: "Requisitions",
                        principalColumn: "RequisitionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequisitionItems",
                columns: table => new
                {
                    RequisitionItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequisitionId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    QuantityRequested = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuantityIssued = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsFullyIssued = table.Column<bool>(type: "bit", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequisitionItems", x => x.RequisitionItemId);
                    table.ForeignKey(
                        name: "FK_RequisitionItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequisitionItems_Requisitions_RequisitionId",
                        column: x => x.RequisitionId,
                        principalTable: "Requisitions",
                        principalColumn: "RequisitionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequisitionIssueItems",
                columns: table => new
                {
                    RequisitionIssueItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequisitionIssueId = table.Column<int>(type: "int", nullable: false),
                    RequisitionItemId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    QuantityIssued = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: true),
                    LocationComponentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequisitionIssueItems", x => x.RequisitionIssueItemId);
                    table.ForeignKey(
                        name: "FK_RequisitionIssueItems_RequisitionIssues_RequisitionIssueId",
                        column: x => x.RequisitionIssueId,
                        principalTable: "RequisitionIssues",
                        principalColumn: "RequisitionIssueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GRNItems_LocationComponentId1",
                table: "GRNItems",
                column: "LocationComponentId1");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequisitionItems_ItemId",
                table: "PurchaseRequisitionItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequisitionItems_PurchaseRequisitionId",
                table: "PurchaseRequisitionItems",
                column: "PurchaseRequisitionId");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionIssueItems_RequisitionIssueId",
                table: "RequisitionIssueItems",
                column: "RequisitionIssueId");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionIssues_RequisitionId",
                table: "RequisitionIssues",
                column: "RequisitionId");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionItems_ItemId",
                table: "RequisitionItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionItems_RequisitionId",
                table: "RequisitionItems",
                column: "RequisitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Requisitions_EmployeeId",
                table: "Requisitions",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_GRNItems_LocationComponents_LocationComponentId",
                table: "GRNItems",
                column: "LocationComponentId",
                principalTable: "LocationComponents",
                principalColumn: "LocationComponentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GRNItems_LocationComponents_LocationComponentId1",
                table: "GRNItems",
                column: "LocationComponentId1",
                principalTable: "LocationComponents",
                principalColumn: "LocationComponentId");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationComponents_Aisles_AisleId",
                table: "LocationComponents",
                column: "AisleId",
                principalTable: "Aisles",
                principalColumn: "AisleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LocationComponents_Bins_BinId",
                table: "LocationComponents",
                column: "BinId",
                principalTable: "Bins",
                principalColumn: "BinId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LocationComponents_Racks_RackId",
                table: "LocationComponents",
                column: "RackId",
                principalTable: "Racks",
                principalColumn: "RackId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LocationComponents_Warehouses_WarehouseId",
                table: "LocationComponents",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LocationComponents_Zones_ZoneId",
                table: "LocationComponents",
                column: "ZoneId",
                principalTable: "Zones",
                principalColumn: "ZoneId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LocationComponents_shelves_ShelfId",
                table: "LocationComponents",
                column: "ShelfId",
                principalTable: "shelves",
                principalColumn: "ShelfId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GRNItems_LocationComponents_LocationComponentId",
                table: "GRNItems");

            migrationBuilder.DropForeignKey(
                name: "FK_GRNItems_LocationComponents_LocationComponentId1",
                table: "GRNItems");

            migrationBuilder.DropForeignKey(
                name: "FK_LocationComponents_Aisles_AisleId",
                table: "LocationComponents");

            migrationBuilder.DropForeignKey(
                name: "FK_LocationComponents_Bins_BinId",
                table: "LocationComponents");

            migrationBuilder.DropForeignKey(
                name: "FK_LocationComponents_Racks_RackId",
                table: "LocationComponents");

            migrationBuilder.DropForeignKey(
                name: "FK_LocationComponents_Warehouses_WarehouseId",
                table: "LocationComponents");

            migrationBuilder.DropForeignKey(
                name: "FK_LocationComponents_Zones_ZoneId",
                table: "LocationComponents");

            migrationBuilder.DropForeignKey(
                name: "FK_LocationComponents_shelves_ShelfId",
                table: "LocationComponents");

            migrationBuilder.DropTable(
                name: "PurchaseRequisitionItems");

            migrationBuilder.DropTable(
                name: "RequisitionIssueItems");

            migrationBuilder.DropTable(
                name: "RequisitionItems");

            migrationBuilder.DropTable(
                name: "PurchaseRequisitions");

            migrationBuilder.DropTable(
                name: "RequisitionIssues");

            migrationBuilder.DropTable(
                name: "Requisitions");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_GRNItems_LocationComponentId1",
                table: "GRNItems");

            migrationBuilder.DropColumn(
                name: "LocationComponentId1",
                table: "GRNItems");

            migrationBuilder.AddForeignKey(
                name: "FK_GRNItems_LocationComponents_LocationComponentId",
                table: "GRNItems",
                column: "LocationComponentId",
                principalTable: "LocationComponents",
                principalColumn: "LocationComponentId");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationComponents_Aisles_AisleId",
                table: "LocationComponents",
                column: "AisleId",
                principalTable: "Aisles",
                principalColumn: "AisleId");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationComponents_Bins_BinId",
                table: "LocationComponents",
                column: "BinId",
                principalTable: "Bins",
                principalColumn: "BinId");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationComponents_Racks_RackId",
                table: "LocationComponents",
                column: "RackId",
                principalTable: "Racks",
                principalColumn: "RackId");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationComponents_Warehouses_WarehouseId",
                table: "LocationComponents",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationComponents_Zones_ZoneId",
                table: "LocationComponents",
                column: "ZoneId",
                principalTable: "Zones",
                principalColumn: "ZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationComponents_shelves_ShelfId",
                table: "LocationComponents",
                column: "ShelfId",
                principalTable: "shelves",
                principalColumn: "ShelfId");
        }
    }
}
