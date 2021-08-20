using Microsoft.EntityFrameworkCore.Migrations;

namespace Menominee.Idp.Migrations
{
    public partial class AddAspNetTenantAlternateKey_Name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AlternateKey_Name",
                table: "AspNetTenants",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AlternateKey_Name",
                table: "AspNetTenants");
        }
    }
}
