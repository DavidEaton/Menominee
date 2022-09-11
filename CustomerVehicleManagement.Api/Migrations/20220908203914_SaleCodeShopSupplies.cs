using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class SaleCodeShopSupplies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncludeLabor",
                schema: "dbo",
                table: "SaleCode");

            migrationBuilder.DropColumn(
                name: "IncludeParts",
                schema: "dbo",
                table: "SaleCode");

            migrationBuilder.DropColumn(
                name: "MaximumCharge",
                schema: "dbo",
                table: "SaleCode");

            migrationBuilder.DropColumn(
                name: "MinimumCharge",
                schema: "dbo",
                table: "SaleCode");

            migrationBuilder.DropColumn(
                name: "MinimumJobAmount",
                schema: "dbo",
                table: "SaleCode");

            migrationBuilder.DropColumn(
                name: "Percentage",
                schema: "dbo",
                table: "SaleCode");

            migrationBuilder.AddColumn<long>(
                name: "ShopSuppliesId",
                schema: "dbo",
                table: "SaleCode",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SaleCodeShopSupplies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Percentage = table.Column<double>(type: "float", nullable: false),
                    MinimumJobAmount = table.Column<double>(type: "float", nullable: false),
                    MinimumCharge = table.Column<double>(type: "float", nullable: false),
                    MaximumCharge = table.Column<double>(type: "float", nullable: false),
                    IncludeParts = table.Column<bool>(type: "bit", nullable: false),
                    IncludeLabor = table.Column<bool>(type: "bit", nullable: false),
                    TrackingState = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleCodeShopSupplies", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SaleCode_ShopSuppliesId",
                schema: "dbo",
                table: "SaleCode",
                column: "ShopSuppliesId");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleCode_SaleCodeShopSupplies_ShopSuppliesId",
                schema: "dbo",
                table: "SaleCode",
                column: "ShopSuppliesId",
                principalTable: "SaleCodeShopSupplies",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleCode_SaleCodeShopSupplies_ShopSuppliesId",
                schema: "dbo",
                table: "SaleCode");

            migrationBuilder.DropTable(
                name: "SaleCodeShopSupplies");

            migrationBuilder.DropIndex(
                name: "IX_SaleCode_ShopSuppliesId",
                schema: "dbo",
                table: "SaleCode");

            migrationBuilder.DropColumn(
                name: "ShopSuppliesId",
                schema: "dbo",
                table: "SaleCode");

            migrationBuilder.AddColumn<bool>(
                name: "IncludeLabor",
                schema: "dbo",
                table: "SaleCode",
                type: "bit",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IncludeParts",
                schema: "dbo",
                table: "SaleCode",
                type: "bit",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "MaximumCharge",
                schema: "dbo",
                table: "SaleCode",
                type: "float",
                nullable: true,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MinimumCharge",
                schema: "dbo",
                table: "SaleCode",
                type: "float",
                nullable: true,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MinimumJobAmount",
                schema: "dbo",
                table: "SaleCode",
                type: "float",
                nullable: true,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Percentage",
                schema: "dbo",
                table: "SaleCode",
                type: "float",
                nullable: true,
                defaultValue: 0.0);
        }
    }
}
