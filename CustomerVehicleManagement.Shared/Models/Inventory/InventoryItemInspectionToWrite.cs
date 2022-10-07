using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemInspectionToWrite
    {
        public long Id { get; set; }
        public LaborAmountToWrite LaborAmount { get; set; }
        public TechAmountToWrite TechAmount { get; set; }
        public InventoryItemInspectionType Type { get; set; }
    }
}
