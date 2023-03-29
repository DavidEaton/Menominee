using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Domain.Entities.Taxes;
using CustomerVehicleManagement.Tests.Unit.Helpers;
using Menominee.Common.Enums;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CustomerVehicleManagement.Tests.Integration
{
    /*
     * VK Im.2:
     *
     * 1) Deleting and re-creating the database on each test is slow. Use migrations to bring the tests db to the
     * desired state (manually, once), and then before each test only remove non-reference data from the database.
     * Each test then should create its fixtures/test data in its Arrange section.
     * (Reference data is data that's not changed by the app and is required for the app to run properly, e.g PhoneType etc)
     *
     * 2) Seeding data shouldn't be part of the "common" step for all integration tests. If its reference data, then
     * modify it in migration scripts. If it's master/non-reference data, then each test should create its own set of such data in Arrange section
     */
    public class IntegrationTestBase
    {
        public IntegrationTestBase()
        {
            // When created, each test that inherits from this IntegrationTestBase
            // automatically clears the database, setting it to a known state.
            ClearDatabase();
        }

        private static void ClearDatabase()
        {
            var sqlCommands = new List<string>();
            var typeNames = DatabaseTableNamesForDeletion();

            foreach (var name in typeNames)
                sqlCommands.Add($"DELETE FROM [dbo].[{name}]");

            foreach (var command in sqlCommands)
            {
                try
                {
                    ExecuteCommand(command);
                }
                catch (Exception ex)
                {
                    // Continue executing list after exception
                    // TODO: log exception
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static IList<string> DatabaseTableNamesForDeletion()
        {
            // Order matters. Use this manual list in ClearDatabase();
            // add new table names as integration tests are added
            return new List<string>
                {
                    "CreditCard",
                    "VendorInvoiceLineItem",
                    "VendorInvoicePayment",
                    "VendorInvoiceTax",
                    "ExciseFee",
                    "SalesTax",
                    "VendorInvoice",
                    "VendorInvoicePaymentMethod",
                    "Phone",
                    "Email",
                    "Vendor",

                    //"Person",
                    //"Organization",
                    //"Customer",
                };
        }

        private static void ExecuteCommand(string query)
        {
            using var connection = new SqlConnection(Helpers.IntegrationTestsConnectionString);
            var command = new SqlCommand(query, connection)
            {
                CommandType = CommandType.Text
            };

            connection.Open();
            command.ExecuteNonQuery();
        }

        protected static void AddInventoryItems(int count)
        {
            using (var context = Helpers.CreateTestContext())
            {
                context.Database.EnsureCreated();
                List<InventoryItem> inventoryItems = InventoryItemTestHelper.CreateInventoryItems(count);
                context.AddRange(inventoryItems);
                context.SaveChanges();
            };

        }

        protected static void AddProductCodes()
        {
            var maxSeedCount = 10;
            var halfSeedCount = maxSeedCount / 2;

            using (var context = Helpers.CreateTestContext())
            {
                context.Database.EnsureCreated();

                var manufacturerCodes = context.ProductCodes
                    .Select(productCode =>
                         $"{productCode.Manufacturer.Id} + {productCode.Code}")
                    .ToList();

                var manufacturers = InventoryItemTestHelper.CreateManufacturers(maxSeedCount);

                var productCodes = CreateProductCodes(
                    maxSeedCount,
                    manufacturers[halfSeedCount],
                    manufacturerCodes);

                context.AddRange(manufacturers);
                context.AddRange(productCodes);
                context.SaveChanges();
            };
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

        private static IReadOnlyList<VendorInvoicePaymentMethod> CreateVendorInvoicePaymentMethodsSansReconcilingVendor(int count)
        {
            var list = new List<VendorInvoicePaymentMethod>();

            for (int i = 0; i < count; i++)
            {
                list.Add(VendorInvoicePaymentMethod.Create(
                    new List<string>(),
                    $"Payment {i}",
                    isActive: true,
                    paymentType: VendorInvoicePaymentMethodType.Normal,
                    reconcilingVendor: null).Value);
            }

            return list;
        }

        protected class CreatedResultResponse
        {
            public string Location { get; set; }
            public Value Value { get; set; }
            public object[] Formatters { get; set; }
            public object[] ContentTypes { get; set; }
            public int StatusCode { get; set; }
        }

        protected class Value
        {
            public long Id { get; set; }
        }
    }
}
