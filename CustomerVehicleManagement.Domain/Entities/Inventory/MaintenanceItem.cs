using Menominee.Common;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class MaintenanceItem : Entity
    {
        public long DisplayOrder { get; set; }
        public InventoryItem Item { get; set; }
    }
}
