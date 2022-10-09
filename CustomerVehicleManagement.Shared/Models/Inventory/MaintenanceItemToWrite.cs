using CustomerVehicleManagement.Domain.Entities.Inventory;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class MaintenanceItemToWrite
    {
        public long Id { get; set; }
        public int DisplayOrder { get; set; }
        public InventoryItemToWrite Item { get; set; }
    }
}
