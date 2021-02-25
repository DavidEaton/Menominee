using Microsoft.EntityFrameworkCore.Migrations;

namespace CustomerVehicleManagement.Data.Migrations
{
    public partial class EmailPhoneIsPrimary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                schema: "dbo",
                table: "Email",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Email_OrganizationId",
                schema: "dbo",
                table: "Email",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Email_Organization_OrganizationId",
                schema: "dbo",
                table: "Email",
                column: "OrganizationId",
                principalSchema: "dbo",
                principalTable: "Organization",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Email_Organization_OrganizationId",
                schema: "dbo",
                table: "Email");

            migrationBuilder.DropIndex(
                name: "IX_Email_OrganizationId",
                schema: "dbo",
                table: "Email");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                schema: "dbo",
                table: "Email");
        }
    }
}
