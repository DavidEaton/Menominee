using System.Collections.Generic;
using System.Linq;
using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Domain.Entities.Payables;
using Microsoft.EntityFrameworkCore;
using TestingHelperLibrary;
using TestingHelperLibrary.Payables;

namespace CustomerVehicleManagement.Tests.Integration
{
    public class Database
    {
        public static void AddInventoryItems(int count)
        {
            using (var context = IntegrationTestBase.CreateTestContext())
            {
                context.Database.EnsureCreated();
                List<InventoryItem> inventoryItems = InventoryItemTestHelper.CreateInventoryItems(count);
                context.AddRange(inventoryItems);
                context.SaveChanges();
            };
        }

        public static void AddProductCodes()
        {
            var maxSeedCount = 10;
            var halfSeedCount = maxSeedCount / 2;

            using (var context = IntegrationTestBase.CreateTestContext())
            {
                context.Database.EnsureCreated();

                var manufacturerCodes = context.ProductCodes
                    .Select(productCode =>
                         $"{productCode.Manufacturer.Id} + {productCode.Code}")
                    .ToList();

                var manufacturers = InventoryItemTestHelper.CreateManufacturers(maxSeedCount);

                var productCodes = DataContracts.CreateProductCodes(
                    maxSeedCount,
                    manufacturers[halfSeedCount],
                    manufacturerCodes);

                context.AddRange(manufacturers);
                context.AddRange(productCodes);
                context.SaveChanges();
            };
        }

        public static void AddSalesTaxes(int count = 10)
        {
            using (var context = IntegrationTestBase.CreateTestContext())
            {
                context.Database.EnsureCreated();

                var entities = DataContracts.CreateSalesTaxes(count);
                context.AddRange(entities);
                context.SaveChanges();
            };
        }

        public static void AddVendors(int count = 10)
        {
            using (var context = IntegrationTestBase.CreateTestContext())
            {
                context.Database.EnsureCreated();

                var entities = VendorTestHelper.CreateVendors(count);
                context.AddRange(entities);
                context.SaveChanges();
            };
        }

        public static void AddVendorInvoicePaymentMethods(int count = 10)
        {
            using (var context = IntegrationTestBase.CreateTestContext())
            {
                context.Database.EnsureCreated();

                var entities = VendorInvoiceTestHelper.CreateVendorInvoicePaymentMethods(count);
                context.AddRange(entities);
                context.SaveChanges();
            };
        }

        public static List<VendorInvoice> AddVendorInvoices(int count = 10)
        {
            var result = new List<VendorInvoice>();
            var childRowCount = 3;

            using (var context = IntegrationTestBase.CreateTestContext())
            {
                context.Database.EnsureCreated();

                var vendors = context.Vendors.ToList();

                if (vendors.Count == 0)
                {
                    vendors = VendorTestHelper.CreateVendors(count);
                    context.AddRange(vendors);
                    context.SaveChanges();
                }

                var vendorInvoices = VendorInvoiceTestHelper.CreateVendorInvoices(vendors, childRowCount);
                context.AddRange(vendorInvoices);
                context.SaveChanges();
                result = GetVendorInvoices(context);
            };

            return result;
        }

        private static List<VendorInvoice> GetVendorInvoices(ApplicationDbContext context)
        {
            return context.VendorInvoices
            .Include(invoice => invoice.Vendor)
                .ThenInclude(vendor => vendor.DefaultPaymentMethod)
                    .ThenInclude(defaultPaymentMethod => defaultPaymentMethod.PaymentMethod)
            .Include(invoice => invoice.LineItems)
                .ThenInclude(item => item.Item.Manufacturer)
            .Include(invoice => invoice.LineItems)
                .ThenInclude(item => item.Item.SaleCode)
            .Include(invoice => invoice.Payments)
                .ThenInclude(payment => payment.PaymentMethod)
            .Include(invoice => invoice.Payments)
                .ThenInclude(payment => payment.PaymentMethod)
                    .ThenInclude(method => method.ReconcilingVendor)
            .Include(invoice => invoice.Taxes)
                .ThenInclude(tax => tax.SalesTax)
            .AsSplitQuery()
            .ToList();
        }
    }
}
