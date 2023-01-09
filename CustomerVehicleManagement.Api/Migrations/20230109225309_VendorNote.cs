using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class VendorNote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                schema: "dbo",
                table: "Vendor",
                type: "nvarchar(max)",
                maxLength: 10000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                schema: "dbo",
                table: "Vendor");
        }
    }
}
