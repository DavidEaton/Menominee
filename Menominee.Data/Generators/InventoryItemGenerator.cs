using Menominee.Data.Database;
using Menominee.Data.Results;
using Menominee.Tests.Helpers.Fakers;

namespace Menominee.Data.Generators
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
