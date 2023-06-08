namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Taxes
{
    public class RepairOrderItemTaxToRead
    {
        public long Id { get; set; }
        public PartTaxToRead PartTax { get; set; }
        public LaborTaxToRead LaborTax { get; set; }
    }
}
