using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CustomerVehicleManagement.Api.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Manufacturer",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturer", x => x.Id);
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
                    DateInvoiced = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RepairOrderPurchase",
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
                    table.PrimaryKey("PK_RepairOrderPurchase", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SaleCode",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleCode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vendor",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VendorInvoicePaymentMethods",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrackingState = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorInvoicePaymentMethods", x => x.Id);
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
                    Note = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: true),
                    AddressLine = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AddressCity = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AddressState = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    AddressPostalCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organization_Person_ContactId",
                        column: x => x.ContactId,
                        principalSchema: "dbo",
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RepairOrderPayment",
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
                    table.PrimaryKey("PK_RepairOrderPayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairOrderPayment_RepairOrder_RepairOrderId",
                        column: x => x.RepairOrderId,
                        principalSchema: "dbo",
                        principalTable: "RepairOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RepairOrderService",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepairOrderId = table.Column<long>(type: "bigint", nullable: false),
                    ServiceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SaleCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCounterSale = table.Column<bool>(type: "bit", nullable: false),
                    IsDeclined = table.Column<bool>(type: "bit", nullable: false),
                    PartsTotal = table.Column<double>(type: "float", nullable: false),
                    LaborTotal = table.Column<double>(type: "float", nullable: false),
                    DiscountTotal = table.Column<double>(type: "float", nullable: false),
                    TaxTotal = table.Column<double>(type: "float", nullable: false),
                    ShopSuppliesTotal = table.Column<double>(type: "float", nullable: false),
                    Total = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrderService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairOrderService_RepairOrder_RepairOrderId",
                        column: x => x.RepairOrderId,
                        principalSchema: "dbo",
                        principalTable: "RepairOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RepairOrderTax",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepairOrderId = table.Column<long>(type: "bigint", nullable: false),
                    TaxId = table.Column<long>(type: "bigint", nullable: false),
                    PartTaxRate = table.Column<double>(type: "float", nullable: false),
                    LaborTaxRate = table.Column<double>(type: "float", nullable: false),
                    PartTax = table.Column<double>(type: "float", nullable: false),
                    LaborTax = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrderTax", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairOrderTax_RepairOrder_RepairOrderId",
                        column: x => x.RepairOrderId,
                        principalSchema: "dbo",
                        principalTable: "RepairOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductCode_SaleCode_SaleCodeId",
                        column: x => x.SaleCodeId,
                        principalSchema: "dbo",
                        principalTable: "SaleCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VendorInvoice",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorId = table.Column<long>(type: "bigint", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DatePosted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    table.ForeignKey(
                        name: "FK_Customer_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalSchema: "dbo",
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customer_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "dbo",
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    PersonId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Email", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Email_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalSchema: "dbo",
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Email_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "dbo",
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    PersonId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phone", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Phone_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalSchema: "dbo",
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Phone_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "dbo",
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RepairOrderServiceTax",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepairOrderServiceId = table.Column<long>(type: "bigint", nullable: false),
                    TaxId = table.Column<long>(type: "bigint", nullable: false),
                    PartTaxRate = table.Column<double>(type: "float", nullable: false),
                    LaborTaxRate = table.Column<double>(type: "float", nullable: false),
                    PartTax = table.Column<double>(type: "float", nullable: false),
                    LaborTax = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrderServiceTax", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairOrderServiceTax_RepairOrderService_RepairOrderServiceId",
                        column: x => x.RepairOrderServiceId,
                        principalSchema: "dbo",
                        principalTable: "RepairOrderService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RepairOrderTech",
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
                    table.PrimaryKey("PK_RepairOrderTech", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairOrderTech_RepairOrderService_RepairOrderServiceId",
                        column: x => x.RepairOrderServiceId,
                        principalSchema: "dbo",
                        principalTable: "RepairOrderService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "RepairOrderItem",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepairOrderServiceId = table.Column<long>(type: "bigint", nullable: false),
                    ManufacturerId = table.Column<long>(type: "bigint", nullable: false),
                    PartNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SaleCodeId = table.Column<long>(type: "bigint", nullable: false),
                    ProductCodeId = table.Column<long>(type: "bigint", nullable: false),
                    SaleType = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    PartType = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsDeclined = table.Column<bool>(type: "bit", nullable: false),
                    IsCounterSale = table.Column<bool>(type: "bit", nullable: false),
                    QuantitySold = table.Column<double>(type: "float", nullable: false),
                    SellingPrice = table.Column<double>(type: "float", nullable: false),
                    LaborType = table.Column<int>(type: "int", nullable: false),
                    LaborEach = table.Column<double>(type: "float", nullable: false),
                    Cost = table.Column<double>(type: "float", nullable: false),
                    Core = table.Column<double>(type: "float", nullable: false),
                    DiscountType = table.Column<int>(type: "int", nullable: false),
                    DiscountEach = table.Column<double>(type: "float", nullable: false),
                    Total = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrderItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairOrderItem_Manufacturer_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalSchema: "dbo",
                        principalTable: "Manufacturer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RepairOrderItem_ProductCode_ProductCodeId",
                        column: x => x.ProductCodeId,
                        principalSchema: "dbo",
                        principalTable: "ProductCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RepairOrderItem_RepairOrderService_RepairOrderServiceId",
                        column: x => x.RepairOrderServiceId,
                        principalSchema: "dbo",
                        principalTable: "RepairOrderService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RepairOrderItem_SaleCode_SaleCodeId",
                        column: x => x.SaleCodeId,
                        principalSchema: "dbo",
                        principalTable: "SaleCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VendorInvoiceItem",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    PartNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MfrId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    Cost = table.Column<double>(type: "float", nullable: false),
                    Core = table.Column<double>(type: "float", nullable: false),
                    PONumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorInvoiceItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VendorInvoiceItem_VendorInvoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalSchema: "dbo",
                        principalTable: "VendorInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VendorInvoicePayment",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VendorInvoiceTax",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    TaxId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    VendorInvoiceId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorInvoiceTax", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VendorInvoiceTax_VendorInvoice_VendorInvoiceId",
                        column: x => x.VendorInvoiceId,
                        principalSchema: "dbo",
                        principalTable: "VendorInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RepairOrderItemTax",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepairOrderItemId = table.Column<long>(type: "bigint", nullable: false),
                    TaxId = table.Column<long>(type: "bigint", nullable: false),
                    PartTaxRate = table.Column<double>(type: "float", nullable: false),
                    LaborTaxRate = table.Column<double>(type: "float", nullable: false),
                    PartTax = table.Column<double>(type: "float", nullable: false),
                    LaborTax = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrderItemTax", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairOrderItemTax_RepairOrderItem_RepairOrderItemId",
                        column: x => x.RepairOrderItemId,
                        principalSchema: "dbo",
                        principalTable: "RepairOrderItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RepairOrderSerialNumber",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepairOrderItemId = table.Column<long>(type: "bigint", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrderSerialNumber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairOrderSerialNumber_RepairOrderItem_RepairOrderItemId",
                        column: x => x.RepairOrderItemId,
                        principalSchema: "dbo",
                        principalTable: "RepairOrderItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RepairOrderWarranty",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepairOrderItemId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    NewWarranty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalWarranty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalInvoiceId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrderWarranty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairOrderWarranty_RepairOrderItem_RepairOrderItemId",
                        column: x => x.RepairOrderItemId,
                        principalSchema: "dbo",
                        principalTable: "RepairOrderItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_InventoryItem_ManufacturerId",
                schema: "dbo",
                table: "InventoryItem",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_ProductCodeId",
                schema: "dbo",
                table: "InventoryItem",
                column: "ProductCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_ContactId",
                schema: "dbo",
                table: "Organization",
                column: "ContactId");

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
                name: "IX_RepairOrderItem_ManufacturerId",
                schema: "dbo",
                table: "RepairOrderItem",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderItem_ProductCodeId",
                schema: "dbo",
                table: "RepairOrderItem",
                column: "ProductCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderItem_RepairOrderServiceId",
                schema: "dbo",
                table: "RepairOrderItem",
                column: "RepairOrderServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderItem_SaleCodeId",
                schema: "dbo",
                table: "RepairOrderItem",
                column: "SaleCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderItemTax_RepairOrderItemId",
                schema: "dbo",
                table: "RepairOrderItemTax",
                column: "RepairOrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderPayment_RepairOrderId",
                schema: "dbo",
                table: "RepairOrderPayment",
                column: "RepairOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderSerialNumber_RepairOrderItemId",
                schema: "dbo",
                table: "RepairOrderSerialNumber",
                column: "RepairOrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderService_RepairOrderId",
                schema: "dbo",
                table: "RepairOrderService",
                column: "RepairOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderServiceTax_RepairOrderServiceId",
                schema: "dbo",
                table: "RepairOrderServiceTax",
                column: "RepairOrderServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderTax_RepairOrderId",
                schema: "dbo",
                table: "RepairOrderTax",
                column: "RepairOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderTech_RepairOrderServiceId",
                schema: "dbo",
                table: "RepairOrderTech",
                column: "RepairOrderServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderWarranty_RepairOrderItemId",
                schema: "dbo",
                table: "RepairOrderWarranty",
                column: "RepairOrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_CustomerId",
                schema: "dbo",
                table: "Vehicle",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorInvoice_VendorId",
                schema: "dbo",
                table: "VendorInvoice",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorInvoiceItem_InvoiceId",
                schema: "dbo",
                table: "VendorInvoiceItem",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorInvoicePayment_VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoicePayment",
                column: "VendorInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorInvoiceTax_VendorInvoiceId",
                schema: "dbo",
                table: "VendorInvoiceTax",
                column: "VendorInvoiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Email",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "InventoryItem",
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
                name: "RepairOrderTax",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RepairOrderTech",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RepairOrderWarranty",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Vehicle",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VendorInvoiceItem",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VendorInvoicePayment",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VendorInvoicePaymentMethods");

            migrationBuilder.DropTable(
                name: "VendorInvoiceTax",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RepairOrderItem",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Customer",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VendorInvoice",
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
                name: "Vendor",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Manufacturer",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SaleCode",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RepairOrder",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Person",
                schema: "dbo");
        }
    }
}
