namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemPartToWrite
    {
        public long Id { get; set; }
        public double List { get; set; }
        public double Cost { get; set; }
        public double Core { get; set; }
        public double Retail { get; set; }
        public TechAmountToWrite TechAmount { get; set; }
        public string LineCode { get; set; }
        public string SubLineCode { get; set; }
        public bool Fractional { get; set; }
    }
}
