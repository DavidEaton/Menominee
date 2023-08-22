using Menominee.Domain.Entities.Inventory;
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
                columns: new[]
                {
                    "Name",
                    "Prefix",
                    "Code"
                },
                values: new object[,]
                {
                    { "Custom", "I", "0" },
                    { "Miscellaneous", "X", "1" },
                    { "Custom - Stocked", "CS", "2" },
                    { "Package", "PKG", "3" }
                });
        }

    }
}
