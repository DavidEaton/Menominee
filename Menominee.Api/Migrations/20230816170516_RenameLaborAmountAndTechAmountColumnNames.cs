using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Menominee.Api.Migrations
{
    public partial class RenameLaborAmountAndTechAmountColumnNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LaborPayType",
                schema: "dbo",
                table: "RepairOrderLineItem",
                newName: "LaborType");

            migrationBuilder.RenameColumn(
                name: "TechAmount_Amount",
                schema: "dbo",
                table: "InventoryItemLabor",
                newName: "TechPayAmount");

            migrationBuilder.RenameColumn(
                name: "LaborPayType",
                schema: "dbo",
                table: "InventoryItemLabor",
                newName: "LaborType");

            migrationBuilder.RenameColumn(
                name: "LaborPayAmount",
                schema: "dbo",
                table: "InventoryItemLabor",
                newName: "LaborAmount");

            migrationBuilder.RenameColumn(
                name: "TechAmount_Amount",
                schema: "dbo",
                table: "InventoryItemInspection",
                newName: "TechPayAmount");

            migrationBuilder.RenameColumn(
                name: "LaborPayType",
                schema: "dbo",
                table: "InventoryItemInspection",
                newName: "LaborType");

            migrationBuilder.RenameColumn(
                name: "LaborPayAmount",
                schema: "dbo",
                table: "InventoryItemInspection",
                newName: "LaborAmount");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LaborType",
                schema: "dbo",
                table: "RepairOrderLineItem",
                newName: "LaborPayType");

            migrationBuilder.RenameColumn(
                name: "TechPayAmount",
                schema: "dbo",
                table: "InventoryItemLabor",
                newName: "TechAmount_Amount");

            migrationBuilder.RenameColumn(
                name: "LaborType",
                schema: "dbo",
                table: "InventoryItemLabor",
                newName: "LaborPayType");

            migrationBuilder.RenameColumn(
                name: "LaborAmount",
                schema: "dbo",
                table: "InventoryItemLabor",
                newName: "LaborPayAmount");

            migrationBuilder.RenameColumn(
                name: "TechPayAmount",
                schema: "dbo",
                table: "InventoryItemInspection",
                newName: "TechAmount_Amount");

            migrationBuilder.RenameColumn(
                name: "LaborType",
                schema: "dbo",
                table: "InventoryItemInspection",
                newName: "LaborPayType");

            migrationBuilder.RenameColumn(
                name: "LaborAmount",
                schema: "dbo",
                table: "InventoryItemInspection",
                newName: "LaborPayAmount");
        }
    }
}
