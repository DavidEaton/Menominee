using Menominee.Domain.Enums;

namespace Menominee.Shared.Models.Inventory.InventoryItems.Labor
{
    public class LaborAmountToWrite
    {
        public ItemLaborType PayType { get; set; } = ItemLaborType.None;
        public double Amount { get; set; } = 0.0;
    }
}
