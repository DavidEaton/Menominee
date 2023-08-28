using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Menominee.Api.Migrations
{
    public partial class AddNullableColumnsToVehiclesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                schema: "dbo",
                table: "Vehicle",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                schema: "dbo",
                table: "Vehicle",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Plate",
                schema: "dbo",
                table: "Vehicle",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlateStateProvince",
                schema: "dbo",
                table: "Vehicle",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnitNumber",
                schema: "dbo",
                table: "Vehicle",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                schema: "dbo",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "Color",
                schema: "dbo",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "Plate",
                schema: "dbo",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "PlateStateProvince",
                schema: "dbo",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "UnitNumber",
                schema: "dbo",
                table: "Vehicle");
        }
    }
}
