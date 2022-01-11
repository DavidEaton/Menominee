using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MenomineePlayWASM.Shared.Migrations
{
    public partial class InventoryItemFKManufacturer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MfrId",
                table: "InventoryItems");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                schema: "dbo",
                table: "VendorInvoices",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2021, 12, 6, 12, 28, 48, 807, DateTimeKind.Local).AddTicks(8809),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2021, 11, 22, 10, 39, 12, 714, DateTimeKind.Local).AddTicks(5619));

            migrationBuilder.AddColumn<int>(
                name: "TrackingState",
                table: "Manufacturers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "PartNumber",
                table: "InventoryItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<long>(
                name: "ManufacturerId",
                table: "InventoryItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrackingState",
                table: "InventoryItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_ManufacturerId",
                table: "InventoryItems",
                column: "ManufacturerId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_Manufacturers_ManufacturerId",
                table: "InventoryItems",
                column: "ManufacturerId",
                principalTable: "Manufacturers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_Manufacturers_ManufacturerId",
                table: "InventoryItems");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItems_ManufacturerId",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "TrackingState",
                table: "Manufacturers");

            migrationBuilder.DropColumn(
                name: "ManufacturerId",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "TrackingState",
                table: "InventoryItems");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                schema: "dbo",
                table: "VendorInvoices",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2021, 11, 22, 10, 39, 12, 714, DateTimeKind.Local).AddTicks(5619),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2021, 12, 6, 12, 28, 48, 807, DateTimeKind.Local).AddTicks(8809));

            migrationBuilder.AlterColumn<string>(
                name: "PartNumber",
                table: "InventoryItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MfrId",
                table: "InventoryItems",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
