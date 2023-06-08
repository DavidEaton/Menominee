using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Labor
{
    public class LaborAmountToRead
    {
        public ItemLaborType PayType { get; set; }
        public double Amount { get; set; }
    }
}
