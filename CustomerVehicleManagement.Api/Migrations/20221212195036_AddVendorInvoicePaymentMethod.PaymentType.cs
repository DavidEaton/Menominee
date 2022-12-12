using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class AddVendorInvoicePaymentMethodPaymentType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOnAccountPaymentType",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod");

            migrationBuilder.AddColumn<int>(
                name: "PaymentType",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentType",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod");

            migrationBuilder.AddColumn<bool>(
                name: "IsOnAccountPaymentType",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
