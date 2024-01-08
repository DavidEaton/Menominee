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
    public class InventoryItemToWrite
    {
        public long Id { get; set; }
        public long ManufacturerId { get; set; }
        public ManufacturerToRead Manufacturer { get; set; }
        public string ItemNumber { get; set; }
        public string Description { get; set; }
        public long ProductCodeId { get; set; }
        public ProductCodeToRead ProductCode { get; set; }
        public InventoryItemType ItemType { get; set; }

        public InventoryItemPartToWrite Part { get; set; }
        public InventoryItemLaborToWrite Labor { get; set; }
        public InventoryItemTireToWrite Tire { get; set; }
        public InventoryItemPackageToWrite Package { get; set; }
        public InventoryItemInspectionToWrite Inspection { get; set; }
        public InventoryItemWarrantyToWrite Warranty { get; set; }
    }
}
