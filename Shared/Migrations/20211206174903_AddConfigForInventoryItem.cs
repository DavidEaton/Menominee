using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MenomineePlayWASM.Shared.Migrations
{
    public partial class AddConfigForInventoryItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                schema: "dbo",
                table: "VendorInvoices",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2021, 12, 6, 12, 49, 3, 567, DateTimeKind.Local).AddTicks(2186),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2021, 12, 6, 12, 28, 48, 807, DateTimeKind.Local).AddTicks(8809));

            migrationBuilder.AddColumn<int>(
                name: "PartType",
                table: "InventoryItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PartType",
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
                oldDefaultValue: new DateTime(2021, 12, 6, 12, 49, 3, 567, DateTimeKind.Local).AddTicks(2186));
        }
    }
}
