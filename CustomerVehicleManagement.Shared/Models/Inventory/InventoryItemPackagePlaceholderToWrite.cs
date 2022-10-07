namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemPackagePlaceholderToWrite
    {
        public long Id { get; set; }
        public int DisplayOrder { get; set; }
        public string ItemType { get; set; }
        public InventoryItemPackageDetailsToWrite Details { get; set; }
    }
}
