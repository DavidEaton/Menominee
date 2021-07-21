using Microsoft.EntityFrameworkCore.Migrations;

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class CustomerRefactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntityId",
                schema: "dbo",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "EntityType",
                schema: "dbo",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "Notes",
                schema: "dbo",
                table: "Organization",
                newName: "Note");

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                schema: "dbo",
                table: "Customer",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PersonId",
                schema: "dbo",
                table: "Customer",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_OrganizationId",
                schema: "dbo",
                table: "Customer",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_PersonId",
                schema: "dbo",
                table: "Customer",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Organization_OrganizationId",
                schema: "dbo",
                table: "Customer",
                column: "OrganizationId",
                principalSchema: "dbo",
                principalTable: "Organization",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Person_PersonId",
                schema: "dbo",
                table: "Customer",
                column: "PersonId",
                principalSchema: "dbo",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Organization_OrganizationId",
                schema: "dbo",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Person_PersonId",
                schema: "dbo",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_OrganizationId",
                schema: "dbo",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_PersonId",
                schema: "dbo",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                schema: "dbo",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "PersonId",
                schema: "dbo",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "Note",
                schema: "dbo",
                table: "Organization",
                newName: "Notes");

            migrationBuilder.AddColumn<int>(
                name: "EntityId",
                schema: "dbo",
                table: "Customer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntityType",
                schema: "dbo",
                table: "Customer",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
