using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Menominee.Api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "CreditCard",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FeeType = table.Column<int>(type: "int", nullable: false),
                    Fee = table.Column<double>(type: "float", nullable: false),
                    IsAddedToDeposit = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCard", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItemInspection",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LaborPayType = table.Column<int>(type: "int", nullable: true),
                    LaborPayAmount = table.Column<double>(type: "float", nullable: true),
                    TechSkillLevel = table.Column<int>(type: "int", nullable: true),
                    TechPayType = table.Column<int>(type: "int", nullable: true),
                    TechAmount_Amount = table.Column<double>(type: "float", nullable: true),
                    InspectionType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItemInspection", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItemLabor",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LaborPayType = table.Column<int>(type: "int", nullable: true),
                    LaborPayAmount = table.Column<double>(type: "float", nullable: true),
                    TechSkillLevel = table.Column<int>(type: "int", nullable: true),
                    TechPayType = table.Column<int>(type: "int", nullable: true),
                    TechAmount_Amount = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItemLabor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItemPackage",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BasePartsAmount = table.Column<double>(type: "float", nullable: false),
                    BaseLaborAmount = table.Column<double>(type: "float", nullable: false),
                    Script = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: true),
                    IsDiscountable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItemPackage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItemPart",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    List = table.Column<double>(type: "float", nullable: false),
                    Cost = table.Column<double>(type: "float", nullable: false),
                    Core = table.Column<double>(type: "float", nullable: false),
                    Retail = table.Column<double>(type: "float", nullable: false),
                    TechSkillLevel = table.Column<int>(type: "int", nullable: true),
                    TechPayType = table.Column<int>(type: "int", nullable: true),
                    TechPayAmount = table.Column<double>(type: "float", nullable: true),
                    LineCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SubLineCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Fractional = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItemPart", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItemTire",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    Width = table.Column<int>(type: "int", nullable: false),
                    AspectRatio = table.Column<int>(type: "int", nullable: false),
                    ConstructionType = table.Column<int>(type: "int", nullable: false),
                    Diameter = table.Column<double>(type: "float", nullable: false),
                    LoadIndex = table.Column<int>(type: "int", nullable: false),
                    SpeedRating = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    List = table.Column<double>(type: "float", nullable: false),
                    Cost = table.Column<double>(type: "float", nullable: false),
                    Core = table.Column<double>(type: "float", nullable: false),
                    Retail = table.Column<double>(type: "float", nullable: false),
                    TechSkillLevel = table.Column<int>(type: "int", nullable: true),
                    TechPayType = table.Column<int>(type: "int", nullable: true),
                    TechPayAmount = table.Column<double>(type: "float", nullable: true),
                    LineCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SubLineCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Fractional = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItemTire", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItemWarranty",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PeriodType = table.Column<int>(type: "int", nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItemWarranty", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Manufacturer",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Prefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RepairOrder",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepairOrderNumber = table.Column<long>(type: "bigint", nullable: false),
                    InvoiceNumber = table.Column<long>(type: "bigint", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vehicle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartsTotal = table.Column<double>(type: "float", nullable: false),
                    LaborTotal = table.Column<double>(type: "float", nullable: false),
                    DiscountTotal = table.Column<double>(type: "float", nullable: false),
                    TaxTotal = table.Column<double>(type: "float", nullable: false),
                    HazMatTotal = table.Column<double>(type: "float", nullable: false),
                    ShopSuppliesTotal = table.Column<double>(type: "float", nullable: false),
                    Total = table.Column<double>(type: "float", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateInvoiced = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountingDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SaleCodeShopSupplies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Percentage = table.Column<double>(type: "float", nullable: false),
                    MinimumJobAmount = table.Column<double>(type: "float", nullable: false),
                    MinimumCharge = table.Column<double>(type: "float", nullable: false),
                    MaximumCharge = table.Column<double>(type: "float", nullable: false),
                    IncludeParts = table.Column<bool>(type: "bit", nullable: false),
                    IncludeLabor = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleCodeShopSupplies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalesTax",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaxType = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsAppliedByDefault = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    IsTaxable = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    TaxIdNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PartTaxRate = table.Column<double>(type: "float", nullable: false, defaultValue: 0.0),
                    LaborTaxRate = table.Column<double>(type: "float", nullable: false, defaultValue: 0.0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesTax", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItemPackagePlaceholder",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemType = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: true),
                    PartAmountIsAdditional = table.Column<bool>(type: "bit", nullable: true),
                    LaborAmountIsAdditional = table.Column<bool>(type: "bit", nullable: true),
                    ExciseFeeIsAdditional = table.Column<bool>(type: "bit", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "RepairOrderPayment",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    RepairOrderId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrderPayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairOrderPayment_RepairOrder_RepairOrderId",
                        column: x => x.RepairOrderId,
                        principalSchema: "dbo",
                        principalTable: "RepairOrder",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RepairOrderStatus",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RepairOrderId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrderStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairOrderStatus_RepairOrder_RepairOrderId",
                        column: x => x.RepairOrderId,
                        principalSchema: "dbo",
                        principalTable: "RepairOrder",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RepairOrderTax",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartTaxRate = table.Column<double>(type: "float", nullable: true),
                    PartTaxAmount = table.Column<double>(type: "float", nullable: true),
                    LaborTaxRate = table.Column<double>(type: "float", nullable: true),
                    LaborTaxAmount = table.Column<double>(type: "float", nullable: true),
                    RepairOrderId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrderTax", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairOrderTax_RepairOrder_RepairOrderId",
                        column: x => x.RepairOrderId,
                        principalSchema: "dbo",
                        principalTable: "RepairOrder",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SaleCode",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LaborRate = table.Column<double>(type: "float", nullable: false, defaultValue: 0.0),
                    DesiredMargin = table.Column<double>(type: "float", nullable: false, defaultValue: 0.0),
                    ShopSuppliesId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleCode_SaleCodeShopSupplies_ShopSuppliesId",
                        column: x => x.ShopSuppliesId,
                        principalTable: "SaleCodeShopSupplies",
                        principalColumn: "Id");
                });

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
                    SalesTaxId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExciseFee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExciseFee_SalesTax_SalesTaxId",
                        column: x => x.SalesTaxId,
                        principalSchema: "dbo",
                        principalTable: "SalesTax",
                        principalColumn: "Id");
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
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductCode_SaleCode_SaleCodeId",
                        column: x => x.SaleCodeId,
                        principalSchema: "dbo",
                        principalTable: "SaleCode",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RepairOrderService",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SaleCodeId = table.Column<long>(type: "bigint", nullable: true),
                    IsCounterSale = table.Column<bool>(type: "bit", nullable: false),
                    IsDeclined = table.Column<bool>(type: "bit", nullable: false),
                    PartsTotal = table.Column<double>(type: "float", nullable: false),
                    LaborTotal = table.Column<double>(type: "float", nullable: false),
                    DiscountTotal = table.Column<double>(type: "float", nullable: false),
                    TaxTotal = table.Column<double>(type: "float", nullable: false),
                    ShopSuppliesTotal = table.Column<double>(type: "float", nullable: false),
                    Total = table.Column<double>(type: "float", nullable: false),
                    RepairOrderId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrderService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairOrderService_RepairOrder_RepairOrderId",
                        column: x => x.RepairOrderId,
                        principalSchema: "dbo",
                        principalTable: "RepairOrder",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RepairOrderService_SaleCode_SaleCodeId",
                        column: x => x.SaleCodeId,
                        principalSchema: "dbo",
                        principalTable: "SaleCode",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InventoryItem",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ManufacturerId = table.Column<long>(type: "bigint", nullable: true),
                    ItemNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ProductCodeId = table.Column<long>(type: "bigint", nullable: true),
                    ItemType = table.Column<int>(type: "int", nullable: false),
                    PartId = table.Column<long>(type: "bigint", nullable: true),
                    LaborId = table.Column<long>(type: "bigint", nullable: true),
                    TireId = table.Column<long>(type: "bigint", nullable: true),
                    PackageId = table.Column<long>(type: "bigint", nullable: true),
                    InspectionId = table.Column<long>(type: "bigint", nullable: true),
                    WarrantyId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryItem_InventoryItemInspection_InspectionId",
                        column: x => x.InspectionId,
                        principalSchema: "dbo",
                        principalTable: "InventoryItemInspection",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InventoryItem_InventoryItemLabor_LaborId",
                        column: x => x.LaborId,
                        principalSchema: "dbo",
                        principalTable: "InventoryItemLabor",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InventoryItem_InventoryItemPackage_PackageId",
                        column: x => x.PackageId,
                        principalSchema: "dbo",
                        principalTable: "InventoryItemPackage",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InventoryItem_InventoryItemPart_PartId",
                        column: x => x.PartId,
                        principalSchema: "dbo",
                        principalTable: "InventoryItemPart",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InventoryItem_InventoryItemTire_TireId",
                        column: x => x.TireId,
                        principalSchema: "dbo",
                        principalTable: "InventoryItemTire",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InventoryItem_InventoryItemWarranty_WarrantyId",
                        column: x => x.WarrantyId,
                        principalSchema: "dbo",
                        principalTable: "InventoryItemWarranty",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InventoryItem_Manufacturer_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalSchema: "dbo",
                        principalTable: "Manufacturer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InventoryItem_ProductCode_ProductCodeId",
                        column: x => x.ProductCodeId,
                        principalSchema: "dbo",
                        principalTable: "ProductCode",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RepairOrderLineItem",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Item_ManufacturerId = table.Column<long>(type: "bigint", nullable: true),
                    ItemPartNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Item_SaleCodeId = table.Column<long>(type: "bigint", nullable: true),
                    Item_ProductCodeId = table.Column<long>(type: "bigint", nullable: true),
                    ItemPartType = table.Column<int>(type: "int", nullable: true),
                    SaleType = table.Column<int>(type: "int", nullable: false),
                    IsDeclined = table.Column<bool>(type: "bit", nullable: false),
                    IsCounterSale = table.Column<bool>(type: "bit", nullable: false),
                    QuantitySold = table.Column<double>(type: "float", nullable: false),
                    SellingPrice = table.Column<double>(type: "float", nullable: false),
                    LaborPayType = table.Column<int>(type: "int", nullable: true),
                    LaborAmount = table.Column<double>(type: "float", nullable: true),
                    Cost = table.Column<double>(type: "float", nullable: false),
                    Core = table.Column<double>(type: "float", nullable: false),
                    DiscountType = table.Column<int>(type: "int", nullable: true),
                    DiscountAmount = table.Column<double>(type: "float", nullable: true),
                    Total = table.Column<double>(type: "float", nullable: false),
                    RepairOrderServiceId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrderLineItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairOrderLineItem_Manufacturer_Item_ManufacturerId",
                        column: x => x.Item_ManufacturerId,
                        principalSchema: "dbo",
                        principalTable: "Manufacturer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RepairOrderLineItem_ProductCode_Item_ProductCodeId",
                        column: x => x.Item_ProductCodeId,
                        principalSchema: "dbo",
                        principalTable: "ProductCode",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RepairOrderLineItem_RepairOrderService_RepairOrderServiceId",
                        column: x => x.RepairOrderServiceId,
                        principalSchema: "dbo",
                        principalTable: "RepairOrderService",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RepairOrderLineItem_SaleCode_Item_SaleCodeId",
                        column: x => x.Item_SaleCodeId,
                        principalSchema: "dbo",
                        principalTable: "SaleCode",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RepairOrderServiceTax",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartTaxRate = table.Column<double>(type: "float", nullable: true),
                    PartTaxAmount = table.Column<double>(type: "float", nullable: true),
                    LaborTaxRate = table.Column<double>(type: "float", nullable: true),
                    LaborTaxAmount = table.Column<double>(type: "float", nullable: true),
                    RepairOrderServiceId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrderServiceTax", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairOrderServiceTax_RepairOrderService_RepairOrderServiceId",
                        column: x => x.RepairOrderServiceId,
                        principalSchema: "dbo",
                        principalTable: "RepairOrderService",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InventoryItemPackageItem",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: true),
                    PartAmountIsAdditional = table.Column<bool>(type: "bit", nullable: true),
                    LaborAmountIsAdditional = table.Column<bool>(type: "bit", nullable: true),
                    ExciseFeeIsAdditional = table.Column<bool>(type: "bit", nullable: true),
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryItemPackageItem_InventoryItemPackage_InventoryItemPackageId",
                        column: x => x.InventoryItemPackageId,
                        principalSchema: "dbo",
                        principalTable: "InventoryItemPackage",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceItem",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    InventoryItemId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceItem_InventoryItem_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalSchema: "dbo",
                        principalTable: "InventoryItem",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RepairOrderItemTax",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartTaxRate = table.Column<double>(type: "float", nullable: true),
                    PartTaxAmount = table.Column<double>(type: "float", nullable: true),
                    LaborTaxRate = table.Column<double>(type: "float", nullable: true),
                    LaborTaxAmount = table.Column<double>(type: "float", nullable: true),
                    RepairOrderLineItemId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrderItemTax", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairOrderItemTax_RepairOrderLineItem_RepairOrderLineItemId",
                        column: x => x.RepairOrderLineItemId,
                        principalSchema: "dbo",
                        principalTable: "RepairOrderLineItem",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RepairOrderSerialNumber",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerialNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RepairOrderLineItemId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrderSerialNumber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairOrderSerialNumber_RepairOrderLineItem_RepairOrderLineItemId",
                        column: x => x.RepairOrderLineItemId,
                        principalSchema: "dbo",
                        principalTable: "RepairOrderLineItem",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RepairOrderWarranty",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    NewWarranty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalWarranty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalInvoiceId = table.Column<long>(type: "bigint", nullable: false),
                    RepairOrderLineItemId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrderWarranty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairOrderWarranty_RepairOrderLineItem_RepairOrderLineItemId",
                        column: x => x.RepairOrderLineItemId,
                        principalSchema: "dbo",
                        principalTable: "RepairOrderLineItem",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: true),
                    CustomerType = table.Column<int>(type: "int", nullable: false),
                    AllowMail = table.Column<bool>(type: "bit", nullable: true),
                    AllowEmail = table.Column<bool>(type: "bit", nullable: true),
                    AllowSms = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicle",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VIN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Make = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicle_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "dbo",
                        principalTable: "Customer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Email",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: true),
                    PersonId = table.Column<long>(type: "bigint", nullable: true),
                    VendorId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Email", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonalDetailsId = table.Column<long>(type: "bigint", nullable: true),
                    Hired = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Exited = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompanyEmployeeId = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RepairOrderServiceTechnician",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    RepairOrderServiceId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrderServiceTechnician", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairOrderServiceTechnician_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "dbo",
                        principalTable: "Employee",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RepairOrderServiceTechnician_RepairOrderService_RepairOrderServiceId",
                        column: x => x.RepairOrderServiceId,
                        principalSchema: "dbo",
                        principalTable: "RepairOrderService",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoleAssignment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleAssignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleAssignment_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "dbo",
                        principalTable: "Employee",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Organization",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ContactId = table.Column<long>(type: "bigint", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: true),
                    AddressLine = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AddressCity = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AddressState = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    AddressPostalCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organization", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DriversLicenseNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DriversLicenseIssued = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DriversLicenseExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DriversLicenseState = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    VendorId = table.Column<long>(type: "bigint", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressLine = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AddressCity = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AddressState = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    AddressPostalCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Phone",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneType = table.Column<int>(type: "int", nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: true),
                    PersonId = table.Column<long>(type: "bigint", nullable: true),
                    VendorId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phone", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Phone_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalSchema: "dbo",
                        principalTable: "Organization",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Phone_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "dbo",
                        principalTable: "Person",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RepairOrderPurchase",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorId = table.Column<long>(type: "bigint", nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PONumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VendorInvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VendorPartNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepairOrderLineItemId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrderPurchase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairOrderPurchase_RepairOrderLineItem_RepairOrderLineItemId",
                        column: x => x.RepairOrderLineItemId,
                        principalSchema: "dbo",
                        principalTable: "RepairOrderLineItem",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Vendor",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VendorCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    VendorRole = table.Column<int>(type: "int", nullable: false),
                    DefaultPaymentMethod_PaymentMethodId = table.Column<long>(type: "bigint", nullable: true),
                    DefaultPaymentMethodAutoCompleteDocuments = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: true),
                    AddressLine = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AddressCity = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AddressState = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    AddressPostalCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VendorInvoice",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorId = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    DocumentType = table.Column<int>(type: "int", nullable: false),
                    DatePosted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Total = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorInvoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VendorInvoice_Vendor_VendorId",
                        column: x => x.VendorId,
                        principalSchema: "dbo",
                        principalTable: "Vendor",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VendorInvoicePaymentMethod",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    PaymentType = table.Column<int>(type: "int", nullable: false),
                    ReconcilingVendorId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorInvoicePaymentMethod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VendorInvoicePaymentMethod_Vendor_ReconcilingVendorId",
                        column: x => x.ReconcilingVendorId,
                        principalSchema: "dbo",
                        principalTable: "Vendor",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VendorInvoiceLineItem",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ItemPartNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ItemDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Item_ManufacturerId = table.Column<long>(type: "bigint", nullable: true),
                    Item_SaleCodeId = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    Cost = table.Column<double>(type: "float", nullable: false),
                    Core = table.Column<double>(type: "float", nullable: false),
                    PONumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VendorInvoiceId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorInvoiceLineItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VendorInvoiceLineItem_Manufacturer_Item_ManufacturerId",
                        column: x => x.Item_ManufacturerId,
                        principalSchema: "dbo",
                        principalTable: "Manufacturer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VendorInvoiceLineItem_SaleCode_Item_SaleCodeId",
                        column: x => x.Item_SaleCodeId,
                        principalSchema: "dbo",
                        principalTable: "SaleCode",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VendorInvoiceLineItem_VendorInvoice_VendorInvoiceId",
                        column: x => x.VendorInvoiceId,
                        principalSchema: "dbo",
                        principalTable: "VendorInvoice",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VendorInvoiceTax",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalesTaxId = table.Column<long>(type: "bigint", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    VendorInvoiceId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorInvoiceTax", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VendorInvoiceTax_SalesTax_SalesTaxId",
                        column: x => x.SalesTaxId,
                        principalSchema: "dbo",
                        principalTable: "SalesTax",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VendorInvoiceTax_VendorInvoice_VendorInvoiceId",
                        column: x => x.VendorInvoiceId,
                        principalSchema: "dbo",
                        principalTable: "VendorInvoice",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VendorInvoicePayment",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentMethodId = table.Column<long>(type: "bigint", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    VendorInvoiceId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorInvoicePayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VendorInvoicePayment_VendorInvoice_VendorInvoiceId",
                        column: x => x.VendorInvoiceId,
                        principalSchema: "dbo",
                        principalTable: "VendorInvoice",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VendorInvoicePayment_VendorInvoicePaymentMethod_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalSchema: "dbo",
                        principalTable: "VendorInvoicePaymentMethod",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_OrganizationId",
                schema: "dbo",
                table: "Customer",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_PersonId",
                schema: "dbo",
                table: "Customer",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Email_OrganizationId",
                schema: "dbo",
                table: "Email",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Email_PersonId",
                schema: "dbo",
                table: "Email",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Email_VendorId",
                schema: "dbo",
                table: "Email",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_PersonalDetailsId",
                schema: "dbo",
                table: "Employee",
                column: "PersonalDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_ExciseFee_SalesTaxId",
                schema: "dbo",
                table: "ExciseFee",
                column: "SalesTaxId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_InspectionId",
                schema: "dbo",
                table: "InventoryItem",
                column: "InspectionId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_LaborId",
                schema: "dbo",
                table: "InventoryItem",
                column: "LaborId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_ManufacturerId",
                schema: "dbo",
                table: "InventoryItem",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_PackageId",
                schema: "dbo",
                table: "InventoryItem",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_PartId",
                schema: "dbo",
                table: "InventoryItem",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_ProductCodeId",
                schema: "dbo",
                table: "InventoryItem",
                column: "ProductCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_TireId",
                schema: "dbo",
                table: "InventoryItem",
                column: "TireId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_WarrantyId",
                schema: "dbo",
                table: "InventoryItem",
                column: "WarrantyId");

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

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceItem_InventoryItemId",
                schema: "dbo",
                table: "MaintenanceItem",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_ContactId",
                schema: "dbo",
                table: "Organization",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_VendorId",
                schema: "dbo",
                table: "Person",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Phone_OrganizationId",
                schema: "dbo",
                table: "Phone",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Phone_PersonId",
                schema: "dbo",
                table: "Phone",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Phone_VendorId",
                schema: "dbo",
                table: "Phone",
                column: "VendorId");

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

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderItemTax_RepairOrderLineItemId",
                schema: "dbo",
                table: "RepairOrderItemTax",
                column: "RepairOrderLineItemId");

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

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderLineItem_Item_SaleCodeId",
                schema: "dbo",
                table: "RepairOrderLineItem",
                column: "Item_SaleCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderLineItem_RepairOrderServiceId",
                schema: "dbo",
                table: "RepairOrderLineItem",
                column: "RepairOrderServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderPayment_RepairOrderId",
                schema: "dbo",
                table: "RepairOrderPayment",
                column: "RepairOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderPurchase_RepairOrderLineItemId",
                schema: "dbo",
                table: "RepairOrderPurchase",
                column: "RepairOrderLineItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderPurchase_VendorId",
                schema: "dbo",
                table: "RepairOrderPurchase",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderSerialNumber_RepairOrderLineItemId",
                schema: "dbo",
                table: "RepairOrderSerialNumber",
                column: "RepairOrderLineItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderService_RepairOrderId",
                schema: "dbo",
                table: "RepairOrderService",
                column: "RepairOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderService_SaleCodeId",
                schema: "dbo",
                table: "RepairOrderService",
                column: "SaleCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderServiceTax_RepairOrderServiceId",
                schema: "dbo",
                table: "RepairOrderServiceTax",
                column: "RepairOrderServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderServiceTechnician_EmployeeId",
                table: "RepairOrderServiceTechnician",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderServiceTechnician_RepairOrderServiceId",
                table: "RepairOrderServiceTechnician",
                column: "RepairOrderServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderStatus_RepairOrderId",
                table: "RepairOrderStatus",
                column: "RepairOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderTax_RepairOrderId",
                schema: "dbo",
                table: "RepairOrderTax",
                column: "RepairOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderWarranty_RepairOrderLineItemId",
                schema: "dbo",
                table: "RepairOrderWarranty",
                column: "RepairOrderLineItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleAssignment_EmployeeId",
                table: "RoleAssignment",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleCode_ShopSuppliesId",
                schema: "dbo",
                table: "SaleCode",
                column: "ShopSuppliesId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_CustomerId",
                schema: "dbo",
                table: "Vehicle",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendor_DefaultPaymentMethod_PaymentMethodId",
                schema: "dbo",
                table: "Vendor",
                column: "DefaultPaymentMethod_PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorInvoice_VendorId",
                schema: "dbo",
                table: "VendorInvoice",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorInvoiceLineItem_Item_ManufacturerId",
                schema: "dbo",
                table: "VendorInvoiceLineItem",
                column: "Item_ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorInvoiceLineItem_Item_SaleCodeId",
                schema: "dbo",
                table: "VendorInvoiceLineItem",
                column: "Item_SaleCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorInvoiceLineItem_VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoiceLineItem",
                column: "VendorInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorInvoicePayment_PaymentMethodId",
                schema: "dbo",
                table: "VendorInvoicePayment",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorInvoicePayment_VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoicePayment",
                column: "VendorInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorInvoicePaymentMethod_ReconcilingVendorId",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod",
                column: "ReconcilingVendorId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorInvoiceTax_SalesTaxId",
                schema: "dbo",
                table: "VendorInvoiceTax",
                column: "SalesTaxId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorInvoiceTax_VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoiceTax",
                column: "VendorInvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Organization_OrganizationId",
                schema: "dbo",
                table: "Customer",
                column: "OrganizationId",
                principalSchema: "dbo",
                principalTable: "Organization",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Person_PersonId",
                schema: "dbo",
                table: "Customer",
                column: "PersonId",
                principalSchema: "dbo",
                principalTable: "Person",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Email_Organization_OrganizationId",
                schema: "dbo",
                table: "Email",
                column: "OrganizationId",
                principalSchema: "dbo",
                principalTable: "Organization",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Email_Person_PersonId",
                schema: "dbo",
                table: "Email",
                column: "PersonId",
                principalSchema: "dbo",
                principalTable: "Person",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Email_Vendor_VendorId",
                schema: "dbo",
                table: "Email",
                column: "VendorId",
                principalSchema: "dbo",
                principalTable: "Vendor",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Person_PersonalDetailsId",
                schema: "dbo",
                table: "Employee",
                column: "PersonalDetailsId",
                principalSchema: "dbo",
                principalTable: "Person",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Organization_Person_ContactId",
                schema: "dbo",
                table: "Organization",
                column: "ContactId",
                principalSchema: "dbo",
                principalTable: "Person",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Person_Vendor_VendorId",
                schema: "dbo",
                table: "Person",
                column: "VendorId",
                principalSchema: "dbo",
                principalTable: "Vendor",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Phone_Vendor_VendorId",
                schema: "dbo",
                table: "Phone",
                column: "VendorId",
                principalSchema: "dbo",
                principalTable: "Vendor",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairOrderPurchase_Vendor_VendorId",
                schema: "dbo",
                table: "RepairOrderPurchase",
                column: "VendorId",
                principalSchema: "dbo",
                principalTable: "Vendor",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendor_VendorInvoicePaymentMethod_DefaultPaymentMethod_PaymentMethodId",
                schema: "dbo",
                table: "Vendor",
                column: "DefaultPaymentMethod_PaymentMethodId",
                principalSchema: "dbo",
                principalTable: "VendorInvoicePaymentMethod",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorInvoicePaymentMethod_Vendor_ReconcilingVendorId",
                schema: "dbo",
                table: "VendorInvoicePaymentMethod");

            migrationBuilder.DropTable(
                name: "CreditCard",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Email",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ExciseFee",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "InventoryItemPackageItem",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "InventoryItemPackagePlaceholder",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MaintenanceItem",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Phone",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RepairOrderItemTax",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RepairOrderPayment",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RepairOrderPurchase",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RepairOrderSerialNumber",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RepairOrderServiceTax",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RepairOrderServiceTechnician");

            migrationBuilder.DropTable(
                name: "RepairOrderStatus");

            migrationBuilder.DropTable(
                name: "RepairOrderTax",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RepairOrderWarranty",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RoleAssignment");

            migrationBuilder.DropTable(
                name: "Vehicle",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VendorInvoiceLineItem",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VendorInvoicePayment",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VendorInvoiceTax",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "InventoryItem",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RepairOrderLineItem",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Employee",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Customer",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SalesTax",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VendorInvoice",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "InventoryItemInspection",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "InventoryItemLabor",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "InventoryItemPackage",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "InventoryItemPart",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "InventoryItemTire",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "InventoryItemWarranty",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ProductCode",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RepairOrderService",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Organization",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Manufacturer",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RepairOrder",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SaleCode",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Person",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SaleCodeShopSupplies");

            migrationBuilder.DropTable(
                name: "Vendor",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VendorInvoicePaymentMethod",
                schema: "dbo");
        }
    }
}
