using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class VendInvItemMfrId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoiceItem_Manufacturer_ManufacturerId",
                schema: "dbo",
                table: "VendorInvoiceItem");

            migrationBuilder.AlterColumn<long>(
                name: "ManufacturerId",
                schema: "dbo",
                table: "VendorInvoiceItem",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoiceItem_Manufacturer_ManufacturerId",
                schema: "dbo",
                table: "VendorInvoiceItem",
                column: "ManufacturerId",
                principalSchema: "dbo",
                principalTable: "Manufacturer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoiceItem_Manufacturer_ManufacturerId",
                schema: "dbo",
                table: "VendorInvoiceItem");

            migrationBuilder.AlterColumn<long>(
                name: "ManufacturerId",
                schema: "dbo",
                table: "VendorInvoiceItem",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoiceItem_Manufacturer_ManufacturerId",
                schema: "dbo",
                table: "VendorInvoiceItem",
                column: "ManufacturerId",
                principalSchema: "dbo",
                principalTable: "Manufacturer",
                principalColumn: "Id");
        }
    }
}
