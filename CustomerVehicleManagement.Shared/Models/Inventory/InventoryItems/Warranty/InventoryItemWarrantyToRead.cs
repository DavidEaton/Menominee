namespace CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Warranty
{
    public class InventoryItemWarrantyToRead
    {
        public long Id { get; set; }
        public InventoryItemWarrantyPeriodToRead WarrantyPeriod { get; set; }
    }
}
