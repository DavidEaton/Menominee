using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MenomineePlayWASM.Shared.Migrations
{
    public partial class VendorIdToVendorCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "Vendors");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "Vendors",
                newName: "Vendors",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "VendorInvoiceTaxes",
                newName: "VendorInvoiceTaxes",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "VendorInvoices",
                newName: "VendorInvoices",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "VendorInvoicePayments",
                newName: "VendorInvoicePayments",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "VendorInvoiceItems",
                newName: "VendorInvoiceItems",
                newSchema: "dbo");

            migrationBuilder.AlterColumn<long>(
                name: "VendorId",
                table: "CreditReturns",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "Vendors",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "dbo",
                table: "Vendors",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "VendorCode",
                schema: "dbo",
                table: "Vendors",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                schema: "dbo",
                table: "VendorInvoices",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                schema: "dbo",
                table: "VendorInvoices",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2021, 11, 19, 8, 15, 0, 257, DateTimeKind.Local).AddTicks(1137),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                schema: "dbo",
                table: "VendorInvoiceItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "PartNumber",
                schema: "dbo",
                table: "VendorInvoiceItems",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MfrId",
                schema: "dbo",
                table: "VendorInvoiceItems",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "dbo",
                table: "VendorInvoiceItems",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VendorCode",
                schema: "dbo",
                table: "Vendors");

            migrationBuilder.RenameTable(
                name: "Vendors",
                schema: "dbo",
                newName: "Vendors");

            migrationBuilder.RenameTable(
                name: "VendorInvoiceTaxes",
                schema: "dbo",
                newName: "VendorInvoiceTaxes");

            migrationBuilder.RenameTable(
                name: "VendorInvoices",
                schema: "dbo",
                newName: "VendorInvoices");

            migrationBuilder.RenameTable(
                name: "VendorInvoicePayments",
                schema: "dbo",
                newName: "VendorInvoicePayments");

            migrationBuilder.RenameTable(
                name: "VendorInvoiceItems",
                schema: "dbo",
                newName: "VendorInvoiceItems");

            migrationBuilder.AlterColumn<string>(
                name: "VendorId",
                table: "CreditReturns",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Vendors",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorId",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "VendorInvoices",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "VendorInvoices",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2021, 11, 19, 8, 15, 0, 257, DateTimeKind.Local).AddTicks(1137));

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "VendorInvoiceItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "PartNumber",
                table: "VendorInvoiceItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "MfrId",
                table: "VendorInvoiceItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "VendorInvoiceItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);
        }
    }
}
