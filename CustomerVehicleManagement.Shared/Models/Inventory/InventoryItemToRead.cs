namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemToRead
    {
        public long Id { get; set; }
        public string MfrId { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public string PartType { get; set; }
        public double Retail { get; set; }
        public double Cost { get; set; }
        public double Core { get; set; }
        public double Labor { get; set; }
        public double OnHand { get; set; }
    }
}
