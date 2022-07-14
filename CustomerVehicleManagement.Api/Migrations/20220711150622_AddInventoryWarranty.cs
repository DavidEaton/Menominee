using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class AddInventoryWarranty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryItemCourtesyCheck",
                schema: "dbo");

            migrationBuilder.CreateTable(
                name: "InventoryItemDonation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventoryItemId = table.Column<long>(type: "bigint", nullable: false),
                    TrackingState = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItemDonation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryItemDonation_InventoryItem_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalSchema: "dbo",
                        principalTable: "InventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItemGiftCertificate",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventoryItemId = table.Column<long>(type: "bigint", nullable: false),
                    TrackingState = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItemGiftCertificate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryItemGiftCertificate_InventoryItem_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalSchema: "dbo",
                        principalTable: "InventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItemInspection",
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
                    SkillLevel = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItemInspection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryItemInspection_InventoryItem_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalSchema: "dbo",
                        principalTable: "InventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItemWarranties",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventoryItemId = table.Column<long>(type: "bigint", nullable: false),
                    PeriodType = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItemWarranties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryItemWarranties_InventoryItem_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalSchema: "dbo",
                        principalTable: "InventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItemDonation_InventoryItemId",
                table: "InventoryItemDonation",
                column: "InventoryItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItemGiftCertificate_InventoryItemId",
                table: "InventoryItemGiftCertificate",
                column: "InventoryItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItemInspection_InventoryItemId",
                schema: "dbo",
                table: "InventoryItemInspection",
                column: "InventoryItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItemWarranties_InventoryItemId",
                table: "InventoryItemWarranties",
                column: "InventoryItemId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryItemDonation");

            migrationBuilder.DropTable(
                name: "InventoryItemGiftCertificate");

            migrationBuilder.DropTable(
                name: "InventoryItemInspection",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "InventoryItemWarranties");

            migrationBuilder.CreateTable(
                name: "InventoryItemCourtesyCheck",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventoryItemId = table.Column<long>(type: "bigint", nullable: false),
                    LaborAmount = table.Column<double>(type: "float", nullable: false),
                    LaborType = table.Column<int>(type: "int", nullable: false),
                    SkillLevel = table.Column<int>(type: "int", nullable: false),
                    TechPayAmount = table.Column<double>(type: "float", nullable: false),
                    TechPayType = table.Column<int>(type: "int", nullable: false)
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
    }
}
