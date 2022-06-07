using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryPackageToWrite
    {
        public double BasePartsAmount { get; set; }
        public double BaseLaborAmount { get; set; }
        public string Script { get; set; }
        public bool IsDiscountable { get; set; }
        public List<InventoryPackageItemToWrite> Items { get; set; } = new List<InventoryPackageItemToWrite>();
        public List<InventoryPackagePlaceholderToWrite> Placeholders { get; set; } = new List<InventoryPackagePlaceholderToWrite>();
    }
}
