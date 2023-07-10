using Menominee.Common.Enums;

namespace Menominee.Shared.Models.Inventory.InventoryItems.Warranty
{
    public class InventoryItemWarrantyPeriodToRead
    {
        public InventoryItemWarrantyPeriodType PeriodType { get; set; }
        public int Duration { get; set; }
    }
}