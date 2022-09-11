using CustomerVehicleManagement.Domain.Enums;
using Menominee.Common;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemInspection : Entity
    {
        public long InventoryItemId { get; set; }
        public ItemLaborType LaborType { get; set; }
        public double LaborAmount { get; set; }
        public ItemLaborType TechPayType { get; set; }
        public double TechPayAmount { get; set; }
        public SkillLevel SkillLevel { get; set; }
        public InventoryItemInspectionType Type { get; set; }

        #region ORM

        // EF requires a parameterless constructor
        public InventoryItemInspection() { }

        #endregion    
    }
}
