namespace CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Package
{
    public class InventoryItemPackageItemToRead
    {
        public long Id { get; set; }
        public int DisplayOrder { get; set; }
        public InventoryItemToRead Item { get; set; }
        public InventoryItemPackageDetailsToRead Details { get; set; }
    }
}
