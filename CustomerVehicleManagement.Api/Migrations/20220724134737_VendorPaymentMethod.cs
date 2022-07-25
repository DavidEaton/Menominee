using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class VendorPaymentMethod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoicePayment_VendorInvoicePaymentMethod_MyPropertyId",
                schema: "dbo",
                table: "VendorInvoicePayment");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                schema: "dbo",
                table: "VendorInvoicePayment");

            migrationBuilder.RenameColumn(
                name: "MyPropertyId",
                schema: "dbo",
                table: "VendorInvoicePayment",
                newName: "PaymentMethodId");

            migrationBuilder.RenameIndex(
                name: "IX_VendorInvoicePayment_MyPropertyId",
                schema: "dbo",
                table: "VendorInvoicePayment",
                newName: "IX_VendorInvoicePayment_PaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoicePayment_VendorInvoicePaymentMethod_PaymentMethodId",
                schema: "dbo",
                table: "VendorInvoicePayment",
                column: "PaymentMethodId",
                principalSchema: "dbo",
                principalTable: "VendorInvoicePaymentMethod",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoicePayment_VendorInvoicePaymentMethod_PaymentMethodId",
                schema: "dbo",
                table: "VendorInvoicePayment");

            migrationBuilder.RenameColumn(
                name: "PaymentMethodId",
                schema: "dbo",
                table: "VendorInvoicePayment",
                newName: "MyPropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_VendorInvoicePayment_PaymentMethodId",
                schema: "dbo",
                table: "VendorInvoicePayment",
                newName: "IX_VendorInvoicePayment_MyPropertyId");

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethod",
                schema: "dbo",
                table: "VendorInvoicePayment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoicePayment_VendorInvoicePaymentMethod_MyPropertyId",
                schema: "dbo",
                table: "VendorInvoicePayment",
                column: "MyPropertyId",
                principalSchema: "dbo",
                principalTable: "VendorInvoicePaymentMethod",
                principalColumn: "Id");
        }
    }
}
