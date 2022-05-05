using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class SplitSaleCodeShopSupplies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AlterColumn<double>(
                name: "LaborRate",
                schema: "dbo",
                table: "SaleCode",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "DesiredMargin",
                schema: "dbo",
                table: "SaleCode",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<bool>(
                name: "IncludeLabor",
                schema: "dbo",
                table: "SaleCode",
                type: "bit",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IncludeParts",
                schema: "dbo",
                table: "SaleCode",
                type: "bit",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "MaximumCharge",
                schema: "dbo",
                table: "SaleCode",
                type: "float",
                nullable: true,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MinimumCharge",
                schema: "dbo",
                table: "SaleCode",
                type: "float",
                nullable: true,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MinimumJobAmount",
                schema: "dbo",
                table: "SaleCode",
                type: "float",
                nullable: true,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Percentage",
                schema: "dbo",
                table: "SaleCode",
                type: "float",
                nullable: true,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncludeLabor",
                schema: "dbo",
                table: "SaleCode");

            migrationBuilder.DropColumn(
                name: "IncludeParts",
                schema: "dbo",
                table: "SaleCode");

            migrationBuilder.DropColumn(
                name: "MaximumCharge",
                schema: "dbo",
                table: "SaleCode");

            migrationBuilder.DropColumn(
                name: "MinimumCharge",
                schema: "dbo",
                table: "SaleCode");

            migrationBuilder.DropColumn(
                name: "MinimumJobAmount",
                schema: "dbo",
                table: "SaleCode");

            migrationBuilder.DropColumn(
                name: "Percentage",
                schema: "dbo",
                table: "SaleCode");

            migrationBuilder.AlterColumn<double>(
                name: "LaborRate",
                schema: "dbo",
                table: "SaleCode",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float",
                oldDefaultValue: 0.0);

            migrationBuilder.AlterColumn<double>(
                name: "DesiredMargin",
                schema: "dbo",
                table: "SaleCode",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float",
                oldDefaultValue: 0.0);

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
    }
}
