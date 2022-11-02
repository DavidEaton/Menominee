namespace CustomerVehicleManagement.Shared.Models.Inventory.MaintenanceItems
{
    public class MaintenanceItemToReadInList
    {
        public long Id { get; set; }
        public long DisplayOrder { get; set; }
        //public InventoryItemToReadInList Item { get; set; }
        public long InventoryItemId { get; set; }
        public string ItemNumber { get; set; }
        public string Description { get; set; }

    }
}
