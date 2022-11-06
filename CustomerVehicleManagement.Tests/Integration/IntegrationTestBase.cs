using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared;
using CustomerVehicleManagement.Tests.Unit.Helpers;
using System.Collections.Generic;
using System.Linq;

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
            SaleCode saleCode = CreateSaleCode();
            var manufacturers = InventoryItemHelper.CreateManufacturers(10);
            ProductCode productCode = CreateProductCode(manufacturers[5], saleCode); // Grab an arbitrary manufacturer: manufacturers[5] and create a ProductCode.
            InventoryItemPart part = InventoryItemHelper.CreateInventoryItemPart();

            ApplicationDbContext context = Helpers.CreateTestContext();

            context.Add(saleCode);
            context.AddRange(manufacturers);
            context.Add(productCode);
            context.Add(part);
            context.SaveChanges();
        }

        private static ProductCode CreateProductCode(Manufacturer manufacturer, SaleCode saleCode)
        {
            var manufacturerCodes = GetManufacturerCodes();
            return ProductCode.Create(manufacturer, "A1", "A One", manufacturerCodes, saleCode).Value;
        }

        private static SaleCode CreateSaleCode()
        {
            string name = Utilities.RandomCharacters(SaleCode.MinimumLength);
            string code = Utilities.RandomCharacters(SaleCode.MinimumLength);
            double laborRate = SaleCode.MinimumValue;
            double desiredMargin = SaleCode.MinimumValue;
            SaleCodeShopSupplies shopSupplies = new();

            return SaleCode.Create(name, code, laborRate, desiredMargin, shopSupplies).Value;
        }

        private static List<string> GetManufacturerCodes()
        {
            ApplicationDbContext context = Helpers.CreateTestContext();

            return context.ProductCodes.Select(productCode => $"{productCode.Manufacturer.Id} + {productCode.Code}").ToList();
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
