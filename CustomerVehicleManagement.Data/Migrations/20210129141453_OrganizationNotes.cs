using Microsoft.EntityFrameworkCore.Migrations;

namespace CustomerVehicleManagement.Data.Migrations
{
    public partial class OrganizationNotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "dbo",
                table: "Organization",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "dbo",
                table: "Organization");
        }
    }
}
