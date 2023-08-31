using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Menominee.Api.Migrations
{
    public partial class RepairOrderItemConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepairOrderItems_Manufacturer_ManufacturerId",
                table: "RepairOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairOrderItems_ProductCode_ProductCodeId",
                table: "RepairOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairOrderItems_RepairOrderItemLabor_LaborId",
                table: "RepairOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairOrderItems_RepairOrderItemPart_PartId",
                table: "RepairOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairOrderItems_SaleCode_SaleCodeId",
                table: "RepairOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairOrderLineItem_RepairOrderItems_ItemId",
                schema: "dbo",
                table: "RepairOrderLineItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RepairOrderItems",
                table: "RepairOrderItems");

            migrationBuilder.RenameTable(
                name: "RepairOrderItems",
                newName: "RepairOrderItem",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_RepairOrderItems_SaleCodeId",
                schema: "dbo",
                table: "RepairOrderItem",
                newName: "IX_RepairOrderItem_SaleCodeId");

            migrationBuilder.RenameIndex(
                name: "IX_RepairOrderItems_ProductCodeId",
                schema: "dbo",
                table: "RepairOrderItem",
                newName: "IX_RepairOrderItem_ProductCodeId");

            migrationBuilder.RenameIndex(
                name: "IX_RepairOrderItems_PartId",
                schema: "dbo",
                table: "RepairOrderItem",
                newName: "IX_RepairOrderItem_PartId");

            migrationBuilder.RenameIndex(
                name: "IX_RepairOrderItems_ManufacturerId",
                schema: "dbo",
                table: "RepairOrderItem",
                newName: "IX_RepairOrderItem_ManufacturerId");

            migrationBuilder.RenameIndex(
                name: "IX_RepairOrderItems_LaborId",
                schema: "dbo",
                table: "RepairOrderItem",
                newName: "IX_RepairOrderItem_LaborId");

            migrationBuilder.AlterColumn<string>(
                name: "PartNumber",
                schema: "dbo",
                table: "RepairOrderItem",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "dbo",
                table: "RepairOrderItem",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RepairOrderItem",
                schema: "dbo",
                table: "RepairOrderItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairOrderItem_Manufacturer_ManufacturerId",
                schema: "dbo",
                table: "RepairOrderItem",
                column: "ManufacturerId",
                principalSchema: "dbo",
                principalTable: "Manufacturer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairOrderItem_ProductCode_ProductCodeId",
                schema: "dbo",
                table: "RepairOrderItem",
                column: "ProductCodeId",
                principalSchema: "dbo",
                principalTable: "ProductCode",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairOrderItem_RepairOrderItemLabor_LaborId",
                schema: "dbo",
                table: "RepairOrderItem",
                column: "LaborId",
                principalSchema: "dbo",
                principalTable: "RepairOrderItemLabor",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairOrderItem_RepairOrderItemPart_PartId",
                schema: "dbo",
                table: "RepairOrderItem",
                column: "PartId",
                principalSchema: "dbo",
                principalTable: "RepairOrderItemPart",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairOrderItem_SaleCode_SaleCodeId",
                schema: "dbo",
                table: "RepairOrderItem",
                column: "SaleCodeId",
                principalSchema: "dbo",
                principalTable: "SaleCode",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairOrderLineItem_RepairOrderItem_ItemId",
                schema: "dbo",
                table: "RepairOrderLineItem",
                column: "ItemId",
                principalSchema: "dbo",
                principalTable: "RepairOrderItem",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepairOrderItem_Manufacturer_ManufacturerId",
                schema: "dbo",
                table: "RepairOrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairOrderItem_ProductCode_ProductCodeId",
                schema: "dbo",
                table: "RepairOrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairOrderItem_RepairOrderItemLabor_LaborId",
                schema: "dbo",
                table: "RepairOrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairOrderItem_RepairOrderItemPart_PartId",
                schema: "dbo",
                table: "RepairOrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairOrderItem_SaleCode_SaleCodeId",
                schema: "dbo",
                table: "RepairOrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairOrderLineItem_RepairOrderItem_ItemId",
                schema: "dbo",
                table: "RepairOrderLineItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RepairOrderItem",
                schema: "dbo",
                table: "RepairOrderItem");

            migrationBuilder.RenameTable(
                name: "RepairOrderItem",
                schema: "dbo",
                newName: "RepairOrderItems");

            migrationBuilder.RenameIndex(
                name: "IX_RepairOrderItem_SaleCodeId",
                table: "RepairOrderItems",
                newName: "IX_RepairOrderItems_SaleCodeId");

            migrationBuilder.RenameIndex(
                name: "IX_RepairOrderItem_ProductCodeId",
                table: "RepairOrderItems",
                newName: "IX_RepairOrderItems_ProductCodeId");

            migrationBuilder.RenameIndex(
                name: "IX_RepairOrderItem_PartId",
                table: "RepairOrderItems",
                newName: "IX_RepairOrderItems_PartId");

            migrationBuilder.RenameIndex(
                name: "IX_RepairOrderItem_ManufacturerId",
                table: "RepairOrderItems",
                newName: "IX_RepairOrderItems_ManufacturerId");

            migrationBuilder.RenameIndex(
                name: "IX_RepairOrderItem_LaborId",
                table: "RepairOrderItems",
                newName: "IX_RepairOrderItems_LaborId");

            migrationBuilder.AlterColumn<string>(
                name: "PartNumber",
                table: "RepairOrderItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "RepairOrderItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RepairOrderItems",
                table: "RepairOrderItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairOrderItems_Manufacturer_ManufacturerId",
                table: "RepairOrderItems",
                column: "ManufacturerId",
                principalSchema: "dbo",
                principalTable: "Manufacturer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairOrderItems_ProductCode_ProductCodeId",
                table: "RepairOrderItems",
                column: "ProductCodeId",
                principalSchema: "dbo",
                principalTable: "ProductCode",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairOrderItems_RepairOrderItemLabor_LaborId",
                table: "RepairOrderItems",
                column: "LaborId",
                principalSchema: "dbo",
                principalTable: "RepairOrderItemLabor",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairOrderItems_RepairOrderItemPart_PartId",
                table: "RepairOrderItems",
                column: "PartId",
                principalSchema: "dbo",
                principalTable: "RepairOrderItemPart",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairOrderItems_SaleCode_SaleCodeId",
                table: "RepairOrderItems",
                column: "SaleCodeId",
                principalSchema: "dbo",
                principalTable: "SaleCode",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairOrderLineItem_RepairOrderItems_ItemId",
                schema: "dbo",
                table: "RepairOrderLineItem",
                column: "ItemId",
                principalTable: "RepairOrderItems",
                principalColumn: "Id");
        }
    }
}
