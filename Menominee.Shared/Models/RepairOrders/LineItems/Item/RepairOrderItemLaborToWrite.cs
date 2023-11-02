using Menominee.Shared.Models.Inventory.InventoryItems.Labor;

namespace Menominee.Shared.Models.RepairOrders.LineItems.Item
{
    public class RepairOrderItemLaborToWrite
    {
        public long Id { get; set; }
        public LaborAmountToWrite LaborAmount { get; set; }
        public TechAmountToWrite TechAmount { get; set; }

    }
}
