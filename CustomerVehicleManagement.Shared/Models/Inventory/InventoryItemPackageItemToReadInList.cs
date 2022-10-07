namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemPackageItemToReadInList
    {
        public long Id { get; set; }
        public int DisplayOrder { get; set; }
        public InventoryItemToReadInList Item { get; set; }
        public InventoryItemPackageDetailsToRead Details { get; set; }
    }
}
