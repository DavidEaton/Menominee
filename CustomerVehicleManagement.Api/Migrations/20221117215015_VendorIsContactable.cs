using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class VendorIsContactable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddressCity",
                schema: "dbo",
                table: "Vendor",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressLine",
                schema: "dbo",
                table: "Vendor",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressPostalCode",
                schema: "dbo",
                table: "Vendor",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressState",
                schema: "dbo",
                table: "Vendor",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "VendorId",
                schema: "dbo",
                table: "Phone",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "VendorId",
                schema: "dbo",
                table: "Email",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Phone_VendorId",
                schema: "dbo",
                table: "Phone",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Email_VendorId",
                schema: "dbo",
                table: "Email",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Email_Vendor_VendorId",
                schema: "dbo",
                table: "Email",
                column: "VendorId",
                principalSchema: "dbo",
                principalTable: "Vendor",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Phone_Vendor_VendorId",
                schema: "dbo",
                table: "Phone",
                column: "VendorId",
                principalSchema: "dbo",
                principalTable: "Vendor",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Email_Vendor_VendorId",
                schema: "dbo",
                table: "Email");

            migrationBuilder.DropForeignKey(
                name: "FK_Phone_Vendor_VendorId",
                schema: "dbo",
                table: "Phone");

            migrationBuilder.DropIndex(
                name: "IX_Phone_VendorId",
                schema: "dbo",
                table: "Phone");

            migrationBuilder.DropIndex(
                name: "IX_Email_VendorId",
                schema: "dbo",
                table: "Email");

            migrationBuilder.DropColumn(
                name: "AddressCity",
                schema: "dbo",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "AddressLine",
                schema: "dbo",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "AddressPostalCode",
                schema: "dbo",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "AddressState",
                schema: "dbo",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "VendorId",
                schema: "dbo",
                table: "Phone");

            migrationBuilder.DropColumn(
                name: "VendorId",
                schema: "dbo",
                table: "Email");
        }
    }
}
