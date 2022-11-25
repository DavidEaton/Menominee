using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class RenameVendorTypeToVendorRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VendorType",
                schema: "dbo",
                table: "Vendor",
                newName: "VendorRole");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VendorRole",
                schema: "dbo",
                table: "Vendor",
                newName: "VendorType");
        }
    }
}
