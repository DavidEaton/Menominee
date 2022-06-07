using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryPackageToRead
    {
        public long Id { get; set; }
        public double BasePartsAmount { get; set; }
        public double BaseLaborAmount { get; set; }
        public string Script { get; set; }
        public bool IsDiscountable { get; set; }
        public List<InventoryPackageItemToRead> Items { get; set; } = new List<InventoryPackageItemToRead>();
        public List<InventoryPackagePlaceholderToRead> Placeholders { get; set; } = new List<InventoryPackagePlaceholderToRead>();
    }
}
