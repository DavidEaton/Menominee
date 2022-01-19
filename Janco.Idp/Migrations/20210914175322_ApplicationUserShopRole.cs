using Microsoft.EntityFrameworkCore.Migrations;

namespace Menominee.Idp.Migrations
{
    public partial class ApplicationUserShopRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShopRole",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShopRole",
                table: "AspNetUsers");
        }
    }
}
