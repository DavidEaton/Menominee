namespace Menominee.Shared.Models.Inventory.InventoryItems.Labor
{
    public class InventoryItemLaborToRead
    {
        public long Id { get; set; }
        public LaborAmountToRead LaborAmount { get; set; }
        public TechAmountToRead TechAmount { get; set; }
    }
}
