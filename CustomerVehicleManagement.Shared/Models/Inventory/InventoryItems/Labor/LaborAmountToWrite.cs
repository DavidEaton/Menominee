using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Labor
{
    public class LaborAmountToWrite
    {
        public ItemLaborType PayType { get; set; }
        public double Amount { get; set; }
    }
}
