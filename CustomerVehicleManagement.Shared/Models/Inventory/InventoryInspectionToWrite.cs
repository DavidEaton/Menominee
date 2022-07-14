using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryInspectionToWrite
    {
        public ItemLaborType LaborType { get; set; }
        public double LaborAmount { get; set; }
        public ItemLaborType TechPayType { get; set; }
        public double TechPayAmount { get; set; }
        public SkillLevel SkillLevel { get; set; }
        public InventoryItemInspectionType Type { get; set; }
    }
}
