using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Labor;

namespace CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Tire
{
    public class InventoryItemTireToReadInList
    {
        public long Id { get; set; }
        public double List { get; set; }
        public double Cost { get; set; }
        public double Core { get; set; }
        public double Retail { get; set; }
        public TechAmountToRead TechAmount { get; set; }
        public string LineCode { get; set; }
        public string SubLineCode { get; set; }
        public bool Fractional { get; set; }
        public string Type { get; set; }
        public int Width { get; set; }
        public int AspectRatio { get; set; }
        public string ConstructionType { get; set; }
        public double Diameter { get; set; }
        public int LoadIndex { get; set; }
        public string SpeedRating { get; set; }
    }
}
