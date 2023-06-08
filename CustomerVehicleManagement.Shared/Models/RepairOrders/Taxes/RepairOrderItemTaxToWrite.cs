namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Taxes
{
    public class RepairOrderItemTaxToWrite
    {
        public long Id { get; set; }
        public PartTaxToWrite PartTax { get; set; }
        public LaborTaxToWrite LaborTax { get; set; }
    }
}
