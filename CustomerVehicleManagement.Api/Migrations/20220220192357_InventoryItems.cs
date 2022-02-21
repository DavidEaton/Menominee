using Microsoft.EntityFrameworkCore.Migrations;

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class InventoryItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InventoryItem",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ManufacturerId = table.Column<long>(type: "bigint", nullable: false),
                    PartNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductCodeId = table.Column<long>(type: "bigint", nullable: false),
                    PartType = table.Column<int>(type: "int", nullable: false),
                    QuantityOnHand = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<double>(type: "float", nullable: false),
                    SuggestedPrice = table.Column<double>(type: "float", nullable: false),
                    Labor = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryItem_Manufacturer_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalSchema: "dbo",
                        principalTable: "Manufacturer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryItem_ProductCode_ProductCodeId",
                        column: x => x.ProductCodeId,
                        principalSchema: "dbo",
                        principalTable: "ProductCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_ManufacturerId",
                schema: "dbo",
                table: "InventoryItem",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_ProductCodeId",
                schema: "dbo",
                table: "InventoryItem",
                column: "ProductCodeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryItem",
                schema: "dbo");
        }
    }
}
