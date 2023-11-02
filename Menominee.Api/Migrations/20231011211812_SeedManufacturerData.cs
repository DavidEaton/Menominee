using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Menominee.Api.Migrations
{
    public partial class SeedManufacturerData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Manufacturer",
                schema: "dbo",
                columns: new[] { "Name", "Prefix" },
                values: new object[,]
                {
                    { "Custom", "I" },
                    { "Miscellaneous", "X" },
                    { "Custom - Stocked", "CS" },
                    { "Package", "PKG" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Manufacturer",
                schema: "dbo",
                keyColumn: "Name",
                keyValue: "Custom");

            migrationBuilder.DeleteData(
                table: "Manufacturer",
                schema: "dbo",
                keyColumn: "Name",
                keyValue: "Miscellaneous");

            migrationBuilder.DeleteData(
                table: "Manufacturer",
                schema: "dbo",
                keyColumn: "Name",
                keyValue: "Custom - Stocked");

            migrationBuilder.DeleteData(
                table: "Manufacturer",
                schema: "dbo",
                keyColumn: "Name",
                keyValue: "Package");
        }
    }
}
