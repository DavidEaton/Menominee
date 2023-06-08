using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class RepairOrderItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepairOrderLineItem_Manufacturer_Item_ManufacturerId",
                schema: "dbo",
                table: "RepairOrderLineItem");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairOrderLineItem_ProductCode_Item_ProductCodeId",
                schema: "dbo",
                table: "RepairOrderLineItem");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairOrderLineItem_SaleCode_Item_SaleCodeId",
                schema: "dbo",
                table: "RepairOrderLineItem");

            migrationBuilder.DropIndex(
                name: "IX_RepairOrderLineItem_Item_ManufacturerId",
                schema: "dbo",
                table: "RepairOrderLineItem");

            migrationBuilder.DropIndex(
                name: "IX_RepairOrderLineItem_Item_ProductCodeId",
                schema: "dbo",
                table: "RepairOrderLineItem");

            migrationBuilder.DropColumn(
                name: "DiscountTotal",
                schema: "dbo",
                table: "RepairOrderService");

            migrationBuilder.DropColumn(
                name: "IsCounterSale",
                schema: "dbo",
                table: "RepairOrderService");

            migrationBuilder.DropColumn(
                name: "IsDeclined",
                schema: "dbo",
                table: "RepairOrderService");

            migrationBuilder.DropColumn(
                name: "LaborTotal",
                schema: "dbo",
                table: "RepairOrderService");

            migrationBuilder.DropColumn(
                name: "PartsTotal",
                schema: "dbo",
                table: "RepairOrderService");

            migrationBuilder.DropColumn(
                name: "TaxTotal",
                schema: "dbo",
                table: "RepairOrderService");

            migrationBuilder.DropColumn(
                name: "ItemDescription",
                schema: "dbo",
                table: "RepairOrderLineItem");

            migrationBuilder.DropColumn(
                name: "ItemPartNumber",
                schema: "dbo",
                table: "RepairOrderLineItem");

            migrationBuilder.DropColumn(
                name: "ItemPartType",
                schema: "dbo",
                table: "RepairOrderLineItem");

            migrationBuilder.DropColumn(
                name: "Item_ManufacturerId",
                schema: "dbo",
                table: "RepairOrderLineItem");

            migrationBuilder.DropColumn(
                name: "Item_ProductCodeId",
                schema: "dbo",
                table: "RepairOrderLineItem");

            migrationBuilder.DropColumn(
                name: "Total",
                schema: "dbo",
                table: "RepairOrderLineItem");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                schema: "dbo",
                table: "RepairOrder");

            migrationBuilder.DropColumn(
                name: "DateInvoiced",
                schema: "dbo",
                table: "RepairOrder");

            migrationBuilder.DropColumn(
                name: "DiscountTotal",
                schema: "dbo",
                table: "RepairOrder");

            migrationBuilder.DropColumn(
                name: "HazMatTotal",
                schema: "dbo",
                table: "RepairOrder");

            migrationBuilder.DropColumn(
                name: "LaborTotal",
                schema: "dbo",
                table: "RepairOrder");

            migrationBuilder.DropColumn(
                name: "PartsTotal",
                schema: "dbo",
                table: "RepairOrder");

            migrationBuilder.DropColumn(
                name: "ShopSuppliesTotal",
                schema: "dbo",
                table: "RepairOrder");

            migrationBuilder.DropColumn(
                name: "TaxTotal",
                schema: "dbo",
                table: "RepairOrder");

            migrationBuilder.DropColumn(
                name: "Total",
                schema: "dbo",
                table: "RepairOrder");

            migrationBuilder.DropColumn(
                name: "Vehicle",
                schema: "dbo",
                table: "RepairOrder");

            migrationBuilder.RenameColumn(
                name: "Total",
                schema: "dbo",
                table: "RepairOrderService",
                newName: "HazMatTotal");

            migrationBuilder.RenameColumn(
                name: "Item_SaleCodeId",
                schema: "dbo",
                table: "RepairOrderLineItem",
                newName: "ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_RepairOrderLineItem_Item_SaleCodeId",
                schema: "dbo",
                table: "RepairOrderLineItem",
                newName: "IX_RepairOrderLineItem_ItemId");

            migrationBuilder.AlterColumn<int>(
                name: "Year",
                schema: "dbo",
                table: "Vehicle",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "NonTraditionalVehicle",
                schema: "dbo",
                table: "Vehicle",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "CustomerId",
                schema: "dbo",
                table: "RepairOrder",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "VehicleId",
                schema: "dbo",
                table: "RepairOrder",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Company",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: true),
                    NextInvoiceNumberOrSeed = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Company_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalSchema: "dbo",
                        principalTable: "Organization",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RepairOrderItemLabor",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LaborAmount_PayType = table.Column<int>(type: "int", nullable: true),
                    LaborAmount = table.Column<double>(type: "float", nullable: true),
                    TechAmount_SkillLevel = table.Column<int>(type: "int", nullable: true),
                    TechAmount_PayType = table.Column<int>(type: "int", nullable: true),
                    TechAmount = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrderItemLabor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RepairOrderItemPart",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    List = table.Column<double>(type: "float", nullable: false),
                    Cost = table.Column<double>(type: "float", nullable: false),
                    Core = table.Column<double>(type: "float", nullable: false),
                    Retail = table.Column<double>(type: "float", nullable: false),
                    TechAmount_SkillLevel = table.Column<int>(type: "int", nullable: true),
                    TechAmount_PayType = table.Column<int>(type: "int", nullable: true),
                    TechAmount = table.Column<double>(type: "float", nullable: true),
                    LineCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubLineCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fractional = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrderItemPart", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RepairOrderItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ManufacturerId = table.Column<long>(type: "bigint", nullable: true),
                    PartNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SaleCodeId = table.Column<long>(type: "bigint", nullable: true),
                    ProductCodeId = table.Column<long>(type: "bigint", nullable: true),
                    PartType = table.Column<int>(type: "int", nullable: false),
                    PartId = table.Column<long>(type: "bigint", nullable: true),
                    LaborId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairOrderItems_Manufacturer_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalSchema: "dbo",
                        principalTable: "Manufacturer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RepairOrderItems_ProductCode_ProductCodeId",
                        column: x => x.ProductCodeId,
                        principalSchema: "dbo",
                        principalTable: "ProductCode",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RepairOrderItems_RepairOrderItemLabor_LaborId",
                        column: x => x.LaborId,
                        principalSchema: "dbo",
                        principalTable: "RepairOrderItemLabor",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RepairOrderItems_RepairOrderItemPart_PartId",
                        column: x => x.PartId,
                        principalSchema: "dbo",
                        principalTable: "RepairOrderItemPart",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RepairOrderItems_SaleCode_SaleCodeId",
                        column: x => x.SaleCodeId,
                        principalSchema: "dbo",
                        principalTable: "SaleCode",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrder_CustomerId",
                schema: "dbo",
                table: "RepairOrder",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrder_VehicleId",
                schema: "dbo",
                table: "RepairOrder",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_OrganizationId",
                schema: "dbo",
                table: "Company",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderItems_LaborId",
                table: "RepairOrderItems",
                column: "LaborId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderItems_ManufacturerId",
                table: "RepairOrderItems",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderItems_PartId",
                table: "RepairOrderItems",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderItems_ProductCodeId",
                table: "RepairOrderItems",
                column: "ProductCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderItems_SaleCodeId",
                table: "RepairOrderItems",
                column: "SaleCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairOrder_Customer_CustomerId",
                schema: "dbo",
                table: "RepairOrder",
                column: "CustomerId",
                principalSchema: "dbo",
                principalTable: "Customer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairOrder_Vehicle_VehicleId",
                schema: "dbo",
                table: "RepairOrder",
                column: "VehicleId",
                principalSchema: "dbo",
                principalTable: "Vehicle",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairOrderLineItem_RepairOrderItems_ItemId",
                schema: "dbo",
                table: "RepairOrderLineItem",
                column: "ItemId",
                principalTable: "RepairOrderItems",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepairOrder_Customer_CustomerId",
                schema: "dbo",
                table: "RepairOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairOrder_Vehicle_VehicleId",
                schema: "dbo",
                table: "RepairOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairOrderLineItem_RepairOrderItems_ItemId",
                schema: "dbo",
                table: "RepairOrderLineItem");

            migrationBuilder.DropTable(
                name: "Company",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RepairOrderItems");

            migrationBuilder.DropTable(
                name: "RepairOrderItemLabor",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RepairOrderItemPart",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_RepairOrder_CustomerId",
                schema: "dbo",
                table: "RepairOrder");

            migrationBuilder.DropIndex(
                name: "IX_RepairOrder_VehicleId",
                schema: "dbo",
                table: "RepairOrder");

            migrationBuilder.DropColumn(
                name: "NonTraditionalVehicle",
                schema: "dbo",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                schema: "dbo",
                table: "RepairOrder");

            migrationBuilder.DropColumn(
                name: "VehicleId",
                schema: "dbo",
                table: "RepairOrder");

            migrationBuilder.RenameColumn(
                name: "HazMatTotal",
                schema: "dbo",
                table: "RepairOrderService",
                newName: "Total");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                schema: "dbo",
                table: "RepairOrderLineItem",
                newName: "Item_SaleCodeId");

            migrationBuilder.RenameIndex(
                name: "IX_RepairOrderLineItem_ItemId",
                schema: "dbo",
                table: "RepairOrderLineItem",
                newName: "IX_RepairOrderLineItem_Item_SaleCodeId");

            migrationBuilder.AlterColumn<int>(
                name: "Year",
                schema: "dbo",
                table: "Vehicle",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DiscountTotal",
                schema: "dbo",
                table: "RepairOrderService",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "IsCounterSale",
                schema: "dbo",
                table: "RepairOrderService",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeclined",
                schema: "dbo",
                table: "RepairOrderService",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "LaborTotal",
                schema: "dbo",
                table: "RepairOrderService",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PartsTotal",
                schema: "dbo",
                table: "RepairOrderService",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TaxTotal",
                schema: "dbo",
                table: "RepairOrderService",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "ItemDescription",
                schema: "dbo",
                table: "RepairOrderLineItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemPartNumber",
                schema: "dbo",
                table: "RepairOrderLineItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemPartType",
                schema: "dbo",
                table: "RepairOrderLineItem",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Item_ManufacturerId",
                schema: "dbo",
                table: "RepairOrderLineItem",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Item_ProductCodeId",
                schema: "dbo",
                table: "RepairOrderLineItem",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Total",
                schema: "dbo",
                table: "RepairOrderLineItem",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                schema: "dbo",
                table: "RepairOrder",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateInvoiced",
                schema: "dbo",
                table: "RepairOrder",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "DiscountTotal",
                schema: "dbo",
                table: "RepairOrder",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HazMatTotal",
                schema: "dbo",
                table: "RepairOrder",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LaborTotal",
                schema: "dbo",
                table: "RepairOrder",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PartsTotal",
                schema: "dbo",
                table: "RepairOrder",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ShopSuppliesTotal",
                schema: "dbo",
                table: "RepairOrder",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TaxTotal",
                schema: "dbo",
                table: "RepairOrder",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Total",
                schema: "dbo",
                table: "RepairOrder",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Vehicle",
                schema: "dbo",
                table: "RepairOrder",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderLineItem_Item_ManufacturerId",
                schema: "dbo",
                table: "RepairOrderLineItem",
                column: "Item_ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderLineItem_Item_ProductCodeId",
                schema: "dbo",
                table: "RepairOrderLineItem",
                column: "Item_ProductCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairOrderLineItem_Manufacturer_Item_ManufacturerId",
                schema: "dbo",
                table: "RepairOrderLineItem",
                column: "Item_ManufacturerId",
                principalSchema: "dbo",
                principalTable: "Manufacturer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairOrderLineItem_ProductCode_Item_ProductCodeId",
                schema: "dbo",
                table: "RepairOrderLineItem",
                column: "Item_ProductCodeId",
                principalSchema: "dbo",
                principalTable: "ProductCode",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairOrderLineItem_SaleCode_Item_SaleCodeId",
                schema: "dbo",
                table: "RepairOrderLineItem",
                column: "Item_SaleCodeId",
                principalSchema: "dbo",
                principalTable: "SaleCode",
                principalColumn: "Id");
        }
    }
}
