using Menominee.Common;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryPart : Entity
    {
        public double List { get; set; }
        public double Cost { get; set; }
        public double Core { get; set; }
        public double Retail { get; set; }
        public ItemLaborType TechPayType { get; set; }
        public double TechPayAmount { get; set; }
        public string LineCode { get; set; }
        public string SubLineCode { get; set; }
        public bool Fractional { get; set; }
        public SkillLevel SkillLevel { get; set; }
    }
}
