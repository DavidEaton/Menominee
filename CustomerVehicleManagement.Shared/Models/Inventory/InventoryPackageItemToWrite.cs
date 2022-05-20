
namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryPackageItemToWrite
    {
        public long Id { get; set; }
        public int Order { get; set; }
        public InventoryItemToWrite Item { get; set; }
        public double Quantity { get; set; }
        public bool PartAmountIsAdditional { get; set; }
        public bool LaborAmountIsAdditional { get; set; }
        public bool ExciseFeeIsAdditional { get; set; }
    }
}
