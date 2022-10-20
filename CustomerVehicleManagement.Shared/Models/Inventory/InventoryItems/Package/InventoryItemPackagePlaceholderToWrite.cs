namespace CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Package
{
    public class InventoryItemPackagePlaceholderToWrite
    {
        public long Id { get; set; }
        public string ItemType { get; set; }
        public string Description { get; private set; }
        public int DisplayOrder { get; set; }
        public InventoryItemPackageDetailsToWrite Details { get; set; }
    }
}
