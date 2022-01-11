using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MenomineePlayWASM.Shared.Migrations
{
    public partial class NotSure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoiceItems_VendorInvoices_InvoiceId1",
                schema: "dbo",
                table: "VendorInvoiceItems");

            migrationBuilder.DropIndex(
                name: "IX_VendorInvoiceItems_InvoiceId1",
                schema: "dbo",
                table: "VendorInvoiceItems");

            migrationBuilder.DropColumn(
                name: "InvoiceId1",
                schema: "dbo",
                table: "VendorInvoiceItems");

            migrationBuilder.AlterColumn<long>(
                name: "InvoiceId",
                schema: "dbo",
                table: "VendorInvoiceTaxes",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

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
                oldDefaultValue: new DateTime(2021, 11, 19, 8, 15, 0, 257, DateTimeKind.Local).AddTicks(1137));

            migrationBuilder.AlterColumn<long>(
                name: "InvoiceId",
                schema: "dbo",
                table: "VendorInvoicePayments",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "InvoiceId",
                schema: "dbo",
                table: "VendorInvoiceItems",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_VendorInvoiceItems_InvoiceId",
                schema: "dbo",
                table: "VendorInvoiceItems",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoiceItems_VendorInvoices_InvoiceId",
                schema: "dbo",
                table: "VendorInvoiceItems",
                column: "InvoiceId",
                principalSchema: "dbo",
                principalTable: "VendorInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoiceItems_VendorInvoices_InvoiceId",
                schema: "dbo",
                table: "VendorInvoiceItems");

            migrationBuilder.DropIndex(
                name: "IX_VendorInvoiceItems_InvoiceId",
                schema: "dbo",
                table: "VendorInvoiceItems");

            migrationBuilder.AlterColumn<int>(
                name: "InvoiceId",
                schema: "dbo",
                table: "VendorInvoiceTaxes",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                schema: "dbo",
                table: "VendorInvoices",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2021, 11, 19, 8, 15, 0, 257, DateTimeKind.Local).AddTicks(1137),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2021, 11, 22, 10, 39, 12, 714, DateTimeKind.Local).AddTicks(5619));

            migrationBuilder.AlterColumn<int>(
                name: "InvoiceId",
                schema: "dbo",
                table: "VendorInvoicePayments",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "InvoiceId",
                schema: "dbo",
                table: "VendorInvoiceItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "InvoiceId1",
                schema: "dbo",
                table: "VendorInvoiceItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VendorInvoiceItems_InvoiceId1",
                schema: "dbo",
                table: "VendorInvoiceItems",
                column: "InvoiceId1");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoiceItems_VendorInvoices_InvoiceId1",
                schema: "dbo",
                table: "VendorInvoiceItems",
                column: "InvoiceId1",
                principalSchema: "dbo",
                principalTable: "VendorInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
