namespace CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Package
{
    public class InventoryItemPackageDetailsToRead
    {
        public double Quantity { get; set; }
        public bool PartAmountIsAdditional { get; set; } = false;
        public bool LaborAmountIsAdditional { get; set; } = false;
        public bool ExciseFeeIsAdditional { get; set; } = false;
    }
}