using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class RemoveTaxIdFromVendorInvoiceTax : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaxId",
                schema: "dbo",
                table: "VendorInvoiceTax");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TaxId",
                schema: "dbo",
                table: "VendorInvoiceTax",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
