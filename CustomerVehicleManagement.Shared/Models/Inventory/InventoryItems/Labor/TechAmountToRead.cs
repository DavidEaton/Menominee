using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Labor
{
    public class TechAmountToRead
    {
        public ItemLaborType PayType { get; set; }
        public double Amount { get; set; }
        public SkillLevel SkillLevel { get; set; }

    }
}
