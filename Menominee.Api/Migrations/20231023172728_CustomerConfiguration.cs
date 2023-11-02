using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Menominee.Api.Migrations
{
    /// <inheritdoc />
    public partial class CustomerConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Email_Customer_CustomerId",
                schema: "dbo",
                table: "Email");

            migrationBuilder.DropForeignKey(
                name: "FK_Phone_Customer_CustomerId",
                schema: "dbo",
                table: "Phone");

            migrationBuilder.DropIndex(
                name: "IX_Phone_CustomerId",
                schema: "dbo",
                table: "Phone");

            migrationBuilder.DropIndex(
                name: "IX_Email_CustomerId",
                schema: "dbo",
                table: "Email");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                schema: "dbo",
                table: "Phone");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                schema: "dbo",
                table: "Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CustomerId",
                schema: "dbo",
                table: "Phone",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CustomerId",
                schema: "dbo",
                table: "Email",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Phone_CustomerId",
                schema: "dbo",
                table: "Phone",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Email_CustomerId",
                schema: "dbo",
                table: "Email",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Email_Customer_CustomerId",
                schema: "dbo",
                table: "Email",
                column: "CustomerId",
                principalSchema: "dbo",
                principalTable: "Customer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Phone_Customer_CustomerId",
                schema: "dbo",
                table: "Phone",
                column: "CustomerId",
                principalSchema: "dbo",
                principalTable: "Customer",
                principalColumn: "Id");
        }
    }
}
