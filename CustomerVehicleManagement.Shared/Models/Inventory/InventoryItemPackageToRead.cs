using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemPackageToRead
    {
        public long Id { get; set; }
        public double BasePartsAmount { get; set; }
        public double BaseLaborAmount { get; set; }
        public string Script { get; set; }
        public bool IsDiscountable { get; set; }
        public List<InventoryItemPackageItemToRead> Items { get; set; } = new List<InventoryItemPackageItemToRead>();
        public List<InventoryItemPackagePlaceholderToRead> Placeholders { get; set; } = new List<InventoryItemPackagePlaceholderToRead>();
    }
}
