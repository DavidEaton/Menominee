
namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryPackageItemToRead
    {
        public long Id { get; set; }
        public int Order { get; set; }
        public InventoryItemToRead Item { get; set; }
        public long InventoryItemId { get; set; }
        public double Quantity { get; set; }
        public bool PartAmountIsAdditional { get; set; }
        public bool LaborAmountIsAdditional { get; set; }
        public bool ExciseFeeIsAdditional { get; set; }
    }
}
