using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Menominee.Api.Migrations
{
    public partial class Add_Settings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Setting",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SettingName = table.Column<int>(type: "int", maxLength: 255, nullable: false),
                    SettingGroup = table.Column<int>(type: "int", maxLength: 255, nullable: false),
                    SettingValueType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setting", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Setting",
                schema: "dbo");
        }
    }
}
