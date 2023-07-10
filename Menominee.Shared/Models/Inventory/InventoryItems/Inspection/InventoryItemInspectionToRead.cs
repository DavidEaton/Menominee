using Menominee.Shared.Models.Inventory.InventoryItems.Labor;
using Menominee.Common.Enums;

namespace Menominee.Shared.Models.Inventory.InventoryItems.Inspection
{
    public class InventoryItemInspectionToRead
    {
        public long Id { get; set; }
        public LaborAmountToRead LaborAmount { get; set; }
        public TechAmountToRead TechAmount { get; set; }
        public InventoryItemInspectionType Type { get; set; }
    }
}
