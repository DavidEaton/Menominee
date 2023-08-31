using Menominee.Domain.Entities.Inventory;

namespace Menominee.Shared.Models.RepairOrders.LineItems.Item
{
    public class RepairOrderItemLaborToWrite
    {
        public long Id { get; set; }
        public LaborAmount LaborAmount { get; set; }
        public TechAmount TechAmount { get; set; }

    }
}
