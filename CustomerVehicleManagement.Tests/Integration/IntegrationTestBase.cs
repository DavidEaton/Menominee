using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Domain.Entities.Taxes;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.LineItems;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using CustomerVehicleManagement.Tests.Unit.Helpers;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using static CustomerVehicleManagement.Tests.Unit.Helpers.VendorInvoiceHelper;
using static CustomerVehicleManagement.Tests.Utilities;

namespace CustomerVehicleManagement.Tests.Integration
{
    public class IntegrationTestBase
    {
        public IntegrationTestBase()
        {
            // When created, each test that inherits from this IntegrationTestBase
            // automatically clears and seeds the database, setting it to a known state.
            ClearDatabase();
            SeedDatabase();
        }

        private static void ClearDatabase()
        {
            ApplicationDbContext context = Helpers.CreateTestContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        private static void SeedDatabase()
        {
            var maxSeedCount = 10;
            ApplicationDbContext context = Helpers.CreateTestContext();

            var vendors = CreateVendors(maxSeedCount);
            IReadOnlyList<SaleCode> saleCodes = CreateSaleCodes(maxSeedCount);
            List<Manufacturer> manufacturers = InventoryItemHelper.CreateManufacturers(maxSeedCount);
            IReadOnlyList<InventoryItemPart> parts = InventoryItemHelper.CreateInventoryItemParts(maxSeedCount);

            context.AddRange(vendors);
            context.AddRange(saleCodes);
            context.AddRange(manufacturers);
            context.AddRange(parts);
            context.SaveChanges();

            IReadOnlyList<string> manufacturerCodes = context.ProductCodes.Select(productCode => $"{productCode.Manufacturer.Id} + {productCode.Code}").ToList();
            IReadOnlyList<ProductCode> productCodes = CreateProductCodes(
                maxSeedCount,
                manufacturers[maxSeedCount / 2],
                manufacturerCodes);
            var paymentMethods = CreateVendorInvoicePaymentMethods(maxSeedCount);
            IReadOnlyList<SalesTax> salesTaxes = CreateSalesTaxes(maxSeedCount);
            VendorInvoicePaymentMethod paymentMethod = paymentMethods[maxSeedCount / 2];

            context.AddRange(productCodes);
            context.AddRange(paymentMethods);
            context.Add(paymentMethod);
            context.AddRange(salesTaxes);
            context.SaveChanges();


            // VendorInvoiceToWrite
            VendorInvoiceToWrite invoice = CreateTestVendorInvoice(vendors[vendors.Count - 1]);
            IList<VendorInvoiceLineItemToWrite> lineItems = CreateTestLineItems(maxSeedCount / 2);
            IList<VendorInvoicePayment> payments = CreateTestPayments(paymentMethod, maxSeedCount / 2);
            IList<VendorInvoiceTax> taxes = CreateTestTaxes(maxSeedCount, salesTaxes[salesTaxes.Count - 1]);

        }

        private static IList<VendorInvoiceTax> CreateTestTaxes(int count, SalesTax salesTax)
        {
            var list = new List<VendorInvoiceTax>();

            for (int i = 0; i < count; i++)
            {
                list.Add(VendorInvoiceTax.Create(salesTax, 22.22 * (i + 1)).Value);
            }

            return list;
        }

        private static IReadOnlyList<ProductCode> CreateProductCodes(int count, Manufacturer manufacturer, IReadOnlyList<string> manufacturerCodes)
        {
            var list = new List<ProductCode>();

            for (int i = 0; i < count; i++)
            {
                list.Add(ProductCode.Create(
                    manufacturer,
                    code: $"{i}Code",
                    name: $"{i}Name",
                    manufacturerCodes: manufacturerCodes
                    ).Value);
            }

            return list;
        }

        private static IReadOnlyList<SaleCode> CreateSaleCodes(int count)
        {
            var list = new List<SaleCode>();

            for (int i = 0; i < count; i++)
            {
                list.Add(SaleCode.Create(
                    name: $"{i}name",
                    code: $"{i}code",
                    laborRate: 1.75,
                    desiredMargin: 33.33,
                    shopSupplies: new SaleCodeShopSupplies()).Value);
            }

            return list;
        }

        private static IList<VendorInvoicePayment> CreateTestPayments(VendorInvoicePaymentMethod paymentMethod, int count)
        {
            var list = new List<VendorInvoicePayment>();

            for (int i = 0; i < count; i++)
            {
                list.Add(VendorInvoicePayment.Create(paymentMethod, 22.22 * (i+1)).Value);
            }

            return list;
        }

        private static IList<VendorInvoiceLineItemToWrite> CreateTestLineItems(int lineItemCount)
        {
            VendorInvoiceLineItemType lineItemType = VendorInvoiceLineItemType.Purchase;
            var lineItemCore = 2.2;
            var lineItemCost = 4.4;
            var lineItemQuantity = 2;

            return CreateLineItemsToWrite(lineItemType, lineItemCount, lineItemCore, lineItemCost, lineItemQuantity);
        }
        private static IReadOnlyList<SalesTax> CreateSalesTaxes(int count)
        {
            var list = new List<SalesTax>();

            for (int i = 0; i < count; i++)
            {
                list.Add(SalesTax.Create(
                    description: $"{i} description",
                    taxType: SalesTaxType.Normal,
                    order: i,
                    taxIdNumber: $"EIN-000{i}",
                    partTaxRate: i * .1,
                    laborTaxRate: i * .2).Value);
            }

            return list;
        }

        private static VendorInvoiceToWrite CreateTestVendorInvoice(Vendor vendor)
        {
            return new()
            {
                Date = DateTime.Today,
                DocumentType = VendorInvoiceDocumentType.Invoice,
                Status = VendorInvoiceStatus.Open,
                Total = 10.0,
                Vendor = new VendorToReadInList()
                {
                    Id = vendor.Id,
                    IsActive = vendor.IsActive,
                    Name = vendor.Name,
                    VendorCode = vendor.VendorCode
                }
            };
        }

        private static IReadOnlyList<VendorInvoicePaymentMethod> CreateVendorInvoicePaymentMethods(int count)
        {
            var list = new List<VendorInvoicePaymentMethod>();

            for (int i = 0; i < count; i++)
            {
                list.Add(VendorInvoicePaymentMethod.Create(
                    new List<string>(),
                    $"Payment {i}",
                    isActive: true,
                    //isOnAccountPaymentType: false,
                    paymentType: VendorInvoicePaymentMethodType.Normal,
                    reconcilingVendor: null).Value);
            }

            return list;
        }

    }

    public class CreatedResultResponse
    {
        public string Location { get; set; }
        public Value Value { get; set; }
        public object[] Formatters { get; set; }
        public object[] ContentTypes { get; set; }
        public int StatusCode { get; set; }
    }

    public class Value
    {
        public int Id { get; set; }
    }
}
