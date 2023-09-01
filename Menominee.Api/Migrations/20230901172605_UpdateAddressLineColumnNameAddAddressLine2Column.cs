using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Menominee.Api.Migrations
{
    public partial class UpdateAddressLineColumnNameAddAddressLine2Column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AddressLine",
                schema: "dbo",
                table: "Vendor",
                newName: "AddressLine1");

            migrationBuilder.RenameColumn(
                name: "AddressLine",
                schema: "dbo",
                table: "Person",
                newName: "AddressLine1");

            migrationBuilder.RenameColumn(
                name: "AddressLine",
                schema: "dbo",
                table: "Business",
                newName: "AddressLine1");

            migrationBuilder.AlterColumn<string>(
                name: "AddressPostalCode",
                schema: "dbo",
                table: "Vendor",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressLine2",
                schema: "dbo",
                table: "Vendor",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AddressPostalCode",
                schema: "dbo",
                table: "Person",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressLine2",
                schema: "dbo",
                table: "Person",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AddressPostalCode",
                schema: "dbo",
                table: "Business",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressLine2",
                schema: "dbo",
                table: "Business",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressLine2",
                schema: "dbo",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "AddressLine2",
                schema: "dbo",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "AddressLine2",
                schema: "dbo",
                table: "Business");

            migrationBuilder.RenameColumn(
                name: "AddressLine1",
                schema: "dbo",
                table: "Vendor",
                newName: "AddressLine");

            migrationBuilder.RenameColumn(
                name: "AddressLine1",
                schema: "dbo",
                table: "Person",
                newName: "AddressLine");

            migrationBuilder.RenameColumn(
                name: "AddressLine1",
                schema: "dbo",
                table: "Business",
                newName: "AddressLine");

            migrationBuilder.AlterColumn<string>(
                name: "AddressPostalCode",
                schema: "dbo",
                table: "Vendor",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AddressPostalCode",
                schema: "dbo",
                table: "Person",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AddressPostalCode",
                schema: "dbo",
                table: "Business",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);
        }
    }
}
