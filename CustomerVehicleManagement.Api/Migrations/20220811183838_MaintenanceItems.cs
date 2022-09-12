using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class MaintenanceItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaintenanceItem",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayOrder = table.Column<long>(type: "bigint", nullable: false),
                    ItemId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceItem_InventoryItem_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "dbo",
                        principalTable: "InventoryItem",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceItem_ItemId",
                schema: "dbo",
                table: "MaintenanceItem",
                column: "ItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaintenanceItem",
                schema: "dbo");
        }
    }
}
