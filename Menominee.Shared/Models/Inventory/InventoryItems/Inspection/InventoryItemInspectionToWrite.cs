using Menominee.Shared.Models.Inventory.InventoryItems.Labor;
using Menominee.Common.Enums;

namespace Menominee.Shared.Models.Inventory.InventoryItems.Inspection
{
    public class InventoryItemInspectionToWrite
    {
        public long Id { get; set; }
        public LaborAmountToWrite LaborAmount { get; set; }
        public TechAmountToWrite TechAmount { get; set; }
        public InventoryItemInspectionType Type { get; set; }
    }
}
