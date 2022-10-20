using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Warranty
{
    public class InventoryWarrantyToReadInList
    {
        public InventoryItemWarrantyPeriodType PeriodType { get; set; }
        public int Duration { get; set; }
    }
}
