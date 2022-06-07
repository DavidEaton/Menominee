using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class PkgItemInventoryItemId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_InventoryItemPackageItem_ItemId",
                schema: "dbo",
                table: "InventoryItemPackageItem");

            migrationBuilder.DropColumn(
                name: "ItemId",
                schema: "dbo",
                table: "InventoryItemPackageItem");

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

            migrationBuilder.AddColumn<long>(
                name: "InventoryItemId",
                schema: "dbo",
                table: "InventoryItemPackageItem",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItemPackageItem_InventoryItemId",
                schema: "dbo",
                table: "InventoryItemPackageItem",
                column: "InventoryItemId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_InventoryItemPackageItem_InventoryItemId",
                schema: "dbo",
                table: "InventoryItemPackageItem");

            migrationBuilder.DropColumn(
                name: "InventoryItemId",
                schema: "dbo",
                table: "InventoryItemPackageItem");

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

            migrationBuilder.AddColumn<long>(
                name: "ItemId",
                schema: "dbo",
                table: "InventoryItemPackageItem",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItemPackageItem_ItemId",
                schema: "dbo",
                table: "InventoryItemPackageItem",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItemPackageItem_InventoryItem_ItemId",
                schema: "dbo",
                table: "InventoryItemPackageItem",
                column: "ItemId",
                principalSchema: "dbo",
                principalTable: "InventoryItem",
                principalColumn: "Id");

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
        }
    }
}
