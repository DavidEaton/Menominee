namespace Menominee.Shared.Models.RepairOrders.Taxes
{
    public class RepairOrderTaxToWrite
    {
        public long Id { get; set; }
        public PartTaxToWrite PartTax { get; set; }
        public LaborTaxToWrite LaborTax { get; set; }
    }
}
