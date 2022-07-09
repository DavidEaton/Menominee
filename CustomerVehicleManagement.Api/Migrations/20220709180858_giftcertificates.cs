using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class giftcertificates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InventoryItemCourtesyCheck",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventoryItemId = table.Column<long>(type: "bigint", nullable: false),
                    LaborType = table.Column<int>(type: "int", nullable: false),
                    LaborAmount = table.Column<double>(type: "float", nullable: false),
                    TechPayType = table.Column<int>(type: "int", nullable: false),
                    TechPayAmount = table.Column<double>(type: "float", nullable: false),
                    SkillLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItemCourtesyCheck", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryItemCourtesyCheck_InventoryItem_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalSchema: "dbo",
                        principalTable: "InventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItemCourtesyCheck_InventoryItemId",
                schema: "dbo",
                table: "InventoryItemCourtesyCheck",
                column: "InventoryItemId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryItemCourtesyCheck",
                schema: "dbo");
        }
    }
}
