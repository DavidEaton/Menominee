using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class Packages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PackageId",
                schema: "dbo",
                table: "InventoryItem",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InventoryItemPackage",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BasePartsAmount = table.Column<double>(type: "float", nullable: false),
                    BaseLaborAmount = table.Column<double>(type: "float", nullable: false),
                    Script = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDiscountable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItemPackage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItemPackageItem",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    PartAmountIsAdditional = table.Column<bool>(type: "bit", nullable: false),
                    LaborAmountIsAdditional = table.Column<bool>(type: "bit", nullable: false),
                    ExciseFeeIsAdditional = table.Column<bool>(type: "bit", nullable: false),
                    InventoryItemPackageId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItemPackageItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryItemPackageItem_InventoryItem_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "dbo",
                        principalTable: "InventoryItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InventoryItemPackageItem_InventoryItemPackage_InventoryItemPackageId",
                        column: x => x.InventoryItemPackageId,
                        principalSchema: "dbo",
                        principalTable: "InventoryItemPackage",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InventoryItemPackagePlaceholder",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ItemType = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    PartAmountIsAdditional = table.Column<bool>(type: "bit", nullable: false),
                    LaborAmountIsAdditional = table.Column<bool>(type: "bit", nullable: false),
                    ExciseFeeIsAdditional = table.Column<bool>(type: "bit", nullable: false),
                    InventoryItemPackageId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItemPackagePlaceholder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryItemPackagePlaceholder_InventoryItemPackage_InventoryItemPackageId",
                        column: x => x.InventoryItemPackageId,
                        principalSchema: "dbo",
                        principalTable: "InventoryItemPackage",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_PackageId",
                schema: "dbo",
                table: "InventoryItem",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItemPackageItem_InventoryItemPackageId",
                schema: "dbo",
                table: "InventoryItemPackageItem",
                column: "InventoryItemPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItemPackageItem_ItemId",
                schema: "dbo",
                table: "InventoryItemPackageItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItemPackagePlaceholder_InventoryItemPackageId",
                schema: "dbo",
                table: "InventoryItemPackagePlaceholder",
                column: "InventoryItemPackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItem_InventoryItemPackage_PackageId",
                schema: "dbo",
                table: "InventoryItem",
                column: "PackageId",
                principalSchema: "dbo",
                principalTable: "InventoryItemPackage",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItem_InventoryItemPackage_PackageId",
                schema: "dbo",
                table: "InventoryItem");

            migrationBuilder.DropTable(
                name: "InventoryItemPackageItem",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "InventoryItemPackagePlaceholder",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "InventoryItemPackage",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItem_PackageId",
                schema: "dbo",
                table: "InventoryItem");

            migrationBuilder.DropColumn(
                name: "PackageId",
                schema: "dbo",
                table: "InventoryItem");
        }
    }
}
