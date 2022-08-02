using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class VendorInvoicePaymentType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoicePayment_VendorInvoicePaymentMethod_PaymentMethodId",
                schema: "dbo",
                table: "VendorInvoicePayment");

            migrationBuilder.DropIndex(
                name: "IX_VendorInvoicePayment_PaymentMethodId",
                schema: "dbo",
                table: "VendorInvoicePayment");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                schema: "dbo",
                table: "VendorInvoicePayment");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOnAccountPaymentType",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReconciledByAnotherVendor",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "VendorId",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "VendorInvoicePaymentMethodId",
                schema: "dbo",
                table: "VendorInvoicePayment",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_VendorInvoicePaymentMethod_VendorId",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorInvoicePayment_VendorInvoicePaymentMethodId",
                schema: "dbo",
                table: "VendorInvoicePayment",
                column: "VendorInvoicePaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoicePayment_VendorInvoicePaymentMethod_VendorInvoicePaymentMethodId",
                schema: "dbo",
                table: "VendorInvoicePayment",
                column: "VendorInvoicePaymentMethodId",
                principalSchema: "dbo",
                principalTable: "VendorInvoicePaymentMethod",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoicePaymentMethod_Vendor_VendorId",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod",
                column: "VendorId",
                principalSchema: "dbo",
                principalTable: "Vendor",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoicePayment_VendorInvoicePaymentMethod_VendorInvoicePaymentMethodId",
                schema: "dbo",
                table: "VendorInvoicePayment");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoicePaymentMethod_Vendor_VendorId",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod");

            migrationBuilder.DropIndex(
                name: "IX_VendorInvoicePaymentMethod_VendorId",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod");

            migrationBuilder.DropIndex(
                name: "IX_VendorInvoicePayment_VendorInvoicePaymentMethodId",
                schema: "dbo",
                table: "VendorInvoicePayment");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod");

            migrationBuilder.DropColumn(
                name: "IsOnAccountPaymentType",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod");

            migrationBuilder.DropColumn(
                name: "IsReconciledByAnotherVendor",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod");

            migrationBuilder.DropColumn(
                name: "VendorId",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod");

            migrationBuilder.DropColumn(
                name: "VendorInvoicePaymentMethodId",
                schema: "dbo",
                table: "VendorInvoicePayment");

            migrationBuilder.AddColumn<long>(
                name: "PaymentMethodId",
                schema: "dbo",
                table: "VendorInvoicePayment",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VendorInvoicePayment_PaymentMethodId",
                schema: "dbo",
                table: "VendorInvoicePayment",
                column: "PaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoicePayment_VendorInvoicePaymentMethod_PaymentMethodId",
                schema: "dbo",
                table: "VendorInvoicePayment",
                column: "PaymentMethodId",
                principalSchema: "dbo",
                principalTable: "VendorInvoicePaymentMethod",
                principalColumn: "Id");
        }
    }
}
