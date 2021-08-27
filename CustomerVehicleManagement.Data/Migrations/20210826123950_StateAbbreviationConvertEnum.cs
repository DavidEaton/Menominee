using Microsoft.EntityFrameworkCore.Migrations;

namespace CustomerVehicleManagement.Data.Migrations
{
    public partial class StateAbbreviationConvertEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AddressState",
                schema: "dbo",
                table: "Person",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 2,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AddressState",
                schema: "dbo",
                table: "Person",
                type: "int",
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2)",
                oldMaxLength: 2,
                oldNullable: true);
        }
    }
}
