namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Taxes
{
    public class RepairOrderTaxToRead
    {
        public long Id { get; set; }
        public PartTaxToRead PartTax { get; set; }
        public LaborTaxToRead LaborTax { get; set; }
    }
}
