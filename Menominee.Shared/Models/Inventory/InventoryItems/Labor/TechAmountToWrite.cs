using Menominee.Domain.Enums;

namespace Menominee.Shared.Models.Inventory.InventoryItems.Labor
{
    public class TechAmountToWrite
    {
        public ItemLaborType PayType { get; set; } = ItemLaborType.None;
        public double Amount { get; set; } = 0.0;
        public SkillLevel SkillLevel { get; set; } = SkillLevel.A;

    }
}
