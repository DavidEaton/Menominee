using Menominee.Common;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public abstract class InstallablePart : Entity
    {
        public InventoryItem InventoryItem { get; private set; }
        public double List { get; private set; }
        public double Cost { get; private set; }
        public double Core { get; private set; }
        public double Retail { get; private set; }
        public ItemLaborType TechPayType { get; private set; }
        public double TechPayAmount { get; private set; }
        public string LineCode { get; private set; }
        public string SubLineCode { get; private set; }
        public bool Fractional { get; private set; }
        public SkillLevel SkillLevel { get; private set; }
    }
}
