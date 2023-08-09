using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Menominee.Api.Migrations
{
    public partial class UpdateExciseFee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExciseFee",
                schema: "dbo");

            migrationBuilder.CreateTable(
                name: "ExciseFee",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                    FeeType = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Amount = table.Column<double>(type: "float", nullable: false, defaultValue: 0.0),
                    InventoryItemPartId = table.Column<long>(type: "bigint", nullable: true),
                    InventoryItemTireId = table.Column<long>(type: "bigint", nullable: true),
                    RepairOrderItemPartId = table.Column<long>(type: "bigint", nullable: true),
                    SalesTaxId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExciseFee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExciseFee_InventoryItemPart_InventoryItemPartId",
                        column: x => x.InventoryItemPartId,
                        principalSchema: "dbo",
                        principalTable: "InventoryItemPart",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExciseFee_InventoryItemTire_InventoryItemTireId",
                        column: x => x.InventoryItemTireId,
                        principalSchema: "dbo",
                        principalTable: "InventoryItemTire",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExciseFee_RepairOrderItemPart_RepairOrderItemPartId",
                        column: x => x.RepairOrderItemPartId,
                        principalSchema: "dbo",
                        principalTable: "RepairOrderItemPart",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExciseFee_SalesTax_SalesTaxId",
                        column: x => x.SalesTaxId,
                        principalSchema: "dbo",
                        principalTable: "SalesTax",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExciseFee_InventoryItemPartId",
                schema: "dbo",
                table: "ExciseFee",
                column: "InventoryItemPartId");

            migrationBuilder.CreateIndex(
                name: "IX_ExciseFee_InventoryItemTireId",
                schema: "dbo",
                table: "ExciseFee",
                column: "InventoryItemTireId");

            migrationBuilder.CreateIndex(
                name: "IX_ExciseFee_RepairOrderItemPartId",
                schema: "dbo",
                table: "ExciseFee",
                column: "RepairOrderItemPartId");

            migrationBuilder.CreateIndex(
                name: "IX_ExciseFee_SalesTaxId",
                schema: "dbo",
                table: "ExciseFee",
                column: "SalesTaxId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExciseFee",
                schema: "dbo");
        }
    }
}
