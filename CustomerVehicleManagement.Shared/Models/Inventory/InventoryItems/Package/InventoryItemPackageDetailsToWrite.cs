namespace CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Package
{
    public class InventoryItemPackageDetailsToWrite
    {
        public double Quantity { get; set; }
        public bool PartAmountIsAdditional { get; set; } = false;
        public bool LaborAmountIsAdditional { get; set; } = false;
        public bool ExciseFeeIsAdditional { get; set; } = false;
    }
}