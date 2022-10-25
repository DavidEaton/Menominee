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
            // Each test that inherits from this IntegrationTestBase automatically
            // clears the database when created, setting it to a known state.
            ClearDatabase();
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            SaleCode saleCode = CreateSaleCode();
            Manufacturer manufacturer = InventoryItemHelper.CreateManufacturer();
            ProductCode productCode = CreateProductCode(manufacturer, saleCode);
            InventoryItemPart part = InventoryItemHelper.CreateInventoryItemPart();

            InventoryItem inventoryItem = InventoryItemHelper.CreateInventoryItem();

            ApplicationDbContext context = Helpers.CreateTestContext();

            context.Add(saleCode);
            context.Add(manufacturer);
            context.Add(productCode);
            //context.Add(inventoryItem);
            context.Add(part);
            context.SaveChanges();

        }

        private static ProductCode CreateProductCode(Manufacturer manufacturer, SaleCode saleCode)
        {
            return new ProductCode()
            {
                Manufacturer = manufacturer,
                SaleCode = saleCode,
                Name = "A Product",
                Code = "P1"
            };
        }

        private void ClearDatabase()
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
