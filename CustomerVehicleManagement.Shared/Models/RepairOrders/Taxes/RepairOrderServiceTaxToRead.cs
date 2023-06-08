namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Taxes
{
    public class RepairOrderServiceTaxToRead
    {
        public long Id { get; set; }
        public PartTaxToRead PartTax { get; set; }
        public LaborTaxToRead LaborTax { get; set; }
    }
}
