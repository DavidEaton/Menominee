using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Domain.Entities.Taxes;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using CustomerVehicleManagement.Tests.Unit.Helpers;
using CustomerVehicleManagement.Tests.Unit.Helpers.Payables;
using Menominee.Common.Enums;
using System.Collections.Generic;
using System.Linq;
using static CustomerVehicleManagement.Tests.Unit.Helpers.Payables.VendorInvoiceTestHelper;
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
            var halfSeedCount = maxSeedCount / 2;
            ApplicationDbContext context = Helpers.CreateTestContext();

            var vendors = CreateVendors(maxSeedCount);
            var saleCodes = CreateTestSaleCodes(maxSeedCount);
            var manufacturers = InventoryItemTestHelper.CreateManufacturers(maxSeedCount);
            var parts = InventoryItemTestHelper.CreateInventoryItemParts(maxSeedCount);

            context.AddRange(vendors);
            context.AddRange(saleCodes);
            context.AddRange(manufacturers);
            context.AddRange(parts);
            context.SaveChanges();

            var manufacturerCodes = context.ProductCodes
                .Select(productCode =>
                     $"{productCode.Manufacturer.Id} + {productCode.Code}")
                .ToList();

            var productCodes = CreateProductCodes(
                maxSeedCount,
                manufacturers[halfSeedCount],
                manufacturerCodes);

            var paymentMethods = CreateVendorInvoicePaymentMethods(maxSeedCount);
            var salesTaxes = CreateTestSalesTaxes(maxSeedCount);
            var paymentMethod = paymentMethods[^1];

            context.AddRange(productCodes);
            context.AddRange(paymentMethods);
            context.Add(paymentMethod);
            context.AddRange(salesTaxes);
            context.SaveChanges();


            // VendorInvoice
            VendorInvoiceToWrite invoiceToWrite = CreateVendorInvoiceToWrite(vendors[^1]);
            invoiceToWrite.LineItems = CreateLineItemsToWrite(new LineItemTestOptions());
            invoiceToWrite.Payments = CreateTestPaymentsToWrite(paymentMethod, paymentCount: halfSeedCount);
            invoiceToWrite.Taxes = CreateTaxesToWrite(salesTaxes[^1], taxLineCount: maxSeedCount, taxAmount: 5.5);

            VendorInvoice invoice = VendorInvoiceHelper.ConvertWriteToEntity(invoiceToWrite, vendors[^1]);

            context.Add(invoice);
            context.SaveChanges();
        }

        private static IList<VendorInvoicePaymentToWrite> CreateTestPaymentsToWrite(VendorInvoicePaymentMethod paymentMethod, int paymentCount)
        {
            var list = new List<VendorInvoicePaymentToWrite>();
            var amount = 11.11;

            for (int i = 0; i < paymentCount; i++)
            {
                list.Add(new VendorInvoicePaymentToWrite()
                {
                    Amount = amount * (++i),
                    PaymentMethod = VendorInvoicePaymentMethodHelper.ConvertEntityToReadDto(paymentMethod)
                });
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

        private static IReadOnlyList<SaleCode> CreateTestSaleCodes(int count)
        {
            var list = new List<SaleCode>();
            var laborRate = count + 11.11;
            var desiredMargin = count + 11.11;

            for (int i = 0; i < count; i++)
            {
                list.Add(SaleCode.Create(
                    name: $"{i}name",
                    code: $"{i}code",
                    laborRate: laborRate + i,
                    desiredMargin: desiredMargin + i,
                    shopSupplies: new SaleCodeShopSupplies()).Value);
            }

            return list;
        }

        private static IReadOnlyList<SalesTax> CreateTestSalesTaxes(int count)
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
