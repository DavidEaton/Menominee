using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class VendInvItemNullableIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoiceItem_SaleCode_SaleCodeId",
                schema: "dbo",
                table: "VendorInvoiceItem");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoiceItem_VendorInvoice_VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoiceItem");

            migrationBuilder.AlterColumn<long>(
                name: "VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoiceItem",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "SaleCodeId",
                schema: "dbo",
                table: "VendorInvoiceItem",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoiceItem_SaleCode_SaleCodeId",
                schema: "dbo",
                table: "VendorInvoiceItem");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoiceItem_VendorInvoice_VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoiceItem");

            migrationBuilder.AlterColumn<long>(
                name: "VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoiceItem",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "SaleCodeId",
                schema: "dbo",
                table: "VendorInvoiceItem",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoiceItem_SaleCode_SaleCodeId",
                schema: "dbo",
                table: "VendorInvoiceItem",
                column: "SaleCodeId",
                principalSchema: "dbo",
                principalTable: "SaleCode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoiceItem_VendorInvoice_VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoiceItem",
                column: "VendorInvoiceId",
                principalSchema: "dbo",
                principalTable: "VendorInvoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
