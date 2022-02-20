using Microsoft.EntityFrameworkCore.Migrations;

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class RepairOrderItemRollback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepairOrderItems_Manufacturer_ManufacturerId",
                schema: "dbo",
                table: "RepairOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairOrderItems_ProductCode_ProductCodeId",
                schema: "dbo",
                table: "RepairOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairOrderItems_SaleCode_SaleCodeId",
                schema: "dbo",
                table: "RepairOrderItems");

            migrationBuilder.AlterColumn<long>(
                name: "SaleCodeId",
                schema: "dbo",
                table: "RepairOrderItems",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ProductCodeId",
                schema: "dbo",
                table: "RepairOrderItems",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ManufacturerId",
                schema: "dbo",
                table: "RepairOrderItems",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairOrderItems_Manufacturer_ManufacturerId",
                schema: "dbo",
                table: "RepairOrderItems",
                column: "ManufacturerId",
                principalSchema: "dbo",
                principalTable: "Manufacturer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairOrderItems_ProductCode_ProductCodeId",
                schema: "dbo",
                table: "RepairOrderItems",
                column: "ProductCodeId",
                principalSchema: "dbo",
                principalTable: "ProductCode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairOrderItems_SaleCode_SaleCodeId",
                schema: "dbo",
                table: "RepairOrderItems",
                column: "SaleCodeId",
                principalSchema: "dbo",
                principalTable: "SaleCode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepairOrderItems_Manufacturer_ManufacturerId",
                schema: "dbo",
                table: "RepairOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairOrderItems_ProductCode_ProductCodeId",
                schema: "dbo",
                table: "RepairOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairOrderItems_SaleCode_SaleCodeId",
                schema: "dbo",
                table: "RepairOrderItems");

            migrationBuilder.AlterColumn<long>(
                name: "SaleCodeId",
                schema: "dbo",
                table: "RepairOrderItems",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "ProductCodeId",
                schema: "dbo",
                table: "RepairOrderItems",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "ManufacturerId",
                schema: "dbo",
                table: "RepairOrderItems",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairOrderItems_Manufacturer_ManufacturerId",
                schema: "dbo",
                table: "RepairOrderItems",
                column: "ManufacturerId",
                principalSchema: "dbo",
                principalTable: "Manufacturer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairOrderItems_ProductCode_ProductCodeId",
                schema: "dbo",
                table: "RepairOrderItems",
                column: "ProductCodeId",
                principalSchema: "dbo",
                principalTable: "ProductCode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairOrderItems_SaleCode_SaleCodeId",
                schema: "dbo",
                table: "RepairOrderItems",
                column: "SaleCodeId",
                principalSchema: "dbo",
                principalTable: "SaleCode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
