using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class Taxes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExciseFee",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FeeType = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExciseFee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalesTax",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaxType = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsAppliedByDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsTaxable = table.Column<bool>(type: "bit", nullable: false),
                    TaxIdNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartTaxRate = table.Column<double>(type: "float", nullable: false),
                    LaborTaxRate = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesTax", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalesTaxTaxableExciseFee",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalesTaxId = table.Column<long>(type: "bigint", nullable: true),
                    ExciseFeeId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesTaxTaxableExciseFee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesTaxTaxableExciseFee_ExciseFee_ExciseFeeId",
                        column: x => x.ExciseFeeId,
                        principalSchema: "dbo",
                        principalTable: "ExciseFee",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesTaxTaxableExciseFee_SalesTax_SalesTaxId",
                        column: x => x.SalesTaxId,
                        principalSchema: "dbo",
                        principalTable: "SalesTax",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesTaxTaxableExciseFee_ExciseFeeId",
                schema: "dbo",
                table: "SalesTaxTaxableExciseFee",
                column: "ExciseFeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesTaxTaxableExciseFee_SalesTaxId",
                schema: "dbo",
                table: "SalesTaxTaxableExciseFee",
                column: "SalesTaxId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesTaxTaxableExciseFee",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ExciseFee",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SalesTax",
                schema: "dbo");
        }
    }
}
