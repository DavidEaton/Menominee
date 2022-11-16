using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class VendorDocumentTypeDefaultPaymentMethod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItemPackageItem_InventoryItem_InventoryItemId",
                schema: "dbo",
                table: "InventoryItemPackageItem");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCode_Manufacturer_ManufacturerId",
                schema: "dbo",
                table: "ProductCode");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCode_SaleCode_SaleCodeId",
                schema: "dbo",
                table: "ProductCode");

            migrationBuilder.RenameColumn(
                name: "InventoryItemId",
                schema: "dbo",
                table: "InventoryItemPackageItem",
                newName: "ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryItemPackageItem_InventoryItemId",
                schema: "dbo",
                table: "InventoryItemPackageItem",
                newName: "IX_InventoryItemPackageItem_ItemId");

            migrationBuilder.AddColumn<int>(
                name: "DocumentType",
                schema: "dbo",
                table: "VendorInvoice",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "DefaultPaymentMethodAutoCompleteDocuments",
                schema: "dbo",
                table: "Vendor",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DefaultPaymentMethod_PaymentMethodId",
                schema: "dbo",
                table: "Vendor",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VendorType",
                schema: "dbo",
                table: "Vendor",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<long>(
                name: "SaleCodeId",
                schema: "dbo",
                table: "ProductCode",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "ManufacturerId",
                schema: "dbo",
                table: "ProductCode",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_Vendor_DefaultPaymentMethod_PaymentMethodId",
                schema: "dbo",
                table: "Vendor",
                column: "DefaultPaymentMethod_PaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItemPackageItem_InventoryItem_ItemId",
                schema: "dbo",
                table: "InventoryItemPackageItem",
                column: "ItemId",
                principalSchema: "dbo",
                principalTable: "InventoryItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCode_Manufacturer_ManufacturerId",
                schema: "dbo",
                table: "ProductCode",
                column: "ManufacturerId",
                principalSchema: "dbo",
                principalTable: "Manufacturer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCode_SaleCode_SaleCodeId",
                schema: "dbo",
                table: "ProductCode",
                column: "SaleCodeId",
                principalSchema: "dbo",
                principalTable: "SaleCode",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendor_VendorInvoicePaymentMethod_DefaultPaymentMethod_PaymentMethodId",
                schema: "dbo",
                table: "Vendor",
                column: "DefaultPaymentMethod_PaymentMethodId",
                principalSchema: "dbo",
                principalTable: "VendorInvoicePaymentMethod",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItemPackageItem_InventoryItem_ItemId",
                schema: "dbo",
                table: "InventoryItemPackageItem");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCode_Manufacturer_ManufacturerId",
                schema: "dbo",
                table: "ProductCode");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCode_SaleCode_SaleCodeId",
                schema: "dbo",
                table: "ProductCode");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendor_VendorInvoicePaymentMethod_DefaultPaymentMethod_PaymentMethodId",
                schema: "dbo",
                table: "Vendor");

            migrationBuilder.DropIndex(
                name: "IX_Vendor_DefaultPaymentMethod_PaymentMethodId",
                schema: "dbo",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "DocumentType",
                schema: "dbo",
                table: "VendorInvoice");

            migrationBuilder.DropColumn(
                name: "DefaultPaymentMethodAutoCompleteDocuments",
                schema: "dbo",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "DefaultPaymentMethod_PaymentMethodId",
                schema: "dbo",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "VendorType",
                schema: "dbo",
                table: "Vendor");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                schema: "dbo",
                table: "InventoryItemPackageItem",
                newName: "InventoryItemId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryItemPackageItem_ItemId",
                schema: "dbo",
                table: "InventoryItemPackageItem",
                newName: "IX_InventoryItemPackageItem_InventoryItemId");

            migrationBuilder.AlterColumn<long>(
                name: "SaleCodeId",
                schema: "dbo",
                table: "ProductCode",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ManufacturerId",
                schema: "dbo",
                table: "ProductCode",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItemPackageItem_InventoryItem_InventoryItemId",
                schema: "dbo",
                table: "InventoryItemPackageItem",
                column: "InventoryItemId",
                principalSchema: "dbo",
                principalTable: "InventoryItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCode_Manufacturer_ManufacturerId",
                schema: "dbo",
                table: "ProductCode",
                column: "ManufacturerId",
                principalSchema: "dbo",
                principalTable: "Manufacturer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCode_SaleCode_SaleCodeId",
                schema: "dbo",
                table: "ProductCode",
                column: "SaleCodeId",
                principalSchema: "dbo",
                principalTable: "SaleCode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
