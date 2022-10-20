namespace CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Labor
{
    public class InventoryItemLaborToWrite
    {
        public long Id { get; set; }
        public LaborAmountToWrite LaborAmount { get; set; }
        public TechAmountToWrite TechAmount { get; set; }

    }
}
