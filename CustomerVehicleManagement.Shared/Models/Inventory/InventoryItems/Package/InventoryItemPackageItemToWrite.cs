namespace CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Package
{
    public class InventoryItemPackageItemToWrite
    {
        public long Id { get; set; }
        public int DisplayOrder { get; set; }
        public InventoryItemToWrite Item { get; set; }
        public double Quantity { get; set; }
        public bool PartAmountIsAdditional { get; set; } = false;
        public bool LaborAmountIsAdditional { get; set; } = false;
        public bool ExciseFeeIsAdditional { get; set; } = false;
    }
}
