namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Taxes
{
    public class RepairOrderItemTaxToRead
    {
        public long Id { get; set; }
        public long RepairOrderItemId { get; set; }
        public long TaxId { get; set; }
        public double PartTaxRate { get; set; }
        public double LaborTaxRate { get; set; }
        public double PartTax { get; set; }
        public double LaborTax { get; set; }
    }
}
