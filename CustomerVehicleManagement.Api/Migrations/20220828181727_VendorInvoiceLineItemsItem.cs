using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class VendorInvoiceLineItemsItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoice_Vendor_VendorId",
                schema: "dbo",
                table: "VendorInvoice");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoiceItem_Manufacturer_ManufacturerId",
                schema: "dbo",
                table: "VendorInvoiceItem");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoiceItem_SaleCode_SaleCodeId",
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
                name: "FK_VendorInvoicePayment_VendorInvoicePaymentMethod_VendorInvoicePaymentMethodId",
                schema: "dbo",
                table: "VendorInvoicePayment");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoicePaymentMethod_Vendor_VendorId",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoiceTax_VendorInvoice_VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoiceTax");

            migrationBuilder.DropTable(
                name: "SalesTaxTaxableExciseFee",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_VendorInvoicePayment_VendorInvoicePaymentMethodId",
                schema: "dbo",
                table: "VendorInvoicePayment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VendorInvoiceItem",
                schema: "dbo",
                table: "VendorInvoiceItem");

            migrationBuilder.DropColumn(
                name: "Amount",
                schema: "dbo",
                table: "VendorInvoiceTax");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                schema: "dbo",
                table: "VendorInvoiceTax");

            migrationBuilder.DropColumn(
                name: "Order",
                schema: "dbo",
                table: "VendorInvoiceTax");

            migrationBuilder.DropColumn(
                name: "TaxName",
                schema: "dbo",
                table: "VendorInvoiceTax");

            migrationBuilder.DropColumn(
                name: "IsReconciledByAnotherVendor",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod");

            migrationBuilder.DropColumn(
                name: "VendorInvoicePaymentMethodId",
                schema: "dbo",
                table: "VendorInvoicePayment");

            migrationBuilder.DropColumn(
                name: "InvoiceNumber",
                schema: "dbo",
                table: "VendorInvoiceItem");

            migrationBuilder.RenameTable(
                name: "VendorInvoiceItem",
                schema: "dbo",
                newName: "VendorInvoiceLineItem",
                newSchema: "dbo");

            migrationBuilder.RenameColumn(
                name: "VendorId",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod",
                newName: "ReconcilingVendorId");

            migrationBuilder.RenameIndex(
                name: "IX_VendorInvoicePaymentMethod_VendorId",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod",
                newName: "IX_VendorInvoicePaymentMethod_ReconcilingVendorId");

            migrationBuilder.RenameColumn(
                name: "SaleCodeId",
                schema: "dbo",
                table: "VendorInvoiceLineItem",
                newName: "Item_SaleCodeId");

            migrationBuilder.RenameColumn(
                name: "PartNumber",
                schema: "dbo",
                table: "VendorInvoiceLineItem",
                newName: "ItemPartNumber");

            migrationBuilder.RenameColumn(
                name: "ManufacturerId",
                schema: "dbo",
                table: "VendorInvoiceLineItem",
                newName: "Item_ManufacturerId");

            migrationBuilder.RenameColumn(
                name: "Description",
                schema: "dbo",
                table: "VendorInvoiceLineItem",
                newName: "ItemDescription");

            migrationBuilder.RenameIndex(
                name: "IX_VendorInvoiceItem_VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoiceLineItem",
                newName: "IX_VendorInvoiceLineItem_VendorInvoiceId");

            migrationBuilder.RenameIndex(
                name: "IX_VendorInvoiceItem_SaleCodeId",
                schema: "dbo",
                table: "VendorInvoiceLineItem",
                newName: "IX_VendorInvoiceLineItem_Item_SaleCodeId");

            migrationBuilder.RenameIndex(
                name: "IX_VendorInvoiceItem_ManufacturerId",
                schema: "dbo",
                table: "VendorInvoiceLineItem",
                newName: "IX_VendorInvoiceLineItem_Item_ManufacturerId");

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
                name: "PaymentMethodId",
                schema: "dbo",
                table: "VendorInvoicePayment",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "VendorId",
                schema: "dbo",
                table: "VendorInvoice",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceNumber",
                schema: "dbo",
                table: "VendorInvoice",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SalesTaxId",
                schema: "dbo",
                table: "ExciseFee",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                schema: "dbo",
                table: "VendorInvoiceLineItem",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "ItemPartNumber",
                schema: "dbo",
                table: "VendorInvoiceLineItem",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "ItemDescription",
                schema: "dbo",
                table: "VendorInvoiceLineItem",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VendorInvoiceLineItem",
                schema: "dbo",
                table: "VendorInvoiceLineItem",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_VendorInvoicePayment_PaymentMethodId",
                schema: "dbo",
                table: "VendorInvoicePayment",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_ExciseFee_SalesTaxId",
                schema: "dbo",
                table: "ExciseFee",
                column: "SalesTaxId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExciseFee_SalesTax_SalesTaxId",
                schema: "dbo",
                table: "ExciseFee",
                column: "SalesTaxId",
                principalSchema: "dbo",
                principalTable: "SalesTax",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoice_Vendor_VendorId",
                schema: "dbo",
                table: "VendorInvoice",
                column: "VendorId",
                principalSchema: "dbo",
                principalTable: "Vendor",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoiceLineItem_Manufacturer_Item_ManufacturerId",
                schema: "dbo",
                table: "VendorInvoiceLineItem",
                column: "Item_ManufacturerId",
                principalSchema: "dbo",
                principalTable: "Manufacturer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoiceLineItem_SaleCode_Item_SaleCodeId",
                schema: "dbo",
                table: "VendorInvoiceLineItem",
                column: "Item_SaleCodeId",
                principalSchema: "dbo",
                principalTable: "SaleCode",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoiceLineItem_VendorInvoice_VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoiceLineItem",
                column: "VendorInvoiceId",
                principalSchema: "dbo",
                principalTable: "VendorInvoice",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoicePayment_VendorInvoice_VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoicePayment",
                column: "VendorInvoiceId",
                principalSchema: "dbo",
                principalTable: "VendorInvoice",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoicePayment_VendorInvoicePaymentMethod_PaymentMethodId",
                schema: "dbo",
                table: "VendorInvoicePayment",
                column: "PaymentMethodId",
                principalSchema: "dbo",
                principalTable: "VendorInvoicePaymentMethod",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoicePaymentMethod_Vendor_ReconcilingVendorId",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod",
                column: "ReconcilingVendorId",
                principalSchema: "dbo",
                principalTable: "Vendor",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExciseFee_SalesTax_SalesTaxId",
                schema: "dbo",
                table: "ExciseFee");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoice_Vendor_VendorId",
                schema: "dbo",
                table: "VendorInvoice");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoiceLineItem_Manufacturer_Item_ManufacturerId",
                schema: "dbo",
                table: "VendorInvoiceLineItem");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoiceLineItem_SaleCode_Item_SaleCodeId",
                schema: "dbo",
                table: "VendorInvoiceLineItem");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoiceLineItem_VendorInvoice_VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoiceLineItem");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoicePayment_VendorInvoice_VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoicePayment");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoicePayment_VendorInvoicePaymentMethod_PaymentMethodId",
                schema: "dbo",
                table: "VendorInvoicePayment");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoicePaymentMethod_Vendor_ReconcilingVendorId",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoiceTax_VendorInvoice_VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoiceTax");

            migrationBuilder.DropIndex(
                name: "IX_VendorInvoicePayment_PaymentMethodId",
                schema: "dbo",
                table: "VendorInvoicePayment");

            migrationBuilder.DropIndex(
                name: "IX_ExciseFee_SalesTaxId",
                schema: "dbo",
                table: "ExciseFee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VendorInvoiceLineItem",
                schema: "dbo",
                table: "VendorInvoiceLineItem");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                schema: "dbo",
                table: "VendorInvoicePayment");

            migrationBuilder.DropColumn(
                name: "SalesTaxId",
                schema: "dbo",
                table: "ExciseFee");

            migrationBuilder.RenameTable(
                name: "VendorInvoiceLineItem",
                schema: "dbo",
                newName: "VendorInvoiceItem",
                newSchema: "dbo");

            migrationBuilder.RenameColumn(
                name: "ReconcilingVendorId",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod",
                newName: "VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_VendorInvoicePaymentMethod_ReconcilingVendorId",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod",
                newName: "IX_VendorInvoicePaymentMethod_VendorId");

            migrationBuilder.RenameColumn(
                name: "Item_SaleCodeId",
                schema: "dbo",
                table: "VendorInvoiceItem",
                newName: "SaleCodeId");

            migrationBuilder.RenameColumn(
                name: "Item_ManufacturerId",
                schema: "dbo",
                table: "VendorInvoiceItem",
                newName: "ManufacturerId");

            migrationBuilder.RenameColumn(
                name: "ItemPartNumber",
                schema: "dbo",
                table: "VendorInvoiceItem",
                newName: "PartNumber");

            migrationBuilder.RenameColumn(
                name: "ItemDescription",
                schema: "dbo",
                table: "VendorInvoiceItem",
                newName: "Description");

            migrationBuilder.RenameIndex(
                name: "IX_VendorInvoiceLineItem_VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoiceItem",
                newName: "IX_VendorInvoiceItem_VendorInvoiceId");

            migrationBuilder.RenameIndex(
                name: "IX_VendorInvoiceLineItem_Item_SaleCodeId",
                schema: "dbo",
                table: "VendorInvoiceItem",
                newName: "IX_VendorInvoiceItem_SaleCodeId");

            migrationBuilder.RenameIndex(
                name: "IX_VendorInvoiceLineItem_Item_ManufacturerId",
                schema: "dbo",
                table: "VendorInvoiceItem",
                newName: "IX_VendorInvoiceItem_ManufacturerId");

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

            migrationBuilder.AddColumn<double>(
                name: "Amount",
                schema: "dbo",
                table: "VendorInvoiceTax",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<long>(
                name: "InvoiceId",
                schema: "dbo",
                table: "VendorInvoiceTax",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                schema: "dbo",
                table: "VendorInvoiceTax",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TaxName",
                schema: "dbo",
                table: "VendorInvoiceTax",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsReconciledByAnotherVendor",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
                name: "VendorInvoicePaymentMethodId",
                schema: "dbo",
                table: "VendorInvoicePayment",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "VendorId",
                schema: "dbo",
                table: "VendorInvoice",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceNumber",
                schema: "dbo",
                table: "VendorInvoice",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                schema: "dbo",
                table: "VendorInvoiceItem",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "PartNumber",
                schema: "dbo",
                table: "VendorInvoiceItem",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "dbo",
                table: "VendorInvoiceItem",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceNumber",
                schema: "dbo",
                table: "VendorInvoiceItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VendorInvoiceItem",
                schema: "dbo",
                table: "VendorInvoiceItem",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SalesTaxTaxableExciseFee",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExciseFeeId = table.Column<long>(type: "bigint", nullable: true),
                    SalesTaxId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesTaxTaxableExciseFee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesTaxTaxableExciseFee_ExciseFee_ExciseFeeId",
                        column: x => x.ExciseFeeId,
                        principalSchema: "dbo",
                        principalTable: "ExciseFee",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesTaxTaxableExciseFee_SalesTax_SalesTaxId",
                        column: x => x.SalesTaxId,
                        principalSchema: "dbo",
                        principalTable: "SalesTax",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_VendorInvoicePayment_VendorInvoicePaymentMethodId",
                schema: "dbo",
                table: "VendorInvoicePayment",
                column: "VendorInvoicePaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesTaxTaxableExciseFee_ExciseFeeId",
                schema: "dbo",
                table: "SalesTaxTaxableExciseFee",
                column: "ExciseFeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesTaxTaxableExciseFee_SalesTaxId",
                schema: "dbo",
                table: "SalesTaxTaxableExciseFee",
                column: "SalesTaxId");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoice_Vendor_VendorId",
                schema: "dbo",
                table: "VendorInvoice",
                column: "VendorId",
                principalSchema: "dbo",
                principalTable: "Vendor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoiceItem_Manufacturer_ManufacturerId",
                schema: "dbo",
                table: "VendorInvoiceItem",
                column: "ManufacturerId",
                principalSchema: "dbo",
                principalTable: "Manufacturer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoiceItem_SaleCode_SaleCodeId",
                schema: "dbo",
                table: "VendorInvoiceItem",
                column: "SaleCodeId",
                principalSchema: "dbo",
                principalTable: "SaleCode",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoiceItem_VendorInvoice_VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoiceItem",
                column: "VendorInvoiceId",
                principalSchema: "dbo",
                principalTable: "VendorInvoice",
                principalColumn: "Id");

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
    }
}
