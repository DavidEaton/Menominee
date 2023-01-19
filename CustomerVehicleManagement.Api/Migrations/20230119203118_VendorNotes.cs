using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class VendorNotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Note",
                schema: "dbo",
                table: "Vendor",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "Note",
                schema: "dbo",
                table: "Organization",
                newName: "Notes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Notes",
                schema: "dbo",
                table: "Vendor",
                newName: "Note");

            migrationBuilder.RenameColumn(
                name: "Notes",
                schema: "dbo",
                table: "Organization",
                newName: "Note");
        }
    }
}
