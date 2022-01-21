using Microsoft.EntityFrameworkCore.Migrations;

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class IgnoreTrackingVendorInvoicePaymentMethod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VendorInvoicePaymentMethods",
                table: "VendorInvoicePaymentMethods");

            migrationBuilder.DropColumn(
                name: "TrackingState",
                table: "VendorInvoicePaymentMethods");

            migrationBuilder.RenameTable(
                name: "VendorInvoicePaymentMethods",
                newName: "VendorInvoicePaymentMethod",
                newSchema: "dbo");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentName",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VendorInvoicePaymentMethod",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VendorInvoicePaymentMethod",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod");

            migrationBuilder.RenameTable(
                name: "VendorInvoicePaymentMethod",
                schema: "dbo",
                newName: "VendorInvoicePaymentMethods");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentName",
                table: "VendorInvoicePaymentMethods",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "TrackingState",
                table: "VendorInvoicePaymentMethods",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VendorInvoicePaymentMethods",
                table: "VendorInvoicePaymentMethods",
                column: "Id");
        }
    }
}
