namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemLaborToWrite
    {
        public long Id { get; set; }
        public LaborAmountToWrite LaborAmount { get; set; }
        public TechAmountToWrite TechAmount { get; set; }

    }
}
