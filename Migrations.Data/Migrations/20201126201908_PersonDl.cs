using Microsoft.EntityFrameworkCore.Migrations;

namespace Migrations.Data.Migrations
{
    public partial class PersonDl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "State",
                table: "Person",
                newName: "DriversLicenseState");

            migrationBuilder.RenameColumn(
                name: "Number",
                table: "Person",
                newName: "DriversLicenseNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DriversLicenseState",
                table: "Person",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "DriversLicenseNumber",
                table: "Person",
                newName: "Number");
        }
    }
}
