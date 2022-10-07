using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemToWrite
    {
        public long Id { get; set; }
        public ManufacturerToRead Manufacturer { get; set; }
        public string ItemNumber { get; set; }
        public string Description { get; set; }
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
