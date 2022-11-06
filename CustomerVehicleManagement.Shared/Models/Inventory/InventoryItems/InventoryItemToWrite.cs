using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Inspection;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Labor;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Package;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Part;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Tire;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Warranty;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.ProductCodes;

namespace CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems
{
    public class InventoryItemToWrite
    {
        public long Id { get; set; }
        public ManufacturerToRead Manufacturer { get; set; }
        public string ItemNumber { get; set; }
        public string Description { get; set; }
        public ProductCodeToRead ProductCode { get; set; }
        public string ItemType { get; set; }

        public InventoryItemPartToWrite Part { get; set; }
        public InventoryItemLaborToWrite Labor { get; set; }
        public InventoryItemTireToWrite Tire { get; set; }
        public InventoryItemPackageToWrite Package { get; set; }
        public InventoryItemInspectionToWrite Inspection { get; set; }
        public InventoryItemWarrantyToWrite Warranty { get; set; }
    }
}
