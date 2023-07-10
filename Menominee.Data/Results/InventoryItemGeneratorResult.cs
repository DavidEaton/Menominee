using Menominee.Domain.Entities.Inventory;

namespace Menominee.Data.Results
{
    public static class InventoryItemGeneratorResult
    {
        public static IReadOnlyList<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();

    }
}
