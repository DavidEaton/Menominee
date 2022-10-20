namespace CustomerVehicleManagement.Shared.Models.Inventory.MaintenanceItems
{
    public class MaintenanceItemToWrite
    {
        public long Id { get; set; }
        public int DisplayOrder { get; set; }
        public InventoryItemToWrite Item { get; set; }
    }
}
