namespace Menominee.Shared.Models.Inventory.InventoryItems.Labor
{
    public class InventoryItemLaborToWrite
    {
        public long Id { get; set; }
        public LaborAmountToWrite LaborAmount { get; set; } = new();
        public TechAmountToWrite TechAmount { get; set; } = new();

    }
}
