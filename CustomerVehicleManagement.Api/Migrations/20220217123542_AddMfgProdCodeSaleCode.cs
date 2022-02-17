using Microsoft.EntityFrameworkCore.Migrations;

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class AddMfgProdCodeSaleCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductCode",
                schema: "dbo",
                table: "RepairOrderItems");

            migrationBuilder.DropColumn(
                name: "SaleCode",
                schema: "dbo",
                table: "RepairOrderItems");

            migrationBuilder.AlterColumn<long>(
                name: "ManufacturerId",
                schema: "dbo",
                table: "RepairOrderItems",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DiscountEach",
                schema: "dbo",
                table: "RepairOrderItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "DiscountType",
                schema: "dbo",
                table: "RepairOrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LaborType",
                schema: "dbo",
                table: "RepairOrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "ProductCodeId",
                schema: "dbo",
                table: "RepairOrderItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SaleCodeId",
                schema: "dbo",
                table: "RepairOrderItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Manufacturer",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SaleCode",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleCode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductCode",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ManufacturerId = table.Column<long>(type: "bigint", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SaleCodeId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCode_Manufacturer_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalSchema: "dbo",
                        principalTable: "Manufacturer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductCode_SaleCode_SaleCodeId",
                        column: x => x.SaleCodeId,
                        principalSchema: "dbo",
                        principalTable: "SaleCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderItems_ManufacturerId",
                schema: "dbo",
                table: "RepairOrderItems",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderItems_ProductCodeId",
                schema: "dbo",
                table: "RepairOrderItems",
                column: "ProductCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderItems_SaleCodeId",
                schema: "dbo",
                table: "RepairOrderItems",
                column: "SaleCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCode_ManufacturerId",
                schema: "dbo",
                table: "ProductCode",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCode_SaleCodeId",
                schema: "dbo",
                table: "ProductCode",
                column: "SaleCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairOrderItems_Manufacturer_ManufacturerId",
                schema: "dbo",
                table: "RepairOrderItems",
                column: "ManufacturerId",
                principalSchema: "dbo",
                principalTable: "Manufacturer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairOrderItems_ProductCode_ProductCodeId",
                schema: "dbo",
                table: "RepairOrderItems",
                column: "ProductCodeId",
                principalSchema: "dbo",
                principalTable: "ProductCode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairOrderItems_SaleCode_SaleCodeId",
                schema: "dbo",
                table: "RepairOrderItems",
                column: "SaleCodeId",
                principalSchema: "dbo",
                principalTable: "SaleCode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepairOrderItems_Manufacturer_ManufacturerId",
                schema: "dbo",
                table: "RepairOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairOrderItems_ProductCode_ProductCodeId",
                schema: "dbo",
                table: "RepairOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairOrderItems_SaleCode_SaleCodeId",
                schema: "dbo",
                table: "RepairOrderItems");

            migrationBuilder.DropTable(
                name: "ProductCode",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Manufacturer",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SaleCode",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_RepairOrderItems_ManufacturerId",
                schema: "dbo",
                table: "RepairOrderItems");

            migrationBuilder.DropIndex(
                name: "IX_RepairOrderItems_ProductCodeId",
                schema: "dbo",
                table: "RepairOrderItems");

            migrationBuilder.DropIndex(
                name: "IX_RepairOrderItems_SaleCodeId",
                schema: "dbo",
                table: "RepairOrderItems");

            migrationBuilder.DropColumn(
                name: "DiscountEach",
                schema: "dbo",
                table: "RepairOrderItems");

            migrationBuilder.DropColumn(
                name: "DiscountType",
                schema: "dbo",
                table: "RepairOrderItems");

            migrationBuilder.DropColumn(
                name: "LaborType",
                schema: "dbo",
                table: "RepairOrderItems");

            migrationBuilder.DropColumn(
                name: "ProductCodeId",
                schema: "dbo",
                table: "RepairOrderItems");

            migrationBuilder.DropColumn(
                name: "SaleCodeId",
                schema: "dbo",
                table: "RepairOrderItems");

            migrationBuilder.AlterColumn<string>(
                name: "ManufacturerId",
                schema: "dbo",
                table: "RepairOrderItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductCode",
                schema: "dbo",
                table: "RepairOrderItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SaleCode",
                schema: "dbo",
                table: "RepairOrderItems",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
