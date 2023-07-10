using Menominee.Shared.Models.Inventory.InventoryItems;

namespace Menominee.Shared.Models.Inventory.MaintenanceItems
{
    public class MaintenanceItemToRead
    {
        public long Id { get; set; }
        public int DisplayOrder { get; set; }
        public InventoryItemToRead Item { get; set; }
    }
}
