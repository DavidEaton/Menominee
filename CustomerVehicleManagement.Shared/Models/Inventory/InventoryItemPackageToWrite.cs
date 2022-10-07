using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemPackageToWrite
    {
        public long Id { get; set; }
        public double BasePartsAmount { get; set; }
        public double BaseLaborAmount { get; set; }
        public string Script { get; set; }
        public bool IsDiscountable { get; set; }
        public List<InventoryItemPackageItemToWrite> Items { get; set; } = new List<InventoryItemPackageItemToWrite>();
        public List<InventoryItemPackagePlaceholderToWrite> Placeholders { get; set; } = new List<InventoryItemPackagePlaceholderToWrite>();
    }
}
