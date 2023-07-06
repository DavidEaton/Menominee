using CustomerVehicleManagement.Domain.Entities.Inventory;

namespace CustomerVehicleManagement.Data.Results
{
    public static class InventoryItemGeneratorResult
    {
        public static IReadOnlyList<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();

    }
}
