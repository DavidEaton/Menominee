using Menominee.Common.Enums;

namespace Menominee.Shared.Models.Inventory.InventoryItems.Package
{
    public class InventoryItemPackagePlaceholderToWrite
    {
        public long Id { get; set; }
        public PackagePlaceholderItemType ItemType { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public InventoryItemPackageDetailsToWrite Details { get; set; }
    }
}
