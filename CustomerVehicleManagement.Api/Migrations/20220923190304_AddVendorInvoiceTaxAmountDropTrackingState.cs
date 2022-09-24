using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class AddVendorInvoiceTaxAmountDropTrackingState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrackingState",
                table: "SaleCodeShopSupplies");

            migrationBuilder.DropColumn(
                name: "TrackingState",
                table: "InventoryItemGiftCertificate");

            migrationBuilder.DropColumn(
                name: "TrackingState",
                table: "InventoryItemDonation");

            migrationBuilder.AddColumn<double>(
                name: "Amount",
                schema: "dbo",
                table: "VendorInvoiceTax",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                schema: "dbo",
                table: "VendorInvoiceTax");

            migrationBuilder.AddColumn<int>(
                name: "TrackingState",
                table: "SaleCodeShopSupplies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TrackingState",
                table: "InventoryItemGiftCertificate",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TrackingState",
                table: "InventoryItemDonation",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
