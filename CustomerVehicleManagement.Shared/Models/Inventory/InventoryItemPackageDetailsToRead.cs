namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemPackageDetailsToRead
    {
        public double Quantity { get; set; }
        public bool PartAmountIsAdditional { get; set; } = false;
        public bool LaborAmountIsAdditional { get; set; } = false;
        public bool ExciseFeeIsAdditional { get; set; } = false;
    }
}