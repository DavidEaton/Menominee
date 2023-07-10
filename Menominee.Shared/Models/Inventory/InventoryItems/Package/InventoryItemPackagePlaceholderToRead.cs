using Menominee.Common.Enums;

namespace Menominee.Shared.Models.Inventory.InventoryItems.Package
{
    public class InventoryItemPackagePlaceholderToRead
    {
        public long Id { get; set; }
        public int DisplayOrder { get; set; }
        public PackagePlaceholderItemType ItemType { get; set; }
        public InventoryItemPackageDetailsToRead Details { get; set; }
    }
}