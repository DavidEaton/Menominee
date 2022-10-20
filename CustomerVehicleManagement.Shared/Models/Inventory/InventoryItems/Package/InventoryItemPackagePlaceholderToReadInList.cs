namespace CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Package
{
    public class InventoryItemPackagePlaceholderToReadInList
    {
        public long Id { get; set; }
        public int DisplayOrder { get; set; }
        public string ItemType { get; set; }
        public InventoryItemPackageDetailsToRead Details { get; set; }
    }
}
