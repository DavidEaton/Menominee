using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class SaleCodeAdditions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DesiredMargin",
                schema: "dbo",
                table: "SaleCode",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LaborRate",
                schema: "dbo",
                table: "SaleCode",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "ShopSuppliesIncludeLabor",
                schema: "dbo",
                table: "SaleCode",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShopSuppliesIncludeParts",
                schema: "dbo",
                table: "SaleCode",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "ShopSuppliesMaximumCharge",
                schema: "dbo",
                table: "SaleCode",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ShopSuppliesMinimumCharge",
                schema: "dbo",
                table: "SaleCode",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ShopSuppliesMinimumJobAmount",
                schema: "dbo",
                table: "SaleCode",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ShopSuppliesPercentage",
                schema: "dbo",
                table: "SaleCode",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DesiredMargin",
                schema: "dbo",
                table: "SaleCode");

            migrationBuilder.DropColumn(
                name: "LaborRate",
                schema: "dbo",
                table: "SaleCode");

            migrationBuilder.DropColumn(
                name: "ShopSuppliesIncludeLabor",
                schema: "dbo",
                table: "SaleCode");

            migrationBuilder.DropColumn(
                name: "ShopSuppliesIncludeParts",
                schema: "dbo",
                table: "SaleCode");

            migrationBuilder.DropColumn(
                name: "ShopSuppliesMaximumCharge",
                schema: "dbo",
                table: "SaleCode");

            migrationBuilder.DropColumn(
                name: "ShopSuppliesMinimumCharge",
                schema: "dbo",
                table: "SaleCode");

            migrationBuilder.DropColumn(
                name: "ShopSuppliesMinimumJobAmount",
                schema: "dbo",
                table: "SaleCode");

            migrationBuilder.DropColumn(
                name: "ShopSuppliesPercentage",
                schema: "dbo",
                table: "SaleCode");
        }
    }
}
