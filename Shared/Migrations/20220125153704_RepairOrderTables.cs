using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MenomineePlayWASM.Shared.Migrations
{
    public partial class RepairOrderTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                schema: "dbo",
                table: "RepairOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateInvoiced",
                schema: "dbo",
                table: "RepairOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                schema: "dbo",
                table: "RepairOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "RepairOrderPayments",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepairOrderId = table.Column<long>(type: "bigint", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Amount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrderPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairOrderPayments_RepairOrders_RepairOrderId",
                        column: x => x.RepairOrderId,
                        principalSchema: "dbo",
                        principalTable: "RepairOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RepairOrderPurchases",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepairOrderItemId = table.Column<long>(type: "bigint", nullable: false),
                    VendorId = table.Column<long>(type: "bigint", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PONumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VendorInvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VendorPartNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrderPurchases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RepairOrderServices",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepairOrderId = table.Column<long>(type: "bigint", nullable: false),
                    SequenceNumber = table.Column<int>(type: "int", nullable: false),
                    ServiceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SaleCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCounterSale = table.Column<bool>(type: "bit", nullable: false),
                    IsDeclined = table.Column<bool>(type: "bit", nullable: false),
                    PartsTotal = table.Column<double>(type: "float", nullable: false),
                    LaborTotal = table.Column<double>(type: "float", nullable: false),
                    TaxTotal = table.Column<double>(type: "float", nullable: false),
                    ShopSuppliesTotal = table.Column<double>(type: "float", nullable: false),
                    Total = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrderServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairOrderServices_RepairOrders_RepairOrderId",
                        column: x => x.RepairOrderId,
                        principalSchema: "dbo",
                        principalTable: "RepairOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RepairOrderTaxes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepairOrderId = table.Column<long>(type: "bigint", nullable: false),
                    SequenceNumber = table.Column<int>(type: "int", nullable: false),
                    TaxId = table.Column<long>(type: "bigint", nullable: false),
                    PartTaxRate = table.Column<double>(type: "float", nullable: false),
                    LaborTaxRate = table.Column<double>(type: "float", nullable: false),
                    PartTax = table.Column<double>(type: "float", nullable: false),
                    LaborTax = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrderTaxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairOrderTaxes_RepairOrders_RepairOrderId",
                        column: x => x.RepairOrderId,
                        principalSchema: "dbo",
                        principalTable: "RepairOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RepairOrderItems",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepairOrderServiceId = table.Column<long>(type: "bigint", nullable: false),
                    SequenceNumber = table.Column<int>(type: "int", nullable: false),
                    ManufacturerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SaleCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SaleType = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    PartType = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsDeclined = table.Column<bool>(type: "bit", nullable: false),
                    IsCounterSale = table.Column<bool>(type: "bit", nullable: false),
                    QuantitySold = table.Column<double>(type: "float", nullable: false),
                    SellingPrice = table.Column<double>(type: "float", nullable: false),
                    LaborEach = table.Column<double>(type: "float", nullable: false),
                    Cost = table.Column<double>(type: "float", nullable: false),
                    Core = table.Column<double>(type: "float", nullable: false),
                    Total = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairOrderItems_RepairOrderServices_RepairOrderServiceId",
                        column: x => x.RepairOrderServiceId,
                        principalSchema: "dbo",
                        principalTable: "RepairOrderServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RepairOrderServiceTaxes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepairOrderServiceId = table.Column<long>(type: "bigint", nullable: false),
                    SequenceNumber = table.Column<int>(type: "int", nullable: false),
                    TaxId = table.Column<long>(type: "bigint", nullable: false),
                    PartTaxRate = table.Column<double>(type: "float", nullable: false),
                    LaborTaxRate = table.Column<double>(type: "float", nullable: false),
                    PartTax = table.Column<double>(type: "float", nullable: false),
                    LaborTax = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrderServiceTaxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairOrderServiceTaxes_RepairOrderServices_RepairOrderServiceId",
                        column: x => x.RepairOrderServiceId,
                        principalSchema: "dbo",
                        principalTable: "RepairOrderServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RepairOrderTechs",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepairOrderServiceId = table.Column<long>(type: "bigint", nullable: false),
                    TechnicianId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrderTechs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairOrderTechs_RepairOrderServices_RepairOrderServiceId",
                        column: x => x.RepairOrderServiceId,
                        principalSchema: "dbo",
                        principalTable: "RepairOrderServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RepairOrderItemTaxes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepairOrderItemId = table.Column<long>(type: "bigint", nullable: false),
                    SequenceNumber = table.Column<int>(type: "int", nullable: false),
                    TaxId = table.Column<long>(type: "bigint", nullable: false),
                    PartTaxRate = table.Column<double>(type: "float", nullable: false),
                    LaborTaxRate = table.Column<double>(type: "float", nullable: false),
                    PartTax = table.Column<double>(type: "float", nullable: false),
                    LaborTax = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrderItemTaxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairOrderItemTaxes_RepairOrderItems_RepairOrderItemId",
                        column: x => x.RepairOrderItemId,
                        principalSchema: "dbo",
                        principalTable: "RepairOrderItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RepairOrderSerialNumbers",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepairOrderItemId = table.Column<long>(type: "bigint", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrderSerialNumbers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairOrderSerialNumbers_RepairOrderItems_RepairOrderItemId",
                        column: x => x.RepairOrderItemId,
                        principalSchema: "dbo",
                        principalTable: "RepairOrderItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RepairOrderWarranties",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepairOrderItemId = table.Column<long>(type: "bigint", nullable: false),
                    SequenceNumber = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    NewWarranty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalWarranty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalInvoiceId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrderWarranties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairOrderWarranties_RepairOrderItems_RepairOrderItemId",
                        column: x => x.RepairOrderItemId,
                        principalSchema: "dbo",
                        principalTable: "RepairOrderItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderItems_RepairOrderServiceId",
                schema: "dbo",
                table: "RepairOrderItems",
                column: "RepairOrderServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderItemTaxes_RepairOrderItemId",
                schema: "dbo",
                table: "RepairOrderItemTaxes",
                column: "RepairOrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderPayments_RepairOrderId",
                schema: "dbo",
                table: "RepairOrderPayments",
                column: "RepairOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderSerialNumbers_RepairOrderItemId",
                schema: "dbo",
                table: "RepairOrderSerialNumbers",
                column: "RepairOrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderServices_RepairOrderId",
                schema: "dbo",
                table: "RepairOrderServices",
                column: "RepairOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderServiceTaxes_RepairOrderServiceId",
                schema: "dbo",
                table: "RepairOrderServiceTaxes",
                column: "RepairOrderServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderTaxes_RepairOrderId",
                schema: "dbo",
                table: "RepairOrderTaxes",
                column: "RepairOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderTechs_RepairOrderServiceId",
                schema: "dbo",
                table: "RepairOrderTechs",
                column: "RepairOrderServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderWarranties_RepairOrderItemId",
                schema: "dbo",
                table: "RepairOrderWarranties",
                column: "RepairOrderItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RepairOrderItemTaxes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RepairOrderPayments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RepairOrderPurchases",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RepairOrderSerialNumbers",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RepairOrderServiceTaxes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RepairOrderTaxes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RepairOrderTechs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RepairOrderWarranties",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RepairOrderItems",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RepairOrderServices",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                schema: "dbo",
                table: "RepairOrders");

            migrationBuilder.DropColumn(
                name: "DateInvoiced",
                schema: "dbo",
                table: "RepairOrders");

            migrationBuilder.DropColumn(
                name: "DateModified",
                schema: "dbo",
                table: "RepairOrders");
        }
    }
}
