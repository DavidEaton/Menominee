using CustomerVehicleManagement.Domain.Entities.Inventory;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class MaintenanceItemToRead
    {
        public long Id { get; set; }
        public long DisplayOrder { get; set; }
        public InventoryItemToRead Item { get; set; }
    }
}
