namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemWarrantyToRead
    {
        public long Id { get; set; }
        public InventoryItemWarrantyPeriodToRead WarrantyPeriod { get; set; }
    }
}
