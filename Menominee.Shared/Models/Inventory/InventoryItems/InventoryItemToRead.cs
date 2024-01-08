using Menominee.Domain.Enums;
using Menominee.Shared.Models.Inventory.InventoryItems.Inspection;
using Menominee.Shared.Models.Inventory.InventoryItems.Labor;
using Menominee.Shared.Models.Inventory.InventoryItems.Package;
using Menominee.Shared.Models.Inventory.InventoryItems.Part;
using Menominee.Shared.Models.Inventory.InventoryItems.Tire;
using Menominee.Shared.Models.Inventory.InventoryItems.Warranty;
using Menominee.Shared.Models.Manufacturers;
using Menominee.Shared.Models.ProductCodes;

namespace Menominee.Shared.Models.Inventory.InventoryItems
{
    public class InventoryItemToRead
    {
        public long Id { get; set; }
        public ManufacturerToRead Manufacturer { get; set; }
        public string ItemNumber { get; set; }
        public string Description { get; set; }
        public ProductCodeToRead ProductCode { get; set; }
        public InventoryItemType ItemType { get; set; }
        public InventoryItemPartToRead Part { get; set; }
        public InventoryItemLaborToRead Labor { get; set; }
        public InventoryItemTireToRead Tire { get; set; }
        public InventoryItemPackageToRead Package { get; set; }
        public InventoryItemInspectionToRead Inspection { get; set; }
        public InventoryItemWarrantyToRead Warranty { get; set; }
    }
}
