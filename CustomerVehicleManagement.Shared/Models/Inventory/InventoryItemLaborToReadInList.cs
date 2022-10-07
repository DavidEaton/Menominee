namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemLaborToReadInList
    {
        public long Id { get; set; }
        public LaborAmountToRead LaborAmount { get; set; }
        public TechAmountToRead TechPayAmount { get; set; }

    }
}
