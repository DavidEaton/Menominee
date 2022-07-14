using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryWarrantyToRead
    {
        public long Id { get; set; }
        public InventoryItemWarrantyPeriodType PeriodType { get; set; }
        public int Duration { get; set; }
    }
}
