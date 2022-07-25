using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class VendorInvoiceNavProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoiceItem_VendorInvoice_InvoiceId",
                schema: "dbo",
                table: "VendorInvoiceItem");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoicePayment_VendorInvoice_VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoicePayment");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoiceTax_VendorInvoice_VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoiceTax");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VendorInvoicePaymentMethods",
                table: "VendorInvoicePaymentMethods");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                schema: "dbo",
                table: "VendorInvoicePayment");

            migrationBuilder.DropColumn(
                name: "MfrId",
                schema: "dbo",
                table: "VendorInvoiceItem");

            migrationBuilder.DropColumn(
                name: "PaymentName",
                table: "VendorInvoicePaymentMethods");

            migrationBuilder.DropColumn(
                name: "TrackingState",
                table: "VendorInvoicePaymentMethods");

            migrationBuilder.RenameTable(
                name: "VendorInvoicePaymentMethods",
                newName: "VendorInvoicePaymentMethod",
                newSchema: "dbo");

            migrationBuilder.RenameColumn(
                name: "InvoiceId",
                schema: "dbo",
                table: "VendorInvoiceItem",
                newName: "VendorInvoiceId");

            migrationBuilder.RenameIndex(
                name: "IX_VendorInvoiceItem_InvoiceId",
                schema: "dbo",
                table: "VendorInvoiceItem",
                newName: "IX_VendorInvoiceItem_VendorInvoiceId");

            migrationBuilder.AlterColumn<long>(
                name: "VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoiceTax",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SalesTaxId",
                schema: "dbo",
                table: "VendorInvoiceTax",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxName",
                schema: "dbo",
                table: "VendorInvoiceTax",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoicePayment",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MyPropertyId",
                schema: "dbo",
                table: "VendorInvoicePayment",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ManufacturerId",
                schema: "dbo",
                table: "VendorInvoiceItem",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SaleCode",
                schema: "dbo",
                table: "VendorInvoiceItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VendorInvoicePaymentMethod",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_VendorInvoiceTax_SalesTaxId",
                schema: "dbo",
                table: "VendorInvoiceTax",
                column: "SalesTaxId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorInvoicePayment_MyPropertyId",
                schema: "dbo",
                table: "VendorInvoicePayment",
                column: "MyPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorInvoiceItem_ManufacturerId",
                schema: "dbo",
                table: "VendorInvoiceItem",
                column: "ManufacturerId");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoiceItem_Manufacturer_ManufacturerId",
                schema: "dbo",
                table: "VendorInvoiceItem",
                column: "ManufacturerId",
                principalSchema: "dbo",
                principalTable: "Manufacturer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoiceItem_VendorInvoice_VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoiceItem",
                column: "VendorInvoiceId",
                principalSchema: "dbo",
                principalTable: "VendorInvoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoicePayment_VendorInvoice_VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoicePayment",
                column: "VendorInvoiceId",
                principalSchema: "dbo",
                principalTable: "VendorInvoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoicePayment_VendorInvoicePaymentMethod_MyPropertyId",
                schema: "dbo",
                table: "VendorInvoicePayment",
                column: "MyPropertyId",
                principalSchema: "dbo",
                principalTable: "VendorInvoicePaymentMethod",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoiceTax_SalesTax_SalesTaxId",
                schema: "dbo",
                table: "VendorInvoiceTax",
                column: "SalesTaxId",
                principalSchema: "dbo",
                principalTable: "SalesTax",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoiceTax_VendorInvoice_VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoiceTax",
                column: "VendorInvoiceId",
                principalSchema: "dbo",
                principalTable: "VendorInvoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoiceItem_Manufacturer_ManufacturerId",
                schema: "dbo",
                table: "VendorInvoiceItem");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoiceItem_VendorInvoice_VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoiceItem");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoicePayment_VendorInvoice_VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoicePayment");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoicePayment_VendorInvoicePaymentMethod_MyPropertyId",
                schema: "dbo",
                table: "VendorInvoicePayment");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoiceTax_SalesTax_SalesTaxId",
                schema: "dbo",
                table: "VendorInvoiceTax");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoiceTax_VendorInvoice_VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoiceTax");

            migrationBuilder.DropIndex(
                name: "IX_VendorInvoiceTax_SalesTaxId",
                schema: "dbo",
                table: "VendorInvoiceTax");

            migrationBuilder.DropIndex(
                name: "IX_VendorInvoicePayment_MyPropertyId",
                schema: "dbo",
                table: "VendorInvoicePayment");

            migrationBuilder.DropIndex(
                name: "IX_VendorInvoiceItem_ManufacturerId",
                schema: "dbo",
                table: "VendorInvoiceItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VendorInvoicePaymentMethod",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod");

            migrationBuilder.DropColumn(
                name: "SalesTaxId",
                schema: "dbo",
                table: "VendorInvoiceTax");

            migrationBuilder.DropColumn(
                name: "TaxName",
                schema: "dbo",
                table: "VendorInvoiceTax");

            migrationBuilder.DropColumn(
                name: "MyPropertyId",
                schema: "dbo",
                table: "VendorInvoicePayment");

            migrationBuilder.DropColumn(
                name: "ManufacturerId",
                schema: "dbo",
                table: "VendorInvoiceItem");

            migrationBuilder.DropColumn(
                name: "SaleCode",
                schema: "dbo",
                table: "VendorInvoiceItem");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod");

            migrationBuilder.RenameTable(
                name: "VendorInvoicePaymentMethod",
                schema: "dbo",
                newName: "VendorInvoicePaymentMethods");

            migrationBuilder.RenameColumn(
                name: "VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoiceItem",
                newName: "InvoiceId");

            migrationBuilder.RenameIndex(
                name: "IX_VendorInvoiceItem_VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoiceItem",
                newName: "IX_VendorInvoiceItem_InvoiceId");

            migrationBuilder.AlterColumn<long>(
                name: "VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoiceTax",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoicePayment",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "InvoiceId",
                schema: "dbo",
                table: "VendorInvoicePayment",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "MfrId",
                schema: "dbo",
                table: "VendorInvoiceItem",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentName",
                table: "VendorInvoicePaymentMethods",
                type: "nvarchar(max)",
                nullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoiceItem_VendorInvoice_InvoiceId",
                schema: "dbo",
                table: "VendorInvoiceItem",
                column: "InvoiceId",
                principalSchema: "dbo",
                principalTable: "VendorInvoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoicePayment_VendorInvoice_VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoicePayment",
                column: "VendorInvoiceId",
                principalSchema: "dbo",
                principalTable: "VendorInvoice",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoiceTax_VendorInvoice_VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoiceTax",
                column: "VendorInvoiceId",
                principalSchema: "dbo",
                principalTable: "VendorInvoice",
                principalColumn: "Id");
        }
    }
}
