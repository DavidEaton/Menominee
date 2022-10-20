using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Warranty
{
    public class InventoryItemWarrantyToWrite
    {
        public long Id { get; set; }
        public InventoryItemWarrantyPeriodType PeriodType { get; set; }
        public int Duration { get; set; }
    }
}
