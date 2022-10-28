using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.TestUtilities;
using CustomerVehicleManagement.Tests.Unit.Helpers;

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

        private static void SeedDatabase()
        {
            SaleCode saleCode = CreateSaleCode();
            var manufacturers = InventoryItemHelper.CreateManufacturers(10);
            ProductCode productCode = CreateProductCode(manufacturers[5], saleCode);
            InventoryItemPart part = InventoryItemHelper.CreateInventoryItemPart();

            InventoryItem inventoryItem = InventoryItemHelper.CreateInventoryItem();

            ApplicationDbContext context = Helpers.CreateTestContext();

            context.Add(saleCode);
            context.AddRange(manufacturers);
            context.Add(productCode);
            context.Add(part);
            context.SaveChanges();
        }

        private static ProductCode CreateProductCode(Manufacturer manufacturer, SaleCode saleCode)
        {
            return ProductCode.Create(manufacturer, "A1", "A One", saleCode).Value;
        }

        private static void ClearDatabase()
        {
            ApplicationDbContext context = Helpers.CreateTestContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
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

    }
}
