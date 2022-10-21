using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems;

namespace CustomerVehicleManagement.Shared.Models.Inventory.MaintenanceItems
{
    public class MaintenanceItemToRead
    {
        public long Id { get; set; }
        public int DisplayOrder { get; set; }
        public InventoryItemToRead Item { get; set; }
    }
}
