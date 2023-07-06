using CustomerVehicleManagement.Data.Database;
using CustomerVehicleManagement.Data.Results;
using CustomerVehicleManagement.Tests.Helpers.Fakers;

namespace CustomerVehicleManagement.Data.Generators
{
    public static class InventoryItemGenerator
    {
        public static void GenerateData()
        {
            GenerateInventoryItems();

            foreach (var item in InventoryItemGeneratorResult.InventoryItems)
                Helper.SaveToDatabase(item);
        }
        private static void GenerateInventoryItems()
        {
            var countOfInventoryItemsToCreate = 20;
            var generateId = false;
            var parts = new InventoryItemFaker(generateId: generateId).Generate(countOfInventoryItemsToCreate);

            InventoryItemGeneratorResult.InventoryItems = parts;
        }

    }
}
