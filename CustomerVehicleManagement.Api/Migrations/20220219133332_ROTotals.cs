using Microsoft.EntityFrameworkCore.Migrations;

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class ROTotals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DiscountTotal",
                schema: "dbo",
                table: "RepairOrderServices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DiscountTotal",
                schema: "dbo",
                table: "RepairOrders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HazMatTotal",
                schema: "dbo",
                table: "RepairOrders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LaborTotal",
                schema: "dbo",
                table: "RepairOrders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PartsTotal",
                schema: "dbo",
                table: "RepairOrders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ShopSuppliesTotal",
                schema: "dbo",
                table: "RepairOrders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TaxTotal",
                schema: "dbo",
                table: "RepairOrders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountTotal",
                schema: "dbo",
                table: "RepairOrderServices");

            migrationBuilder.DropColumn(
                name: "DiscountTotal",
                schema: "dbo",
                table: "RepairOrders");

            migrationBuilder.DropColumn(
                name: "HazMatTotal",
                schema: "dbo",
                table: "RepairOrders");

            migrationBuilder.DropColumn(
                name: "LaborTotal",
                schema: "dbo",
                table: "RepairOrders");

            migrationBuilder.DropColumn(
                name: "PartsTotal",
                schema: "dbo",
                table: "RepairOrders");

            migrationBuilder.DropColumn(
                name: "ShopSuppliesTotal",
                schema: "dbo",
                table: "RepairOrders");

            migrationBuilder.DropColumn(
                name: "TaxTotal",
                schema: "dbo",
                table: "RepairOrders");
        }
    }
}
