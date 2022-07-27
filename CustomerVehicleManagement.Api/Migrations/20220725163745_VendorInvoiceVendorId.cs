using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class VendorInvoiceVendorId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoice_Vendor_VendorId",
                schema: "dbo",
                table: "VendorInvoice");

            migrationBuilder.AlterColumn<long>(
                name: "VendorId",
                schema: "dbo",
                table: "VendorInvoice",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoice_Vendor_VendorId",
                schema: "dbo",
                table: "VendorInvoice",
                column: "VendorId",
                principalSchema: "dbo",
                principalTable: "Vendor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoice_Vendor_VendorId",
                schema: "dbo",
                table: "VendorInvoice");

            migrationBuilder.AlterColumn<long>(
                name: "VendorId",
                schema: "dbo",
                table: "VendorInvoice",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoice_Vendor_VendorId",
                schema: "dbo",
                table: "VendorInvoice",
                column: "VendorId",
                principalSchema: "dbo",
                principalTable: "Vendor",
                principalColumn: "Id");
        }
    }
}
