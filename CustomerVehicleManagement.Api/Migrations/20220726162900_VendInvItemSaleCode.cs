using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class VendInvItemSaleCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SaleCode",
                schema: "dbo",
                table: "VendorInvoiceItem");

            migrationBuilder.AddColumn<long>(
                name: "SaleCodeId",
                schema: "dbo",
                table: "VendorInvoiceItem",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_VendorInvoiceItem_SaleCodeId",
                schema: "dbo",
                table: "VendorInvoiceItem",
                column: "SaleCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorInvoiceItem_SaleCode_SaleCodeId",
                schema: "dbo",
                table: "VendorInvoiceItem",
                column: "SaleCodeId",
                principalSchema: "dbo",
                principalTable: "SaleCode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoiceItem_SaleCode_SaleCodeId",
                schema: "dbo",
                table: "VendorInvoiceItem");

            migrationBuilder.DropIndex(
                name: "IX_VendorInvoiceItem_SaleCodeId",
                schema: "dbo",
                table: "VendorInvoiceItem");

            migrationBuilder.DropColumn(
                name: "SaleCodeId",
                schema: "dbo",
                table: "VendorInvoiceItem");

            migrationBuilder.AddColumn<string>(
                name: "SaleCode",
                schema: "dbo",
                table: "VendorInvoiceItem",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
