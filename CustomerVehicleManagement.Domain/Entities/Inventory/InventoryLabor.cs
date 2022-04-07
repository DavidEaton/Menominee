using Menominee.Common;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryLabor : Entity
    {
        public ItemLaborType LaborType { get; set; }
        public double LaborAmount { get; set; }
        public ItemLaborType TechPayType { get; set; }
        public double TechPayAmount { get; set; }
        public SkillLevel SkillLevel { get; set; }
    }
}
