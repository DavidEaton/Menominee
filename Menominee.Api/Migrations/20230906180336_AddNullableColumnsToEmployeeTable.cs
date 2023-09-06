using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Menominee.Api.Migrations
{
    public partial class AddNullableColumnsToEmployeeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                schema: "dbo",
                table: "Employee",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<double>(
                name: "BenefitLoad",
                schema: "dbo",
                table: "Employee",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "CertificationNumber",
                schema: "dbo",
                table: "Employee",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExpenseCategory",
                schema: "dbo",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PrintedName",
                schema: "dbo",
                table: "Employee",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SSN",
                schema: "dbo",
                table: "Employee",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                schema: "dbo",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "BenefitLoad",
                schema: "dbo",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "CertificationNumber",
                schema: "dbo",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "ExpenseCategory",
                schema: "dbo",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "PrintedName",
                schema: "dbo",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "SSN",
                schema: "dbo",
                table: "Employee");
        }
    }
}
