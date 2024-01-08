using Menominee.Domain.Enums;
using Menominee.Shared.Models.Inventory.InventoryItems.Labor;

namespace Menominee.Shared.Models.Inventory.InventoryItems.Inspection
{
    public class InventoryItemInspectionToWrite
    {
        public long Id { get; set; }
        public LaborAmountToWrite LaborAmount { get; set; } = new();
        public TechAmountToWrite TechAmount { get; set; } = new();
        public InventoryItemInspectionType Type { get; set; }
    }
}
