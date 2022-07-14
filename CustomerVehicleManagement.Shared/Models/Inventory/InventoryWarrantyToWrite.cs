using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryWarrantyToWrite
    {
        public InventoryItemWarrantyPeriodType PeriodType { get; set; }
        public int Duration { get; set; }
    }
}
