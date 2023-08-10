using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Menominee.Api.Migrations
{
    /// <inheritdoc />
    public partial class RenameOrganizationToBusiness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Company_Organization_OrganizationId",
                schema: "dbo",
                table: "Company");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Organization_OrganizationId",
                schema: "dbo",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Email_Organization_OrganizationId",
                schema: "dbo",
                table: "Email");

            migrationBuilder.DropForeignKey(
                name: "FK_Phone_Organization_OrganizationId",
                schema: "dbo",
                table: "Phone");

            migrationBuilder.DropTable(
                name: "Organization",
                schema: "dbo");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                schema: "dbo",
                table: "Phone",
                newName: "BusinessId");

            migrationBuilder.RenameIndex(
                name: "IX_Phone_OrganizationId",
                schema: "dbo",
                table: "Phone",
                newName: "IX_Phone_BusinessId");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                schema: "dbo",
                table: "Email",
                newName: "BusinessId");

            migrationBuilder.RenameIndex(
                name: "IX_Email_OrganizationId",
                schema: "dbo",
                table: "Email",
                newName: "IX_Email_BusinessId");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                schema: "dbo",
                table: "Customer",
                newName: "BusinessId");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_OrganizationId",
                schema: "dbo",
                table: "Customer",
                newName: "IX_Customer_BusinessId");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                schema: "dbo",
                table: "Company",
                newName: "BusinessId");

            migrationBuilder.RenameIndex(
                name: "IX_Company_OrganizationId",
                schema: "dbo",
                table: "Company",
                newName: "IX_Company_BusinessId");

            migrationBuilder.CreateTable(
                name: "Business",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ContactId = table.Column<long>(type: "bigint", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: true),
                    AddressLine = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AddressCity = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AddressState = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    AddressPostalCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Business", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Business_Person_ContactId",
                        column: x => x.ContactId,
                        principalSchema: "dbo",
                        principalTable: "Person",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Business_ContactId",
                schema: "dbo",
                table: "Business",
                column: "ContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_Company_Business_BusinessId",
                schema: "dbo",
                table: "Company",
                column: "BusinessId",
                principalSchema: "dbo",
                principalTable: "Business",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Business_BusinessId",
                schema: "dbo",
                table: "Customer",
                column: "BusinessId",
                principalSchema: "dbo",
                principalTable: "Business",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Email_Business_BusinessId",
                schema: "dbo",
                table: "Email",
                column: "BusinessId",
                principalSchema: "dbo",
                principalTable: "Business",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Phone_Business_BusinessId",
                schema: "dbo",
                table: "Phone",
                column: "BusinessId",
                principalSchema: "dbo",
                principalTable: "Business",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Company_Business_BusinessId",
                schema: "dbo",
                table: "Company");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Business_BusinessId",
                schema: "dbo",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Email_Business_BusinessId",
                schema: "dbo",
                table: "Email");

            migrationBuilder.DropForeignKey(
                name: "FK_Phone_Business_BusinessId",
                schema: "dbo",
                table: "Phone");

            migrationBuilder.DropTable(
                name: "Business",
                schema: "dbo");

            migrationBuilder.RenameColumn(
                name: "BusinessId",
                schema: "dbo",
                table: "Phone",
                newName: "OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_Phone_BusinessId",
                schema: "dbo",
                table: "Phone",
                newName: "IX_Phone_OrganizationId");

            migrationBuilder.RenameColumn(
                name: "BusinessId",
                schema: "dbo",
                table: "Email",
                newName: "OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_Email_BusinessId",
                schema: "dbo",
                table: "Email",
                newName: "IX_Email_OrganizationId");

            migrationBuilder.RenameColumn(
                name: "BusinessId",
                schema: "dbo",
                table: "Customer",
                newName: "OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_BusinessId",
                schema: "dbo",
                table: "Customer",
                newName: "IX_Customer_OrganizationId");

            migrationBuilder.RenameColumn(
                name: "BusinessId",
                schema: "dbo",
                table: "Company",
                newName: "OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_Company_BusinessId",
                schema: "dbo",
                table: "Company",
                newName: "IX_Company_OrganizationId");

            migrationBuilder.CreateTable(
                name: "Organization",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactId = table.Column<long>(type: "bigint", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: true),
                    AddressLine = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AddressCity = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AddressPostalCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    AddressState = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organization_Person_ContactId",
                        column: x => x.ContactId,
                        principalSchema: "dbo",
                        principalTable: "Person",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Organization_ContactId",
                schema: "dbo",
                table: "Organization",
                column: "ContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_Company_Organization_OrganizationId",
                schema: "dbo",
                table: "Company",
                column: "OrganizationId",
                principalSchema: "dbo",
                principalTable: "Organization",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Organization_OrganizationId",
                schema: "dbo",
                table: "Customer",
                column: "OrganizationId",
                principalSchema: "dbo",
                principalTable: "Organization",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Email_Organization_OrganizationId",
                schema: "dbo",
                table: "Email",
                column: "OrganizationId",
                principalSchema: "dbo",
                principalTable: "Organization",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Phone_Organization_OrganizationId",
                schema: "dbo",
                table: "Phone",
                column: "OrganizationId",
                principalSchema: "dbo",
                principalTable: "Organization",
                principalColumn: "Id");
        }
    }
}
